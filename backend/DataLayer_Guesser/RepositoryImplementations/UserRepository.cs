using DataLayer_Guesser.AdditionalModels;
using DataLayer_Guesser.Models;
using DataLayer_Guesser.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared_Guesser;
using Shared_Guesser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using static Shared_Guesser.Helpers.ExtensionHelper;

namespace DataLayer_Guesser.RepositoryImplementations
{
    public class UserRepository : IUserRepository
    {
        private readonly GuesserDBContext _context;
        private string frontURL;

        public UserRepository(GuesserDBContext context, IConfiguration _config)
        {
            _context = context;
            frontURL = _config["URLS:Front"];
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
            
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Email.ToLower().Equals(email.ToLower()));

            return user;
        }

        public async Task<PaginatedData<User>> GetUsers(UserConfigs config)
        {
            var query = _context.Users.AsQueryable();

            //Other filter params
            if (!string.IsNullOrEmpty(config.Email) && config.Email.Length > 2)
            {
                query.Where(user => user.Email.Contains(config.Email));
            }
            if (!string.IsNullOrEmpty(config.FirstName))
            {
                query.Where(user => user.FirstName.Contains(config.FirstName));
            }
            if (!string.IsNullOrEmpty(config.LastName))
            {
                query.Where(user => user.LastName.Contains(config.LastName));
            }
            if (!string.IsNullOrEmpty(config.NickName))
            {
                query.Where(user => user.LastName.Contains(config.NickName));
            }
            if (!string.IsNullOrEmpty(config.FullName))
            {
                query.Where(user => user.FullName.Contains(config.FullName));
            }

            int totalUserCount = query.Count();
            DataProviderHelper<User> paginatedResult = new DataProviderHelper<User>();
            if (config.CurrentPage <= 0)
            {
                config.CurrentPage = 1;
            }
            if (config.ItemsOnPage <= 0)
            {
                config.ItemsOnPage = 10;
            }

            int itemsToSkip = paginatedResult.GetItemsToSkip(totalUserCount, config.CurrentPage, config.ItemsOnPage);
            int itemsToTake = paginatedResult.GetItemsToTake(totalUserCount, config.CurrentPage, config.ItemsOnPage);

            return new DataProviderHelper<User>().GetPaginatedData(await query.Skip(itemsToSkip).Take(itemsToTake).ToListAsync(), config.CurrentPage, config.ItemsOnPage, totalUserCount);
        }

        public async Task<User> CreateUser(User userToCreate)
        {
            var oldUser = await GetUserByEmail(userToCreate.Email);
            if (oldUser != null)
            {
                if (oldUser.UserStatus == UserStatus.Registered)
                {
                    var activateModel = JsonConvert.DeserializeObject<UserDetailsModel>(oldUser.Details);
                    if (!string.IsNullOrEmpty(activateModel.ActivationCode) && DateTime.UtcNow <= activateModel.ActivationCodeExpire)
                    {
                        throw new BadRequestException($"User with Email {oldUser.Email} is registered, but not activated.\nCheck your mail to finish registering process.");
                    }

                    await SendMessage(oldUser.Email, MessageTypes.AccountActivation);
                    throw new BadRequestException($"User with Email {oldUser.Email} is registered, but not activated.\nActivation letter resended.");
                }
                else if (oldUser.UserStatus == UserStatus.Active)
                {
                    throw new BadRequestException($"User with Email {oldUser.Email} is registered");
                }
                else if (oldUser.UserStatus == UserStatus.Blocked)
                {
                    throw new BadRequestException($"User with Email {oldUser.Email} is blocked");
                }
            }

            userToCreate.UserType = UserType.Player;
            userToCreate.UserStatus = UserStatus.Registered;
            userToCreate.Details = JsonConvert.SerializeObject(new UserDetailsModel());
            userToCreate.Password = ExtensionHelper.HashPassword(userToCreate.Password);

            await _context.Users.AddAsync(userToCreate);
            await _context.SaveChangesAsync();
            await SendMessage(userToCreate.Email, MessageTypes.AccountActivation);
            return await GetUserById(userToCreate.Id);
        }

        public async Task<User> UpdateUser(User userToUpdate)
        {
            User currentUser = await GetUserById(userToUpdate.Id);
            if (currentUser == null)
            {
                throw new NotFoundException("User not found.");
            }
            currentUser.FullName = userToUpdate.FullName;
            currentUser.NickName = userToUpdate.NickName;
            currentUser.FirstName = userToUpdate.FirstName;
            currentUser.LastName = userToUpdate.LastName;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            return await GetUserById(userToUpdate.Id);
        }

        public async Task<bool> UpdateUserStatus(int userId, UserStatus newStatus)
        {
            User currentUser = await GetUserById(userId);
            if (currentUser == null)
            {
                throw new NotFoundException("User not found.");
            }
            currentUser.UserStatus = newStatus;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserPassword(int userId, string newPassword, string userDetails)
        {
            User currentUser = await GetUserById(userId);
            if (currentUser == null)
            {
                throw new NotFoundException("User not found.");
            }
            currentUser.Password = newPassword;
            if (!string.IsNullOrEmpty(userDetails))
            {
                currentUser.Details = userDetails;
            }
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserDetails (int userId, string userDetails)
        {
            User currentUser = await GetUserById(userId);
            currentUser.Details = userDetails;
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUser(int userId)
        {
            User userToDelete = await GetUserById(userId);
            if (userToDelete == null)
            {
                throw new BadRequestException("User not found.");
            }
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendMessage(string email, MessageTypes messageType)
        {
            var user = await GetUserByEmail(email);

            string code;
            string messageHTML;
            UserDetailsModel userDetailsModel = JsonConvert.DeserializeObject<UserDetailsModel>(user.Details);

            if (messageType == MessageTypes.ForgetPassword)
            {
                if (userDetailsModel.LastPasswordRestoringMessageDate != null &&
                    userDetailsModel.LastPasswordRestoringMessageDate == DateTime.Today &&
                    userDetailsModel.PasswordRestoringMessagePerDayCounter.HasValue &&
                    userDetailsModel.PasswordRestoringMessagePerDayCounter == 10)
                {
                    throw new BadRequestException($"Daily message limit for email {user.Email} is over, please try again tomorrow.");
                }
                else
                {
                    code = RandomNumberGenerator.RandomCode();
                    userDetailsModel.RestorePasswordCode = code;
                    userDetailsModel.RestorePasswordCodeExpire = DateTime.UtcNow.AddDays(2);
                    userDetailsModel.PasswordRestoringMessagePerDayCounter = userDetailsModel.PasswordRestoringMessagePerDayCounter.HasValue ?
                            userDetailsModel.PasswordRestoringMessagePerDayCounter + 1 : 1;
                    userDetailsModel.LastPasswordRestoringMessageDate = DateTime.Today;

                    await UpdateUserDetails(user.Id, JsonConvert.SerializeObject(userDetailsModel));
                    var option = new Dictionary<string, string>
                    { 
                        ["frontURL"] = frontURL, 
                        ["code"] = userDetailsModel.RestorePasswordCode, 
                        ["email"] = user.Email
                    };
                    return ExtensionHelper.SendEmail(email, user.FullName, messageType, option);
                }
            }
            else if (messageType == MessageTypes.AccountActivation)
            {
                if (userDetailsModel.LastActivationMessageDate != null &&
                    userDetailsModel.LastActivationMessageDate == DateTime.Today &&
                    userDetailsModel.ActivationMessagePerDayCounter.HasValue &&
                    userDetailsModel.ActivationMessagePerDayCounter == 10)
                {
                    throw new BadRequestException($"Daily message limit for email {user.Email} is over, please try again tomorrow.");
                }
                else
                {
                    code = RandomNumberGenerator.RandomCode();
                    userDetailsModel.ActivationCode = code;
                    userDetailsModel.ActivationCodeExpire = DateTime.UtcNow.AddDays(2);
                    userDetailsModel.ActivationMessagePerDayCounter = userDetailsModel.ActivationMessagePerDayCounter.HasValue ?
                            userDetailsModel.ActivationMessagePerDayCounter + 1 : 1;
                    userDetailsModel.LastActivationMessageDate = DateTime.Today;

                    await UpdateUserDetails(user.Id, JsonConvert.SerializeObject(userDetailsModel));
                    var option = new Dictionary<string, string>
                    {
                        ["frontURL"] = frontURL,
                        ["code"] = userDetailsModel.ActivationCode,
                        ["id"] = user.Id.ToString()
                    };
                    return ExtensionHelper.SendEmail(email, user.FullName, messageType, option);
                }
            }

            return false;
        }
    }
}

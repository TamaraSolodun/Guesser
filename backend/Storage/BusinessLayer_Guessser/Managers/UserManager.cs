using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataLayer_Guesser.Models;
using Shared_Guesser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using DataLayer_Guesser.RepositoryInterfaces;
using Shared_Guesser.Helpers;
using DataLayer_Guesser.AdditionalModels;
using BusinessLayer_Guesser.DTO;
using AutoMapper;
using BusinessLayer_Guesser.Helpers;
using Microsoft.Extensions.Configuration;
using BusinessLayer_Guesser.DTO.Requests;

namespace BusinessLayer_Guesser.Managers
{
    public class UserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        

        public UserManager(IUserRepository userRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
            
        }

        #region User search

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = _mapper.Map<UserResponse>(await _userRepository.GetUserById(id));
            if (user == null)
            {
                throw new NotFoundException($"User with provided Id: {id} not found.");
            }
            return user;
        }

        public async Task<UserResponse> GetUserByEmail(string email)
        {
            var user = _mapper.Map<UserResponse>(await _userRepository.GetUserByEmail(email));
            if (user == null)
            {
                throw new NotFoundException($"User with provided email: {email} not found.");
            }
            return user;
        }

        public async Task<PaginatedData<UserResponse>> GetUsers(UserConfigs config)
        {
            PaginatedData<User> userPaged = await _userRepository.GetUsers(config);
            return new PaginatedData<UserResponse>()
            {
                CurrentPage = userPaged.CurrentPage,
                RecordsPerPage = userPaged.RecordsPerPage,
                RecordsReturned = userPaged.RecordsReturned,
                TotalRecordsFound = userPaged.TotalRecordsFound,
                Data = _mapper.Map<List<UserResponse>>(userPaged.Data)
            };
        }

        #endregion

        #region User management

        public async Task<bool> SetUserStatus(int userId, UserStatus userStatus, string token, int requesterTypeId)
        {
            var user = (await _userRepository.GetUserById(userId)) ??
                throw new NotFoundException("User not found");
            if (!string.IsNullOrWhiteSpace(token)) {
                if (await ValidateUserSecretCode(user, token, "", CodeValidationType.AccountActivationCode))
                {
                    var details = JsonConvert.DeserializeObject<UserDetailsModel>(user.Details);
                    details.ActivationCode = "";
                    details.ActivationCodeExpire = DateTime.MinValue;
                    await _userRepository.UpdateUserDetails(userId, JsonConvert.SerializeObject(details));
                    await _userRepository.UpdateUserStatus(userId, UserStatus.Active);
                }
                else
                {
                    throw new BadRequestException("Password recovery code is incorrect or has expired.");
                }
            }
            else
            {
                if (requesterTypeId != 3)
                {
                    throw new BadRequestException("Only admin can change user status.");
                }
                else
                {
                    await _userRepository.UpdateUserStatus(userId, userStatus);
                }
                
            }
            
            return true;
        }

        public async Task<AuthenticationResponse> LoginUser(UserLoginModel loginModel)
        {
            var user = (await _userRepository.GetUserByEmail(loginModel.Email)) ??
                throw new NotFoundException("User not found");

            if (user.UserStatus == UserStatus.Registered || user.UserStatus == UserStatus.Disabled)
            {
                var activateModel = JsonConvert.DeserializeObject<UserDetailsModel>(user.Details);
                if (!string.IsNullOrEmpty(activateModel.ActivationCode) && DateTime.UtcNow <= activateModel.ActivationCodeExpire)
                {
                    throw new BadRequestException($"User with Email {user.Email} is registered, but not activated.\nCheck mail to activate user.");
                }
                else
                {
                    throw new BadRequestException($"User with Email {user.Email} is registered, but not activated.\nRequest new email to activate.");
                }
            }
            else if (user.UserStatus == UserStatus.Blocked)
            {
                throw new BadRequestException($"User with Email {user.Email} is blocked");
            }

            if (ExtensionHelper.VerifyHashedPassword(user.Password, loginModel.Password))
            {
                AuthenticationResponse authenticationResponse = new AuthenticationResponse
                {
                    UserInfo = _mapper.Map<UserResponse>(user)
                };
                authenticationResponse.Token = "Bearer " + TokenHelper.GenerateJSONWebToken(authenticationResponse.UserInfo, _config);
                return authenticationResponse;
            }
            else
            {
                throw new BadRequestException("Provided user password is incorrect or other error is occurred.");
            }
        }

        public async Task<UserResponse> RegisterUser(UserRequest user)
        {
            var registeredUser = _mapper.Map<UserResponse>(await _userRepository.CreateUser(_mapper.Map<User>(user)));
            return registeredUser;
        }

        //TODO: Review
        public async Task<UserResponse> UpdateUser(User user)
        {
            return _mapper.Map<UserResponse>(await _userRepository.UpdateUser(user));
        }

        public async Task<bool> DeleteUser(int userId)
        {
            return await _userRepository.DeleteUser(userId);
        }

        public async Task<bool> ChangePassword(int userId, string oldPass, string newPass)
        {
            if (!ExtensionHelper.ValidatePassword(newPass))
            {
                throw new BadRequestException("User password must be at least 8 characters lenght and also contains at least one upper and lower case letters and at least one number.");
            }
            var user = (await _userRepository.GetUserById(userId)) ??
                throw new NotFoundException("User not found");
            if (ExtensionHelper.VerifyHashedPassword(user.Password, oldPass))
            {
                await _userRepository.UpdateUserPassword(userId, ExtensionHelper.HashPassword(newPass), null);
            }
            else
            {
                throw new BadRequestException($"Current password is incorrect.");
            }

            return true;
        }

        public async Task<bool> ChangePasswordUsingEmail(string email, string newPass, string retorePassCode)
        {
            var user = (await _userRepository.GetUserByEmail(email)) ??
                throw new NotFoundException("User not found");

            if (await ValidateUserSecretCode(user, retorePassCode))
            {
                var details = JsonConvert.DeserializeObject<UserDetailsModel>(user.Details);
                details.RestorePasswordCode = "";
                details.RestorePasswordCodeExpire = DateTime.MinValue;
                await _userRepository.UpdateUserPassword(user.Id, ExtensionHelper.HashPassword(newPass), JsonConvert.SerializeObject(details));
            }
            else
            {
                throw new BadRequestException("Password recovery code is incorrect or has expired.");
            }

            return true;
        }

        public async Task<bool> ForgetPassword(string email)
        {
            var user = (await _userRepository.GetUserByEmail(email)) ??
                throw new NotFoundException("User not found.");

            UserDetailsModel restoringModel = JsonConvert.DeserializeObject<UserDetailsModel>(user.Details);
            if (!string.IsNullOrEmpty(restoringModel.RestorePasswordCode) && DateTime.UtcNow <= restoringModel.RestorePasswordCodeExpire)
            {
                throw new BadRequestException($"Message with valid restore password link already sended to {user.Email}");
            }

            restoringModel.RestorePasswordCode = RandomNumberGenerator.RandomCode();
            restoringModel.RestorePasswordCodeExpire = DateTime.UtcNow.AddDays(2);
            await _userRepository.UpdateUserDetails(user.Id, JsonConvert.SerializeObject(restoringModel));

            return await SendMessage(email, MessageTypes.ForgetPassword);
        }

        public async Task<bool> ValidateUserSecretCode(User user, string codeToValidate, string email = "", CodeValidationType validationType = CodeValidationType.RestorePassCode)
        {
            if (user == null)
            {
                user = (await _userRepository.GetUserByEmail(email)) ??
                throw new NotFoundException("User not found");
            }
            UserDetailsModel restoringModel = JsonConvert.DeserializeObject<UserDetailsModel>(user.Details);
            if (validationType == CodeValidationType.AccountActivationCode)
            {
                return (restoringModel.ActivationCode.Equals(codeToValidate) && DateTime.UtcNow <= restoringModel.ActivationCodeExpire);
            }
            else
            {
                return (restoringModel.RestorePasswordCode.Equals(codeToValidate) && DateTime.UtcNow <= restoringModel.RestorePasswordCodeExpire);
            }
            
        }

        public async Task<bool> SendMessage(string email, MessageTypes messageType)
        {
            return await _userRepository.SendMessage(email, messageType);
        }

        /*public async Task<User> SetUserImage(int userId, string objectKey)
        {
            User user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new BadRequestException($"Can not find user with provided Id {userId} in database.");
            }
            user.GeneratedAvatarUrl = avatarImageStorageUrl + "/" + avatareImageBucketName + "/" + objectKey;
            await _userRepository.UpdateAsync(user);
            return user;
        }*/

        #endregion
    }
}

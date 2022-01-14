using AutoMapper;
using BusinessLayer_Guesser.DTO;
using BusinessLayer_Guesser.DTO.Requests;
using BusinessLayer_Guesser.Managers;
using DataLayer_Guesser.AdditionalModels;
using DataLayer_Guesser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shared_Guesser;
using Shared_Guesser.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api_Guesser.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager _userManager;

        public UserController(ILogger<UserController> logger, IConfiguration config,UserManager userManager)
        {
            _config = config;
            _userManager = userManager;
            _logger = logger;
        }

        #region UserRegion

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> SetUserStatus([FromBody, Required] SetUserStatusRequest request)
        {
            _logger.LogInformation($"Calling {nameof(SetUserStatus)}. request: {JsonConvert.SerializeObject(request)}");
            var response = await _userManager.SetUserStatus(request.Id.Value, request.UserStatus.Value, request.TokenToActivate, User.GetUserTypeId().HasValue ? User.GetUserTypeId().Value : 0);
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById()
        {
            _logger.LogInformation($"Calling {nameof(GetUserById)}. userId: {User.GetUserId()}");

            UserResponse user = await _userManager.GetUserById(User.GetUserId()) ??
                throw new NotFoundException("User not found.");

            return Ok(user);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(PaginatedData<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromBody, Required] UserConfigs config)
        {
            _logger.LogInformation($"Calling {nameof(GetUsers)}. config: {JsonConvert.SerializeObject(config)}");

            PaginatedData<UserResponse> foundData = await _userManager.GetUsers(config);

            return Ok(foundData);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserRegister([FromBody, Required] UserRequest request)
        {
            _logger.LogInformation($"Calling {nameof(UserRegister)}. UserRequest: {JsonConvert.SerializeObject(request)}");

            UserResponse registeredUser = await _userManager.RegisterUser(request);
            return Ok(registeredUser);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser([FromQuery, Required] int userId)
        {
            _logger.LogInformation($"Calling {nameof(DeleteUser)}. userId: {User.GetUserId()}, userId: {userId}");

            if (User.GetUserTypeId() != 3)
            {
                throw new NoRightsException("Only house can delete user.");
            }

            var response = await _userManager.DeleteUser(userId);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgetPassword([FromBody, Required] ForgetPasswordRequest request)
        {
            _logger.LogInformation($"Calling {nameof(ForgetPassword)}. request: {JsonConvert.SerializeObject(request)}");

            var response = await _userManager.ForgetPassword(request.Email);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateRestorePassCode([FromBody, Required] ValidateRestorePassCodeRequest request)
        {
            _logger.LogInformation($"Calling {nameof(ValidateRestorePassCode)}. request: {JsonConvert.SerializeObject(request)}");

            var response = await _userManager.ValidateUserSecretCode(null, request.Email, request.RestorePassCode, CodeValidationType.RestorePassCode);
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody, Required] ChangePasswordRequest request)
        {
            _logger.LogInformation($"Calling {nameof(ChangePassword)}. userId: {User.GetUserId()}, request: {JsonConvert.SerializeObject(request)}");

            var response = await _userManager.ChangePassword(request.UserId.Value, request.OldPass, request.NewPass);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePasswordUsingEmail([FromBody, Required] ChangePasswordUsingEmailRequest request)
        {
            _logger.LogInformation($"Calling {nameof(ChangePasswordUsingEmail)}. request: {JsonConvert.SerializeObject(request)}");

            var response = await _userManager.ChangePasswordUsingEmail(request.Email, request.NewPass, request.RestorePassCode);
            return Ok(response);
        }


        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody, Required] UserLoginModel model)
        {
            _logger.LogInformation($"Calling {nameof(Login)}. model: {JsonConvert.SerializeObject(model.Email)}");

            return Ok(await _userManager.LoginUser(model));
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Tags = new[] { "UserEndpoints" })]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendMessage([FromQuery, Required] string email, [FromQuery, Required] MessageTypes messageType)
        {
            _logger.LogInformation($"Calling {nameof(SendMessage)}. email: {email}, messageType: {messageType}");

            var response = await _userManager.SendMessage(email, messageType);
            return Ok(response);
        }

        #endregion
    }
}

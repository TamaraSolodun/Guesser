using BusinessLayer_Guesser.DTO.Requests;
using DataLayer_Guesser;
using FluentValidation;
using Shared_Guesser.Helpers;
using System;
using System.Linq;

namespace Api_Guesser.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator(GuesserDBContext context)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name should be provided.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Last name should be provided.");
            RuleFor(x => x.NickName).NotEmpty().WithMessage("Nick name should be provided.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email address must be valid and not empty.");
            RuleFor(x => x.Password).NotEmpty().Must(i => ExtensionHelper.ValidatePassword(i)).WithMessage("Password must be valid and not empty.");
        }
    }
}

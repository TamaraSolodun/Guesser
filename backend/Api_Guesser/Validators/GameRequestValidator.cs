using DataLayer_Guesser;
using DataLayer_Guesser.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Guesser.Validators
{
    public class GameRequestValidator : AbstractValidator<Game>
    {
        public GameRequestValidator(GuesserDBContext context)
        {
            RuleFor(x => x.GameResultType).NotEmpty().WithMessage("Game result must be not empty.");
            RuleFor(x => x.PlayerId).NotEmpty().WithMessage("Player Id should be provided.").Must(i => context.Users.Any(a => a.Id == i)).WithMessage("User with provided PlayerId should be exists.");
        }
    }
}

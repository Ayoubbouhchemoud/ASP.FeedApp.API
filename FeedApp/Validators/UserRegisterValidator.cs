using FluentValidation;
using static ASP.FeedApp.API.Controllers.UserController;
using ASP.FeedApp.API.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace ASP.FeedApp.API.Validators
{

    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^\D.*$").WithMessage("Username cannot start with a number.")
                .Matches(@"^\S*$").WithMessage("Username cannot contain spaces.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[!@#$%^&*(),.?""\{\}\[\]<>]").WithMessage("Password must contain at least one special character.")
                .Matches(@"[a-zA-Z]").WithMessage("Password must contain at least one letter.");
        }
    }
}
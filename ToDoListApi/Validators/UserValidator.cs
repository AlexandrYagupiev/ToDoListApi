using FluentValidation;
using ToDoListAPI.Models;

namespace ToDoListAPI.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        // Правила валидации для поля Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Электронная почта не может быть пустой")
            .EmailAddress().WithMessage("Некорректный формат электронной почты");

        // Правила валидации для поля Password
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль не может быть пустым")
            .MinimumLength(6).WithMessage("Минимальная длина пароля — 6 символов");
    }
}
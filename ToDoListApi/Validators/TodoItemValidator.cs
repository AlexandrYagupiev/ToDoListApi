using FluentValidation;
using ToDoListAPI.Models;

namespace ToDoListAPI.Validators;

public class TodoItemValidator : AbstractValidator<TodoItem>
{
    public TodoItemValidator()
    {
        // Правила валидации для поля Title
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок задачи не может быть пустым")
            .MaximumLength(50).WithMessage("Максимальная длина заголовка — 50 символов");

        // Правила валидации для поля Description
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Описание не должно превышать 250 символов");

    }
}
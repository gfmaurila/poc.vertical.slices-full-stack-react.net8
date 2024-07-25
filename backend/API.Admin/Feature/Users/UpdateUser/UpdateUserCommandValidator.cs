﻿using FluentValidation;
using poc.core.api.net8.Enumerado;

namespace API.Admin.Feature.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Gender)
            .Must(gender => gender != EGender.None)
            .WithMessage("Selecione um gênero válido. 'Não informar' não é uma opção permitida.");

        RuleFor(command => command.DateOfBirth)
        .NotEmpty()
        .WithMessage("Data de nascimento é obrigatória.")
        .LessThan(DateTime.Today)
        .WithMessage("Data de nascimento deve ser uma data passada.")
        .LessThan(DateTime.Today) // Isso garante que a data de nascimento não é uma data futura.
        .Must(BeAtLeast18YearsOld) // Isso garante que o usuário tem pelo menos 18 anos de idade.
        .WithMessage("O usuário deve ter pelo menos 18 anos de idade.");
    }

    private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        return dateOfBirth <= DateTime.Today.AddYears(-18);
    }
}

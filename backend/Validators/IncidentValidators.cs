using FluentValidation;
using IncidentTracker.Api.DTOs;

namespace IncidentTracker.Api.Validators;

public class CreateIncidentRequestValidator : AbstractValidator<CreateIncidentRequest>
{
    public CreateIncidentRequestValidator()
    {
        RuleFor(x => x.title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.service)
            .NotEmpty().WithMessage("Service is required")
            .MaximumLength(100).WithMessage("Service must not exceed 100 characters");

        RuleFor(x => x.severity)
            .IsInEnum().WithMessage("Invalid severity level");

        RuleFor(x => x.status)
            .IsInEnum().WithMessage("Invalid status");

        RuleFor(x => x.owner)
            .MaximumLength(100).WithMessage("Owner must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.owner));

        RuleFor(x => x.summary)
            .MaximumLength(2000).WithMessage("Summary must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.summary));
    }
}

public class UpdateIncidentRequestValidator : AbstractValidator<UpdateIncidentRequest>
{
    public UpdateIncidentRequestValidator()
    {
        RuleFor(x => x.title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.title));

        RuleFor(x => x.service)
            .MaximumLength(100).WithMessage("Service must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.service));

        RuleFor(x => x.severity)
            .IsInEnum().WithMessage("Invalid severity level")
            .When(x => x.severity.HasValue);

        RuleFor(x => x.status)
            .IsInEnum().WithMessage("Invalid status")
            .When(x => x.status.HasValue);

        RuleFor(x => x.owner)
            .MaximumLength(100).WithMessage("Owner must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.owner));

        RuleFor(x => x.summary)
            .MaximumLength(2000).WithMessage("Summary must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.summary));
    }
}
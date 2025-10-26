using FluentValidation;

namespace Api.Features.Orders.Delete;

public sealed class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");
    }
}

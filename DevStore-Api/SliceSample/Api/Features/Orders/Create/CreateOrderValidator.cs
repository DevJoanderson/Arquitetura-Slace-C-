using FluentValidation;

namespace Api.Features.Orders.Create;

// Regras de validação separadas (limpas e testáveis).
public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code é obrigatório.")
            .MaximumLength(20);

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName é obrigatório.")
            .MaximumLength(80);

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("TotalAmount deve ser > 0.");
    }
}

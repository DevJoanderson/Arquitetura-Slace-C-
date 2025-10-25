using FluentValidation;

namespace Api.Features.Orders.List;

public sealed class ListOrdersValidator : AbstractValidator<ListOrdersQuery>
{
    public ListOrdersValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

        RuleFor(x => x.SortBy)
            .Must(s => new[] { "createdAt", "code", "customerName", "totalAmount" }.Contains((s ?? "").ToLower()))
            .WithMessage("SortBy deve ser: createdAt | code | customerName | totalAmount");

        RuleFor(x => x.SortDir)
            .Must(s => new[] { "asc", "desc" }.Contains((s ?? "").ToLower()))
            .WithMessage("SortDir deve ser: asc | desc");
    }
}

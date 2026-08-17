using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(temp => temp.ProductID)
            .NotEmpty()
            .WithMessage("Product ID can't be blank");
            
        RuleFor(temp => temp.ProductName)
            .NotEmpty()
            .WithMessage("Product Name can't be blanc");

        RuleFor(temp => temp.Category)
            .IsInEnum()
            .WithMessage("Category can't be blanc");

        RuleFor(temp => temp.UnitPrice)
            .InclusiveBetween(0, double.MaxValue)
            .WithMessage($"Unit Price should between 0 to {double.MaxValue}");
        
        RuleFor(temp => temp.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue)
            .WithMessage($"Quantity in Stock should between 0 to {int.MaxValue}");
    }
}
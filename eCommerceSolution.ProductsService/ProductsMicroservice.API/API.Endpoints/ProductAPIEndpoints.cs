using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ProductsMicroservice.API.API.Endpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        // GET /api/products
        app.MapGet("/api/products", async ([FromServices] IProductService productService) =>
        {
            List<ProductResponse?> products = await productService.GetProducts();

            return Results.Ok(products);
        });
        
        // GET /api/products/search/product-id/guid
        app.MapGet("/api/products/search/product-id/{productID:guid}", async (
            [FromServices] IProductService productService,
            [FromRoute] Guid productID) =>
        {
            ProductResponse? product = await productService.GetProductByCondition(temp =>
                temp.ProductID == productID);
            
            return Results.Ok(product);
        });
        
        // GET /api/products/search/product-id/searchString
        app.MapGet("/api/products/search/{searchString}", async (
            [FromServices] IProductService productService,
            [FromRoute] string searchString) =>
        {
            List<ProductResponse?> productsByProductName = await productService
                .GetProductsByCondition(temp =>
                    temp.ProductName != null &&
                    temp.ProductName.Contains(searchString));

            List<ProductResponse?> productsByCategory = await productService
                .GetProductsByCondition(temp =>
                    temp.Category != null && temp.Category.Contains(searchString));

            var products = productsByProductName.Union(productsByCategory);

            return Results.Ok(products);
        });
        
        // POST /api/products
        app.MapPost("/api/products", async (
            [FromServices] IProductService productService,
            [FromServices] IValidator<ProductAddRequest> productAddRequestValidator,
            [FromBody] ProductAddRequest productAddRequest) =>
        {
            ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(temp => temp.PropertyName)
                    .ToDictionary(grp => grp.Key,
                        grp => grp.Select(err => err.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            ProductResponse? addedProductResponse = await productService.AddProduct(productAddRequest);

            if (addedProductResponse != null)
                return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
            else
                return Results.Problem("Error in adding product");
        });
        
        // PUT /api/products
        app.MapPut("/api/products", async (
            [FromServices] IProductService productService,
            [FromServices] IValidator<ProductUpdateRequest> productUpdateRequestValidator,
            [FromBody] ProductUpdateRequest productUpdateRequest) =>
        {
            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(temp => temp.PropertyName)
                    .ToDictionary(grp => grp.Key,
                        grp => grp.Select(err => err.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            ProductResponse? updatedProductResponse = await productService.UpdateProduct(productUpdateRequest);
            
            if (updatedProductResponse != null)
                return Results.Ok(updatedProductResponse);
            else
                return Results.Problem("Error in updating product");
        });
        
        //DELETE /api/products/guid
        app.MapDelete("/api/products/{productID:guid}", async (
            [FromServices] IProductService productService,
            [FromRoute] Guid productID) =>
        {
            bool isDeleted = await productService.DeleteProduct(productID);

            if (isDeleted)
                return Results.Ok(true);
            else
                return Results.Problem("Error in deleting product");
        });

        return app;
    }
}
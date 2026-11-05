using DataEntities;

using Microsoft.EntityFrameworkCore;

using Products.Data;

namespace Products.Endpoints;

public static class ProductEndpoints {
    public static void MapProductEndpoints(this IEndpointRouteBuilder routes) {
        var group = routes.MapGroup("/api/Product");

        group.MapGet("/", async (ProductDataContext db) => {
            return await db.Product.ToListAsync();
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <param name="db">The database context.</param>
        group.MapGet("/{id:int}", async (int id, ProductDataContext db) => {
            var product = await db.Product.FindAsync(id);
            return product is null ? Results.NotFound() : Results.Ok(product);
        })
        .WithName("GetProductById")
        .WithSummary("Gets a product by its ID.")
        .WithDescription("Retrieves a product using its unique identifier.");

        group.MapPost("/", async (Product product, ProductDataContext db) => {
            product.Id = 0;
            db.Product.Add(product);
            await db.SaveChangesAsync();

            return Results.CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        })
        .WithName("CreateProduct")
        .Produces<Product>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", async (int id, Product updatedProduct, ProductDataContext db) => {
            var product = await db.Product.FindAsync(id);
            if (product is null) {
                return Results.NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.ImageUrl = updatedProduct.ImageUrl;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, ProductDataContext db) => {
            var product = await db.Product.FindAsync(id);
            if (product is null) {
                return Results.NotFound();
            }

            db.Product.Remove(product);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}

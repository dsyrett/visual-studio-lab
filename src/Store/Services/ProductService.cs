using DataEntities;
using System.Net;
using System.Net.Http.Json;

namespace Store.Services;

public class ProductService
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<Product>> GetProducts()
    {
        using var response = await httpClient.GetAsync("/api/Product");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.ListProduct)
            ?? throw new InvalidDataException("The product API returned an empty response.");
    }

    public async Task<Product?> GetProduct(int id)
    {
        using var response = await httpClient.GetAsync($"/api/Product/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.Product)
            ?? throw new InvalidDataException("The product API returned an empty response.");
    }

    public async Task<Product> CreateProduct(Product product)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "/api/Product",
            product,
            ProductSerializerContext.Default.Product);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.Product)
            ?? throw new InvalidDataException("The product API returned an empty response.");
    }

    public async Task<bool> UpdateProduct(int id, Product product)
    {
        using var response = await httpClient.PutAsJsonAsync(
            $"/api/Product/{id}",
            product,
            ProductSerializerContext.Default.Product);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        using var response = await httpClient.DeleteAsync($"/api/Product/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Model.Extensions;

public static class QueryExtensions
{
    public static async Task<T?> GetQuery<T>(this string query, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();
        throw new ArgumentNullException($"server error code {response.StatusCode}");
    }
    
    public static async Task PostQuery(this string query, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await client.PostAsync(query, null);
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) 
            throw new ArgumentNullException($"server error code {response.StatusCode}");
    }

    public static async Task<T?> PostQuery<T>(this string query, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await client.PostAsync(query, null);
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();
        throw new ArgumentNullException($"server error code {response.StatusCode}");
    }

    public static async Task PostQuery<T>(this string query, T? entity, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var content = JsonContent.Create(entity);
        var response = await client.PostAsync(query, content);
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode)
            throw new ArgumentNullException($"server error code {response.StatusCode}");
    }
    
    public static async Task DeleteQuery(this string query, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await client.DeleteAsync(query);
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode)
            throw new ArgumentNullException($"server error code {response.StatusCode}");
    }
    
    public static async Task PatchQuery<T>(this string query, T entity, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var content = JsonContent.Create(entity);
        var response = await client.PatchAsync(query, content);
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) 
            throw new ArgumentNullException($"server error code {response.StatusCode}");
    }
}
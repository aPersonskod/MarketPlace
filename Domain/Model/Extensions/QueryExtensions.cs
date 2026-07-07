using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Model.SharedExceptions;

namespace Model.Extensions;

internal enum HttpVerb
{
    GET,
    POST,
    PATCH,
    PUT,
    DELETE
}

public static class QueryExtensions
{
    public static async Task PostQuery(this string query, string? token = null)
        => await ExecuteQuery(query, HttpVerb.POST, token);
    public static async Task DeleteQuery(this string query, string? token = null)
        => await ExecuteQuery(query, HttpVerb.DELETE, token);
    
    public static async Task PostQuery<T>(this string query, T? entity, string? token = null)
        => await ExecuteQuery(query, entity, HttpVerb.POST, token);
    public static async Task PatchQuery<T>(this string query, T entity, string? token = null)
        => await ExecuteQuery(query, entity, HttpVerb.PATCH, token);
    
    public static async Task<T?> GetQuery<T>(this string query, string? token = null)
        => await ExecuteQuery<T>(query, HttpVerb.GET, token);
    public static async Task<T?> PostQuery<T>(this string query, string? token = null)
        => await ExecuteQuery<T>(query, HttpVerb.POST, token);
    public static async Task<T?> PatchQuery<T>(this string query, string? token = null)
        => await ExecuteQuery<T>(query, HttpVerb.PATCH, token);
    
    public static async Task<T?> PostQuery<T, B>(this string query, B body, string? token = null)
        => await ExecuteQuery<T, B>(query, body, HttpVerb.POST, token);
    public static async Task<T?> PatchQuery<T, B>(this string query, B body, string? token = null)
        => await ExecuteQuery<T, B>(query, body, HttpVerb.PATCH, token);
    public static async Task<T?> PutQuery<T, B>(this string query, B body, string? token = null)
        => await ExecuteQuery<T, B>(query, body, HttpVerb.PUT, token);
    
    private static async Task ExecuteQuery(this string query, HttpVerb verb = HttpVerb.POST, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = verb switch
        {
            HttpVerb.GET => await client.GetAsync(query),
            HttpVerb.POST => await client.PostAsync(query, null),
            HttpVerb.PATCH => await client.PatchAsync(query, null),
            HttpVerb.PUT => await client.PutAsync(query, null),
            HttpVerb.DELETE => await client.DeleteAsync(query),
            _ => throw new Exception($"Unsupported HTTP verb: {verb.ToString()}")
        };
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) 
            throw new ResponseException($"Api responded error", (int)response.StatusCode);
    }
    
    private static async Task ExecuteQuery<T>(this string query, T entity, HttpVerb verb, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var content = JsonContent.Create(entity);
        var response = verb switch
        {
            HttpVerb.POST => await client.PostAsync(query, content),
            HttpVerb.PATCH => await client.PatchAsync(query, content),
            HttpVerb.PUT => await client.PutAsync(query, content),
            _ => throw new Exception($"Unsupported HTTP verb: {verb.ToString()}")
        };
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) 
            throw new ResponseException($"Api responded error", (int)response.StatusCode);
    }
    
    private static async Task<T?> ExecuteQuery<T>(this string query, HttpVerb verb, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = verb switch
        {
            HttpVerb.GET => await client.GetAsync(query),
            HttpVerb.POST => await client.PostAsync(query, null),
            HttpVerb.PATCH => await client.PatchAsync(query, null),
            HttpVerb.PUT => await client.PutAsync(query, null),
            HttpVerb.DELETE => await client.DeleteAsync(query),
            _ => throw new Exception($"Unsupported HTTP verb: {verb.ToString()}")
        };
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == HttpStatusCode.NoContent) throw new NoContentException($"No content while getting {typeof(T).Name}");
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();
        throw new ResponseException($"Api responded error", (int)response.StatusCode);
    }
    
    private static async Task<T?> ExecuteQuery<T, B>(this string query, B body, HttpVerb verb = HttpVerb.POST, string? token = null)
    {
        using var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = verb switch
        {
            HttpVerb.POST => await client.PostAsJsonAsync(query, body),
            HttpVerb.PATCH => await client.PatchAsJsonAsync(query, body),
            HttpVerb.PUT => await client.PutAsJsonAsync(query, body),
            _ => throw new Exception($"Unsupported HTTP verb: {verb.ToString()}")
        };
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == HttpStatusCode.NoContent) throw new NoContentException($"No content while getting {typeof(T).Name}");
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();
        throw new ResponseException($"Api responded error", (int)response.StatusCode);
    }
}
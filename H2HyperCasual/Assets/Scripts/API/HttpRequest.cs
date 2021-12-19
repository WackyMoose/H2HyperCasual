using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class HttpRequest
{
    private HttpClient _httpClient;

    public HttpRequest()
    {
        _httpClient = new HttpClient();
    }

    public async Task<APIResponseWrapper<TEntity>> GetAsync<TEntity>(string url, string accesToken = null)
        where TEntity : class
    {
        var httpResponse = await SendRequestAsync(url, HttpMethod.Get, authorization: accesToken);
        if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Debug.LogWarning("Unauthorized...");
        }

        var response = await httpResponse.Content.ReadAsStringAsync();
        try
        {
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return APIResponseWrapper<TEntity>.Error("Unauthorized...");
            }

            var data = JsonConvert.DeserializeObject<TEntity>(response);
            return APIResponseWrapper<TEntity>.Success(data);
        }
        catch (Exception)
        {
            var errorMessageFromAPI = JsonConvert.DeserializeObject<string>(response);
            return APIResponseWrapper<TEntity>.Error(errorMessageFromAPI);
        }
    }

    public async Task<APIResponseWrapper<TEntity>> PostAsync<TEntity>(string url, object body = null, string accesToken = null)
        where TEntity : class
    {
        var bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8);
        var httpResponse = await SendRequestAsync(url, HttpMethod.Post, accesToken, httpContent: bodyContent);

        var response = await httpResponse.Content.ReadAsStringAsync();

        try
        {
            var data = JsonConvert.DeserializeObject<TEntity>(response);
            return APIResponseWrapper<TEntity>.Success(data);
        }
        catch (Exception)
        {
            var errorMessageFromAPI = JsonConvert.DeserializeObject<string>(response);
            return APIResponseWrapper<TEntity>.Error(errorMessageFromAPI);
        }
    }

    public async Task<APIResponseWrapper<TEntity>> PatchAsync<TEntity>(string url, object body = null, string accesToken = null)
    where TEntity : class
    {
        var bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8);
        var httpResponse = await SendRequestAsync(url, HttpMethod.Patch, accesToken, httpContent: bodyContent);

        var response = await httpResponse.Content.ReadAsStringAsync();

        try
        {
            var data = JsonConvert.DeserializeObject<TEntity>(response);
            return APIResponseWrapper<TEntity>.Success(data);
        }
        catch (Exception)
        {
            var errorMessageFromAPI = JsonConvert.DeserializeObject<string>(response);
            return APIResponseWrapper<TEntity>.Error(errorMessageFromAPI);
        }
    }

    public async Task<HttpResponseMessage> SendRequestAsync(string uri, HttpMethod httpMethod, string authorization = null, HttpContent httpContent = null)
    {
        if (httpContent != null)
        {
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
        var httpRequestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(uri),
            Method = httpMethod,
            Content = (httpMethod != HttpMethod.Get && httpContent != null) ? httpContent : null
        };
        httpRequestMessage.Headers.Clear();
        httpRequestMessage.Headers.Add("Accept", "application/json");

        if (authorization != null)
        {
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
        }

        return await _httpClient.SendAsync(httpRequestMessage);
    }
}
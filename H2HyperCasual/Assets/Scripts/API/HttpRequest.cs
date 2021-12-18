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

    public async Task<TEntity> GetAsync<TEntity>(string url, string accesToken = null)
    {
        var httpResponse = await SendRequestAsync(url, HttpMethod.Get, authorization: accesToken);
        if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Debug.LogWarning("Unauthorized...");
        }

        var response = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TEntity>(response);
    }

    public async Task<TEntity> PostAsync<TEntity>(string url, object body = null)
    {
        var bodyContent = new StringContent(JsonUtility.ToJson(body), Encoding.UTF8);
        var httpResponse = await SendRequestAsync(url, HttpMethod.Post, httpContent: bodyContent);

        var response = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TEntity>(response); 
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
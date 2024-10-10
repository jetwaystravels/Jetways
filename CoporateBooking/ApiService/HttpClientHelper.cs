using OnionConsumeWebAPI.Extensions;
using System.Net.Http.Headers;

namespace OnionConsumeWebAPI.ApiService
{
    public class HttpClientHelper
    {
        private static HttpClient client = new HttpClient();

        static HttpClientHelper()
        {
           // client.BaseAddress = new Uri(AppUrlConstant.ApiURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void SetAuthorizationHeader(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        //public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T obj)
        //{
        //   if (!(url.Contains("http:") || url.Contains("https:")))
        //{
        //    url = AppUrlConstant.ApiURL + url;
        //}
        //    return await client.PostAsJsonAsync(url, obj);
        //}
    }
}

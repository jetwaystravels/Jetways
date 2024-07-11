using Newtonsoft.Json;
using NuGet.Common;
using OnionConsumeWebAPI.Extensions;
using OnionConsumeWebAPI.Models;
using System.Net.Http.Headers;


namespace OnionConsumeWebAPI.ApiService
{
    public static class AppHttpContext
    {
        static IServiceProvider services = null;
       

        /// <summary>
        /// Provides static access to the framework's services provider
        /// </summary>
        public static IServiceProvider Services
        {
            get { return services; }
            set
            {
                if (services != null)
                {
                    //throw new Exception("Can't set once a value has already been set.");
                }
                services = value;
            }
        }

        /// <summary>
        /// Provides static access to the current HttpContext
        /// </summary>
        public static HttpContext Current
        {
            get
            {
                IHttpContextAccessor httpContextAccessor = services?.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                return httpContextAccessor?.HttpContext;
            }
        }

    }
    public class ApiRequests 
    {
       // private readonly string Token = "abc";//AppHttpContext.Current.Session.GetString("AirasiaTokan");
       
        // string tokenview = HttpContext.("AirasiaTokan");
       //  string tokenview = HttpContext.Session.GetString("AirasiaTokan");
         private readonly string Token = AppHttpContext.Current?.Session.GetString("AirasiaTokan");
       
        public async Task<ApiResponseModel> PostAsJsonAsync<T>(string Uri, T RequestModel)
        {
            ApiResponseModel responseModel = new ApiResponseModel();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppUrlConstant.URLAirasia);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string jsonString = JsonConvert.SerializeObject(RequestModel);//for Postman purpose


                    if (!(Uri.Contains("http:") || Uri.Contains("https:")))
                    {
                        Uri = AppUrlConstant.AirasiaTokan + Uri;
                    }
                    HttpResponseMessage response = await client.PostAsJsonAsync(Uri, RequestModel);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<ApiResponseModel>(result.ToString());
                    }
                    else
                    {
                        responseModel.Message = "Internal server error";
                    }
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }

        internal Task OnGetBySearchParamAsync(object getLevelOfUser, object queryParam)
        {
            throw new NotImplementedException();
        }

        //public async Task<ApiResponseModel> OnGetAllAsync(string Uri)
        //{
        //    ApiResponseModel responseModel = new ApiResponseModel();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(AppUrlConstant.ApiURL);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            if (!(Uri.Contains("http:") || Uri.Contains("https:")))
        //            {
        //                Uri = AppUrlConstant.ApiURL + Uri;
        //            }
        //            HttpResponseMessage response = await client.GetAsync(Uri);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = response.Content.ReadAsStringAsync().Result;
        //                responseModel = JsonConvert.DeserializeObject<ApiResponseModel>(result.ToString());
        //            }
        //            else
        //            {
        //                responseModel.Message = "Internal server error";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.Message = ex.Message;
        //    }
        //    return responseModel;
        //}
        //public async Task<ApiResponseModel> OnGetByIdAsync(string Uri, string id)
        //{
        //    ApiResponseModel responseModel = new ApiResponseModel();
        //    try
        //    {

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(AppUrlConstant.ApiURL);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            HttpResponseMessage response = await client.GetAsync(Uri + "?id=" + id);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = response.Content.ReadAsStringAsync().Result;
        //                responseModel = JsonConvert.DeserializeObject<ApiResponseModel>(result.ToString());

        //            }
        //            else
        //            {
        //                responseModel.Message = "Internal server error";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.Message = ex.Message;
        //    }

        //    return responseModel;
        //}

        //public async Task<ApiResponseModel> OnGetBySearchParamAsync(string Uri, string SearchParam)
        //{
        //    ApiResponseModel responseModel = new ApiResponseModel();
        //    try
        //    {

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(AppUrlConstant.ApiURL);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            if (!(Uri.Contains("http:") || Uri.Contains("https:")))
        //            {
        //                Uri = AppUrlConstant.ApiURL + Uri;
        //            }
        //            HttpResponseMessage response = await client.GetAsync(Uri + SearchParam);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = response.Content.ReadAsStringAsync().Result;
        //                responseModel = JsonConvert.DeserializeObject<ApiResponseModel>(result.ToString());

        //            }
        //            else
        //            {
        //                responseModel.Message = "Internal server error";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.Message = ex.Message;
        //    }

        //    return responseModel;
        //}

        //public async Task<ApiResponseModel> OnGetByIdsAsync(string Uri, int id1, int id2)
        //{
        //    ApiResponseModel responseModel = new ApiResponseModel();
        //    try
        //    {

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(AppUrlConstant.ApiURL);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            HttpResponseMessage response = await client.GetAsync(Uri + "?id1=" + id1 + "?id2=" + id2);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = response.Content.ReadAsStringAsync().Result;
        //                responseModel = JsonConvert.DeserializeObject<ApiResponseModel>(result.ToString());

        //            }
        //            else
        //            {
        //                responseModel.Message = "Internal server error";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.Message = ex.Message;
        //    }

        //    return responseModel;
        //}

        //public async Task<ApiResponseModel> OnGetByIntIdAsync(string Uri, int id)
        //{
        //    ApiResponseModel responseModel = new ApiResponseModel();
        //    try
        //    {

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(AppUrlConstant.ApiURL);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            HttpResponseMessage response = await client.GetAsync(Uri + "?id=" + id);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = response.Content.ReadAsStringAsync().Result;
        //                responseModel = JsonConvert.DeserializeObject<ApiResponseModel>(result.ToString());

        //            }
        //            else
        //            {
        //                responseModel.Message = "Internal server error";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.Message = ex.Message;
        //    }

        //    return responseModel;
        //}


    }
}

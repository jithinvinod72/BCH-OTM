using BCMCH.OTM.API.Shared.General;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.External
{
    public class OTMDataClient : IOTMDataClient 
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public OTMDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "tmBQGvA27MhFYAMGZiV5DD2gV3laMQZP");
        }
        public async Task<T> GetAsync<T>(string uri)
        {
            using (var response = await _httpClient.GetAsync(uri))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    throw new ExternalApiException($"OTM api failed with statuscode : {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(content))
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }
                throw new ExternalApiException($"OTM api failed with statuscode : {response.StatusCode}");

            }
        }

        public async Task<T> PostAsync<T>(string relativeUrl, object payload)
        {
            using (var response = await _httpClient.PostAsync(relativeUrl, new JsonContent(payload)))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    throw new ExternalApiException($"OTM api call failed with statuscode : {response.StatusCode}");
                }

                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(content))
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }

                throw new ExternalApiException($"OTM api call failed with statuscode : {response.StatusCode}");
            }
        }
        public async Task<Authentication> AuthenticateUser(string token)
        {
            Authentication auth = new Authentication();
            BodyExternalAPI key = new BodyExternalAPI();
            key.key = token;
            auth.Authenticated = false;
            string baseUrl= _configuration.GetSection("baseURL").Value + @"user/validate";

            using (var response = await _httpClient.PostAsync(baseUrl, new JsonContent(key)))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return auth;
                }

                string content = await response.Content.ReadAsStringAsync();
                

                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(content))
                {
                    var result= JsonConvert.DeserializeObject<ResponseModelExternalAPI>(content);
                    if (result.success) {
                        auth.Authenticated = true;
                        return auth;
                    }
                }

                return auth;
            }
        }
    }
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }
   }

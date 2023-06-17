using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LabModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    public interface IChatGptService
    {
        Task<string> GetAdviceAsync(AnalysisResultResult analysisResult);
    }

    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public ChatGptService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:ChatUrl");
        }

        public async Task<string> GetAdviceAsync(AnalysisResultResult analysisResult)
        {
            string prompt = "Can you analyze the " + analysisResult.AnalysisName + 
                " analysis values: " + analysisResult.AnalysisValues + ". And give the recommendations.";
            var json = JsonConvert.SerializeObject(prompt);
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl + "api/ChatGpt", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<string[]>(responseContent);
                string answer = "";
                foreach ( var item in responseObject )
                {
                    answer += item;
                }
                return answer;
            }
            return "Error";
        }
    }
}

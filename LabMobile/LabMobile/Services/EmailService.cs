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
    public interface IEmailService
    {
        public Task SendEmail(AnalysisResultResultDto analysisResult);
    }

    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public EmailService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl");
        }

        public async Task SendEmail(AnalysisResultResultDto analysisResult)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(analysisResult);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl + "api/AnalysisResult/sendemail", content);

            // Handle the response here
            if (response.IsSuccessStatusCode)
            {
                // Request successful
            }
            else
            {
                // Request failed
            }
        }
    }
}

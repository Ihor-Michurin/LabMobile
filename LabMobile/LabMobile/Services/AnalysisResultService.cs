using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LabModels;
using Microsoft.Extensions.Configuration;

namespace LabMobile.Services
{
    public interface IAnalysisResultService
    {
        Task<List<AnalysisResultResult>> GetAnalysisResultResultAsync();
    }

    public class AnalysisResultService : IAnalysisResultService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string BaseUrl;

        public AnalysisResultService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/AnalysisResult";
        }

        public async Task<List<AnalysisResultResult>> GetAnalysisResultResultAsync()
        {
            var id = await SecureStorage.GetAsync("RoleId");
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/patient/{id}");
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<AnalysisResultResult>>(content, _jsonSerializerOptions);
            }
            else
            {
                return new List<AnalysisResultResult>();
            }
        }
    }
}

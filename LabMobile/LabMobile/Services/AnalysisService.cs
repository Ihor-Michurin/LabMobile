using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LabModels;
using Microsoft.Extensions.Configuration;

namespace LabMobile.Services
{
    public interface IAnalysisService
    {
        Task<List<Analysis>> GetAnalysesAsync();
        Task<Analysis> GetAnalysisAsync(Guid id);
        Task<Analysis> CreateAnalysisAsync(Analysis analysis);
        Task UpdateAnalysisAsync(Guid id, Analysis analysis);
        Task DeleteAnalysisAsync(Guid id);
    }


    public class AnalysisService : IAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public AnalysisService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/Analysis";
        }

        public async Task<List<Analysis>> GetAnalysesAsync()
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Analysis>>();
        }

        public async Task<Analysis> GetAnalysisAsync(Guid id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Analysis>();
        }

        public async Task<Analysis> CreateAnalysisAsync(Analysis analysis)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, analysis);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Analysis>();
        }

        public async Task UpdateAnalysisAsync(Guid id, Analysis analysis)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = JsonSerializer.Serialize(analysis);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{BaseUrl}/{id}", content);
        }

        public async Task DeleteAnalysisAsync(Guid id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}

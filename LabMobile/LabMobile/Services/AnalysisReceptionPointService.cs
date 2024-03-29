﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LabModels;
using Microsoft.Extensions.Configuration;

namespace LabMobile.Services
{
    public interface IAnalysisReceptionPointService
    {
        Task<List<AnalysisReceptionPoint>> GetAllAnalysisReceptionPointsAsync();
        Task<AnalysisReceptionPoint> GetAnalysisReceptionPointAsync(Guid? id);
        Task AddAnalysisReceptionPointAsync(AnalysisReceptionPoint analysisReceptionPoint);
        Task UpdateAnalysisReceptionPointAsync(Guid? id, AnalysisReceptionPoint analysisReceptionPoint);
        Task DeleteAnalysisReceptionPointAsync(Guid? id);
    }

    public class AnalysisReceptionPointService : IAnalysisReceptionPointService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string BaseUrl;

        public AnalysisReceptionPointService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/AnalysisReceptionPoints";
        }

        public async Task<List<AnalysisReceptionPoint>> GetAllAnalysisReceptionPointsAsync()
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AnalysisReceptionPoint>>(content, _jsonSerializerOptions);
        }

        public async Task<AnalysisReceptionPoint> GetAnalysisReceptionPointAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AnalysisReceptionPoint>(content, _jsonSerializerOptions);
        }

        public async Task AddAnalysisReceptionPointAsync(AnalysisReceptionPoint analysisReceptionPoint)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = JsonSerializer.Serialize(analysisReceptionPoint);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAnalysisReceptionPointAsync(Guid? id, AnalysisReceptionPoint analysisReceptionPoint)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = JsonSerializer.Serialize(analysisReceptionPoint);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAnalysisReceptionPointAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}

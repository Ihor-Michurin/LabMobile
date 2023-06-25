using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LabModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    // Interface for LaboratoryAssistant service
    public interface ILaboratoryAssistantService
    {
        Task<LaboratoryAssistant> GetAsync(Guid? id);
        Task<List<LaboratoryAssistant>> GetAllAsync();
        Task CreateAsync(LaboratoryAssistant assistant);
        Task UpdateAsync(Guid? id, LaboratoryAssistant assistant);
        Task DeleteAsync(Guid? id);
    }

    // Implementation of LaboratoryAssistant service using HttpClient
    public class LaboratoryAssistantService : ILaboratoryAssistantService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public LaboratoryAssistantService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/LaboratoryAssistants";
        }

        public async Task<LaboratoryAssistant> GetAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LaboratoryAssistant>(content);
        }

        public async Task<List<LaboratoryAssistant>> GetAllAsync()
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<LaboratoryAssistant>>(content);
        }

        public async Task CreateAsync(LaboratoryAssistant assistant)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            assistant.DateOfBirth = DateTime.SpecifyKind(assistant.DateOfBirth, DateTimeKind.Utc);

            var json = JsonConvert.SerializeObject(assistant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
        }



        public async Task UpdateAsync(Guid? id, LaboratoryAssistant assistant)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            assistant.DateOfBirth = DateTime.SpecifyKind(assistant.DateOfBirth, DateTimeKind.Utc);
            var response = await _httpClient.PutAsync($"{BaseUrl}/{id}", new StringContent(JsonConvert.SerializeObject(assistant), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}

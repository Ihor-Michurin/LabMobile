using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LabMobile.Models;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    // Interface for LaboratoryAssistant service
    public interface ILaboratoryAssistantService
    {
        Task<LaboratoryAssistant> GetAsync(Guid id);
        Task<List<LaboratoryAssistant>> GetAllAsync();
        Task CreateAsync(LaboratoryAssistant assistant);
        Task UpdateAsync(Guid id, LaboratoryAssistant assistant);
        Task DeleteAsync(Guid id);
    }

    // Implementation of LaboratoryAssistant service using HttpClient
    public class LaboratoryAssistantService : ILaboratoryAssistantService
    {
        private readonly HttpClient _httpClient;

        public LaboratoryAssistantService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<LaboratoryAssistant> GetAsync(Guid id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"https://labapi123.azurewebsites.net/api/LaboratoryAssistants/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LaboratoryAssistant>(content);
        }

        public async Task<List<LaboratoryAssistant>> GetAllAsync()
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync("https://labapi123.azurewebsites.net/api/LaboratoryAssistants");
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

            var response = await _httpClient.PostAsync("https://labapi123.azurewebsites.net/api/LaboratoryAssistants", content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
        }



        public async Task UpdateAsync(Guid id, LaboratoryAssistant assistant)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            assistant.DateOfBirth = DateTime.SpecifyKind(assistant.DateOfBirth, DateTimeKind.Utc);
            var response = await _httpClient.PutAsync($"https://labapi123.azurewebsites.net/api/LaboratoryAssistants/{id}", new StringContent(JsonConvert.SerializeObject(assistant), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.DeleteAsync($"https://labapi123.azurewebsites.net/api/LaboratoryAssistants/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<Guid> CreateAsync(LaboratoryAssistant assistant);
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
            var response = await _httpClient.GetAsync($"https://labapi123.azurewebsites.net/api/LaboratoryAssistants/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LaboratoryAssistant>(content);
        }

        public async Task<List<LaboratoryAssistant>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("https://labapi123.azurewebsites.net/api/LaboratoryAssistants");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<LaboratoryAssistant>>(content);
        }

        public async Task<Guid> CreateAsync(LaboratoryAssistant assistant)
        {
            var response = await _httpClient.PostAsync("https://labapi123.azurewebsites.net/api/LaboratoryAssistants", new StringContent(JsonConvert.SerializeObject(assistant), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Guid>(content);
        }

        public async Task UpdateAsync(Guid id, LaboratoryAssistant assistant)
        {
            var response = await _httpClient.PutAsync($"https://labapi123.azurewebsites.net/api/LaboratoryAssistants/{id}", new StringContent(JsonConvert.SerializeObject(assistant), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://labapi123.azurewebsites.net/api/LaboratoryAssistants/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}

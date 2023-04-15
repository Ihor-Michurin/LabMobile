using System.Text;
using LabMobile.Models;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    // Interface for Registrar service
    public interface IRegistrarService
    {
        Task<Registrar> GetAsync(Guid id);
        Task<List<Registrar>> GetAllAsync();
        Task CreateAsync(Registrar assistant);
        Task UpdateAsync(Guid id, Registrar assistant);
        Task DeleteAsync(Guid id);
    }

    // Implementation of Registrar service using HttpClient
    public class RegistrarService : IRegistrarService
    {
        private readonly HttpClient _httpClient;

        public RegistrarService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Registrar> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"https://labapi123.azurewebsites.net/api/Registrars/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Registrar>(content);
        }

        public async Task<List<Registrar>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("https://labapi123.azurewebsites.net/api/Registrars");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Registrar>>(content);
        }

        public async Task CreateAsync(Registrar assistant)
        {
            assistant.DateOfBirth = DateTime.SpecifyKind(assistant.DateOfBirth, DateTimeKind.Utc);

            var json = JsonConvert.SerializeObject(assistant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://labapi123.azurewebsites.net/api/Registrars", content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
        }



        public async Task UpdateAsync(Guid id, Registrar assistant)
        {
            assistant.DateOfBirth = DateTime.SpecifyKind(assistant.DateOfBirth, DateTimeKind.Utc);

            var response = await _httpClient.PutAsync($"https://labapi123.azurewebsites.net/api/Registrars/{id}", new StringContent(JsonConvert.SerializeObject(assistant), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://labapi123.azurewebsites.net/api/Registrars/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

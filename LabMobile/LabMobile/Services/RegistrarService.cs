using System.Net.Http.Headers;
using System.Text;
using LabModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    // Interface for Registrar service
    public interface IRegistrarService
    {
        Task<Registrar> GetAsync(Guid? id);
        Task<List<Registrar>> GetAllAsync();
        Task CreateAsync(Registrar assistant);
        Task UpdateAsync(Guid? id, Registrar assistant);
        Task DeleteAsync(Guid? id);
    }

    // Implementation of Registrar service using HttpClient
    public class RegistrarService : IRegistrarService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public RegistrarService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/Registrars";
        }

        public async Task<Registrar> GetAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Registrar>(content);
        }

        public async Task<List<Registrar>> GetAllAsync()
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Registrar>>(content);
        }

        public async Task CreateAsync(Registrar assistant)
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



        public async Task UpdateAsync(Guid? id, Registrar assistant)
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

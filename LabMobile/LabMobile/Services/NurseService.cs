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
    // Interface for Nurse service
    public interface INurseService
    {
        Task<Nurse> GetAsync(Guid? id);
        Task<List<Nurse>> GetAllAsync();
        Task CreateAsync(Nurse assistant);
        Task UpdateAsync(Guid? id, Nurse assistant);
        Task DeleteAsync(Guid? id);
    }

    // Implementation of Nurse service using HttpClient
    public class NurseService : INurseService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public NurseService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/Nurses";
        }

        public async Task<Nurse> GetAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Nurse>(content);
        }

        public async Task<List<Nurse>> GetAllAsync()
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Nurse>>(content);
        }

        public async Task CreateAsync(Nurse assistant)
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



        public async Task UpdateAsync(Guid? id, Nurse assistant)
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

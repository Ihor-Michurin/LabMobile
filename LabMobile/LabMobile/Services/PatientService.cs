using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabMobile.Models;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    // Interface for Patient service
    public interface IPatientService
    {
        Task<Patient> GetAsync(Guid id);
        Task<List<Patient>> GetAllAsync();
        Task CreateAsync(Patient assistant);
        Task UpdateAsync(Guid id, Patient assistant);
        Task DeleteAsync(Guid id);
    }

    // Implementation of Patient service using HttpClient
    public class PatientService : IPatientService
    {
        private readonly HttpClient _httpClient;

        public PatientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Patient> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"https://labapi123.azurewebsites.net/api/Patients/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Patient>(content);
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("https://labapi123.azurewebsites.net/api/Patients");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Patient>>(content);
        }

        public async Task CreateAsync(Patient assistant)
        {
            var json = JsonConvert.SerializeObject(assistant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://labapi123.azurewebsites.net/api/Patients", content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
        }



        public async Task UpdateAsync(Guid id, Patient assistant)
        {
            var response = await _httpClient.PutAsync($"https://labapi123.azurewebsites.net/api/Patients/{id}", new StringContent(JsonConvert.SerializeObject(assistant), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://labapi123.azurewebsites.net/api/Patients/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

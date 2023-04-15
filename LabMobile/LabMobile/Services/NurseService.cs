﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabMobile.Models;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    // Interface for Nurse service
    public interface INurseService
    {
        Task<Nurse> GetAsync(Guid id);
        Task<List<Nurse>> GetAllAsync();
        Task CreateAsync(Nurse assistant);
        Task UpdateAsync(Guid id, Nurse assistant);
        Task DeleteAsync(Guid id);
    }

    // Implementation of Nurse service using HttpClient
    public class NurseService : INurseService
    {
        private readonly HttpClient _httpClient;

        public NurseService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Nurse> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"https://labapi123.azurewebsites.net/api/Nurses/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Nurse>(content);
        }

        public async Task<List<Nurse>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("https://labapi123.azurewebsites.net/api/Nurses");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Nurse>>(content);
        }

        public async Task CreateAsync(Nurse assistant)
        {
            assistant.DateOfBirth = DateTime.SpecifyKind(assistant.DateOfBirth, DateTimeKind.Utc);

            var json = JsonConvert.SerializeObject(assistant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://labapi123.azurewebsites.net/api/Nurses", content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
        }



        public async Task UpdateAsync(Guid id, Nurse assistant)
        {
            assistant.DateOfBirth = DateTime.SpecifyKind(assistant.DateOfBirth, DateTimeKind.Utc);

            var response = await _httpClient.PutAsync($"https://labapi123.azurewebsites.net/api/Nurses/{id}", new StringContent(JsonConvert.SerializeObject(assistant), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://labapi123.azurewebsites.net/api/Nurses/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

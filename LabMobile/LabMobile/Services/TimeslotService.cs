using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LabModels;
using Microsoft.Extensions.Configuration;

namespace LabMobile.Services
{
    public interface ITimeslotService
    {
        Task<List<Timeslot>> GetTimeslotsAsync(Guid id);

        Task BookTimeslotAsync(DateTime timeslotTime);

        Task<List<TimeslotResult>> GetPatientTimeslotsAsync();

        Task CanselBookingPatientTimeslotsAsync(TimeslotResult timeslotResult);
    }

    public class TimeslotService : ITimeslotService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string BaseUrl;

        public TimeslotService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/TimeSlot";
        }

        public async Task<List<Timeslot>> GetTimeslotsAsync(Guid id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/analisisreceptionpoint/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Timeslot>>(content, _jsonSerializerOptions);
        }

        public async Task<List<TimeslotResult>> GetPatientTimeslotsAsync()
        {
            var id = await SecureStorage.GetAsync("RoleId");
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{BaseUrl}/patient/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TimeslotResult>>(content, _jsonSerializerOptions);
        }

        public async Task CanselBookingPatientTimeslotsAsync(TimeslotResult timeslotResult)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            timeslotResult.Time = DateTime.SpecifyKind(timeslotResult.Time, DateTimeKind.Utc);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(timeslotResult);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/cancelbooking", content);

            response.EnsureSuccessStatusCode();
        }


        public async Task BookTimeslotAsync(DateTime timeslotTime)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            var analysisReceptionPointId = await SecureStorage.GetAsync("receptionPointId");
            var patientId = await SecureStorage.GetAsync("RoleId");
            var analisisDuration = await SecureStorage.GetAsync("duration");
            var analisisId = await SecureStorage.GetAsync("analysisId");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var queryParams = new Dictionary<string, string>
    {
        { "analysisReceptionPointId", analysisReceptionPointId },
        { "timeslotTime", timeslotTime.ToString("yyyy-MM-ddTHH:mm:ss+00:00") },
        { "analisisId", analisisId },
        { "analisisDuration", analisisDuration },
        { "patientId", patientId }
    };

            var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            var url = $"{BaseUrl}/BookTimeslots?{queryString}";

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

    }
}

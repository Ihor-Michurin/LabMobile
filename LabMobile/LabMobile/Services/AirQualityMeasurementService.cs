using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using LabModels;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace LabMobile.Services
{
    public interface IAirQualityMeasurementService
    {
        public Task<List<AirQualityMeasurement>> GetAirQualityMeasurementsAsync();

        public Task<AirQualityMeasurement> GetAirQualityMeasurementAsync(Guid? id);

        public Task<bool> CreateAirQualityMeasurementAsync(AirQualityMeasurement measurement);

        public Task<bool> UpdateAirQualityMeasurementAsync(AirQualityMeasurement measurement);

        public Task<bool> DeleteAirQualityMeasurementAsync(Guid? id);
    }

    public class AirQualityMeasurementService: IAirQualityMeasurementService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public AirQualityMeasurementService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/AirQualityMeasurements";
        }

        public async Task<List<AirQualityMeasurement>> GetAirQualityMeasurementsAsync()
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirQualityMeasurement>>(content);
            }
            return null;
        }

        public async Task<AirQualityMeasurement> GetAirQualityMeasurementAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AirQualityMeasurement>(content);
            }
            return null;
        }

        public async Task<bool> CreateAirQualityMeasurementAsync(AirQualityMeasurement measurement)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = JsonConvert.SerializeObject(measurement);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAirQualityMeasurementAsync(AirQualityMeasurement measurement)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var url = $"{BaseUrl}/{measurement.Id}";
            var json = JsonConvert.SerializeObject(measurement);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAirQualityMeasurementAsync(Guid? id)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
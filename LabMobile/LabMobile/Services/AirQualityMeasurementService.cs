using System.Text;
using LabMobile.Models;
using Newtonsoft.Json;

namespace LabMobile.Services
{
    public interface IAirQualityMeasurementService
    {
        public Task<List<AirQualityMeasurement>> GetAirQualityMeasurementsAsync();

        public Task<AirQualityMeasurement> GetAirQualityMeasurementAsync(Guid id);

        public Task<bool> CreateAirQualityMeasurementAsync(AirQualityMeasurement measurement);

        public Task<bool> UpdateAirQualityMeasurementAsync(AirQualityMeasurement measurement);

        public Task<bool> DeleteAirQualityMeasurementAsync(Guid id);
    }

    public class AirQualityMeasurementService: IAirQualityMeasurementService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://labapi123.azurewebsites.net/api/AirQualityMeasurements";

        public AirQualityMeasurementService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<AirQualityMeasurement>> GetAirQualityMeasurementsAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirQualityMeasurement>>(content);
            }
            return null;
        }

        public async Task<AirQualityMeasurement> GetAirQualityMeasurementAsync(Guid id)
        {
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
            var json = JsonConvert.SerializeObject(measurement);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAirQualityMeasurementAsync(AirQualityMeasurement measurement)
        {
            var url = $"{BaseUrl}/{measurement.Id}";
            var json = JsonConvert.SerializeObject(measurement);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAirQualityMeasurementAsync(Guid id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
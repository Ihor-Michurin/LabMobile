using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LabMobile.Services
{

    public interface IAuthService
    {
        Task<bool> LoginAsync(UserDto user);

        Task<bool> RegisterAsync(UserPatientDto user);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public AuthService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            BaseUrl = configuration.GetValue<string>("AppSettings:AuthUrl");
        }

        public async Task<bool> LoginAsync(UserDto user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl + "api/Auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<UserResult>(responseContent);
                await SecureStorage.SetAsync("Role", responseObject.Role);
                await SecureStorage.SetAsync("RoleId", responseObject.RoleId.ToString());
                await SecureStorage.SetAsync("AccessToken", responseObject.Token);
                RoleChanged?.Invoke(null, EventArgs.Empty);
                return true;

            }
            return false;
        }

        public async Task<bool> RegisterAsync(UserPatientDto user)
        {
            user.DateOfBirth = DateTime.SpecifyKind(user.DateOfBirth, DateTimeKind.Utc);
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl + "api/Auth/registerpatient", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<UserResult>(responseContent);
                await SecureStorage.SetAsync("Role", responseObject.Role);
                await SecureStorage.SetAsync("RoleId", responseObject.RoleId.ToString() ?? "");
                await SecureStorage.SetAsync("AccessToken", responseObject.Token);
                RoleChanged?.Invoke(null, EventArgs.Empty);
                return true;

            }
            return false;
        }

        public static event EventHandler RoleChanged;
    }
}

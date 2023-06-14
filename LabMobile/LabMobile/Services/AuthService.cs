using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabMobile.Models;
using Newtonsoft.Json;

namespace LabMobile.Services
{

    public interface IAuthService
    {
        Task<bool> LoginAsync(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://labapi123.azurewebsites.net/api/Auth";

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> LoginAsync(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl + "/token", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string accessToken = responseObject.token;
                await SecureStorage.SetAsync("AccessToken", accessToken);
                return true;

            }
            return false;
        }
    }
}

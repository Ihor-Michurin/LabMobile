﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LabMobile.Services
{
    public interface IDatabaseService
    {
        Task<string> ExportData();
        Task<bool> ImportData(string database);
        Task<bool> DeleteData();
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly string BaseUrl;

        public DatabaseService(IConfiguration configuration) {
            BaseUrl = configuration.GetValue<string>("AppSettings:MainApiUrl") + "/api/DatabaseData";
        }

        async Task<bool> IDatabaseService.DeleteData()
        {
            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var accessToken = await SecureStorage.GetAsync("AccessToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    string url = BaseUrl;
                    var apiResponse = await client.DeleteAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }

        async Task<string> IDatabaseService.ExportData()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = BaseUrl;
                    var accessToken = await SecureStorage.GetAsync("AccessToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return null;
        }

        async Task<bool> IDatabaseService.ImportData(string database)
        {
            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var accessToken = await SecureStorage.GetAsync("AccessToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    string url = BaseUrl;

                    var apiResponse = await client.PostAsync(url, new StringContent(database, Encoding.UTF8, "application/json"));
                    result = true;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                result = false;
            }
            return result;
        }
    }
}

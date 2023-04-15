using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string _baseUrl = "https://labapi123.azurewebsites.net";

        async Task<bool> IDatabaseService.DeleteData()
        {
            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/DatabaseData";
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
                    string url = $"{_baseUrl}/api/DatabaseData";

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
                    string url = $"{_baseUrl}/api/DatabaseData";

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

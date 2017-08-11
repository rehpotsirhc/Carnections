using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class HttpGet
    {

        public static async Task<string> Get(string endpoint, string headerKey, string headerValue)
        {
            return await Get(endpoint, new Dictionary<string, string>  {
                  { headerKey, headerValue}
                });
        }
        public static async Task<string> Get(string endpoint, IDictionary<string, string> headers = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 100);
                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, string> header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                    HttpResponseMessage response = client.GetAsync(endpoint).Result;
                    if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                    else return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

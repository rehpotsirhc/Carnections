using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace Common.Utils
{
    public static class HttpUtils
    {

        public static async Task<string> GetAsString(string endpoint, string headerKey, string headerValue)
        {

            var response = await GetAsResponseMessage(endpoint, headerKey, headerValue);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else return null;

        }

        public static async Task<string> GetAsString(string endpoint, IDictionary<string, string> headers = null)
        {

            var response = await GetAsResponseMessage(endpoint, headers);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else return null;

        }

        public static async Task<HttpResponseMessage> GetAsResponseMessage(string endpoint, string headerKey, string headerValue)
        {
            return await GetAsResponseMessage(endpoint, new Dictionary<string, string>  {
                  { headerKey, headerValue}
                });
        }
        public static async Task<HttpResponseMessage> GetAsResponseMessage(string endpoint, IDictionary<string, string> headers = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 100);
                    AddHeaders(client, headers);
                    return await client.GetAsync(endpoint);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        // string body = Username:Bartd
        // Password:bj1234567
        // CSRFToken:d873a3798996b524ed173da1a2b2eebb6d19d1c2912672280eb6964d008d684c


        private static async Task<HttpResponseMessage> PostFormUrlEncodedContentAsResponseMessage(string endpoint, IDictionary<string, string> formData = null, IDictionary<string, string> headers = null)
        {
            try
            {

                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;
               
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 100);
                    AddHeaders(client, headers);

                    HttpResponseMessage response = await client.PostAsync(endpoint, new FormUrlEncodedContent(formData));


                    IEnumerable<Cookie> responseCookies = cookies.GetCookies(new Uri(endpoint)).Cast<Cookie>();

                    if (response.IsSuccessStatusCode) return response;
                    else return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> LoginToCD(string endpoint, string username, string password, string csrfToken)
        {
            var formData = new Dictionary<string, string>  {
                  { "Username", username},
                  { "Password", password },
                  { "r", "" },
                  { "CSRFToken", csrfToken}
            };

            var headers = new Dictionary<string, string>
            {
                { "Cookie", "Cookie: CSRF_TOKEN=d873a3798996b524ed173da1a2b2eebb6d19d1c2912672280eb6964d008d684c; visitedDashboard=1; defaultView=list; CSRF_TOKEN=d873a3798996b524ed173da1a2b2eebb6d19d1c2912672280eb6964d008d684c; ak_bmsc=113FAF0864F36D317EE3751545F077E83F977712346900008574625A70817D34~plrDOYSJWVHYH3FrVYFIGROaNUPGZPdN1hHsG4UwiwMhL7mldP2QoqZAP9NAKplvavTMTLO0dmAlGG79xLgaLmn7iuHf+YPSC9Y8jUabQzLV9eVw7HBc3Nsr0JzaWavgY19EjZvs7BE4yF6rJSQI3tMwgfumkmOFd7gnJEnOR+uNeLBiO/JZqmTHYNvXLORcQ6CfDH2qJ4K3sQYb6fe/VMZSXpf8R3Z2YYgX+1GtwqVGhzqp/7xvDXQMPPwswC+eHd; visitedDashboard=1; defaultView=list; PHPSESSID=6b4caf775b860420cf7135260205346b; test-persistent=1; test-session=1; bm_sv=89E695A7B40822A0B67B36CDD088F87A~dOCrFGvIebSLNrJWfOa3e/Im+q8DLfXDQ7iS7Vve102YLFPyycId9UGw5GSWBICqeoA3mSyfj+jNkUxTcqFM9+d0PeMsiIQuve8ApwCQjtVp6Hpwendz8nHiWEKBIl397q3i/A+VgTuTXGai/HzAWCv/Jj5B4dsX1KBPJ+hjfHQ=; test-persistent=1; test-session=1; test-session=1; PHPSESSID=5bb95c9f19fb1e4d92d4d1e4720578f5"}

            };
            

            var httpResponse = await PostFormUrlEncodedContentAsResponseMessage(endpoint, formData, headers);
            var cookieHeaderResponse = httpResponse.Headers.GetValues("Set-Cookie");

            var httpResponse2 = await GetAsResponseMessage("https://www.centraldispatch.com/protected/", "Cookie", String.Join("; ", cookieHeaderResponse));

            var cookieHeaderResponse2 = httpResponse2.Headers.GetValues("Set-Cookie");


            //example: PHPSESSID=5bf772d70efebae3bf8229770123e6bb; path=/; domain=.centraldispatch.com; secure; HttpOnly
            var phpSessIdAndJunk = cookieHeaderResponse2.First();

            var phpSessId = phpSessIdAndJunk.Split(";")[0].Split("=")[1].Trim();
            return phpSessId;

        }

        private static void AddHeaders(HttpClient client, IDictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ZlatkoBandrov.Common;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.BusinessLogic.Managers
{
    public class ArrivalTrackerManager
    {
        public static TokenData CurrentToken { get; set; }

        public void InitializeCommunication()
        {
            string callbackUrl = Utils.GetArrivalTrackerUrl();
            string trackingDate = DateTime.Now.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string subscribeBaseUrl = Utils.GetConfigSetting("SubscribeBaseUrl");

            string subscribeEntryPointUrl = Utils.GetConfigSetting("SubscribeEntryPointUrl");
            subscribeEntryPointUrl = string.Format(subscribeEntryPointUrl, trackingDate, callbackUrl);

            // Execute the subscribtion
            MakeSubscribtion(subscribeBaseUrl, subscribeEntryPointUrl);
        }

        public bool ValidateToken(string token)
        {
            bool tokenIsValid = true;
            if (CurrentToken == null || string.IsNullOrEmpty(CurrentToken.Token) || string.IsNullOrEmpty(token))
            {
                tokenIsValid = false;
            }
            else if (string.Compare(CurrentToken.Token, token, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                tokenIsValid = false;
            }
            else if (CurrentToken.Expires < DateTime.Now)
            {
                tokenIsValid = false;
            }
            return tokenIsValid;
        }

        public void TrackArrivals(EmployeeArrivalData[] data)
        {
            var employeeManager = new EmployeeManager();
            var employees = employeeManager.GetAllEmployees();
        }


        private void MakeSubscribtion(string subscribeBaseUrl, string subscribeEntryPointUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(subscribeBaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Accept-Client", "Fourth-Monitor");

                HttpResponseMessage response = httpClient.GetAsync(subscribeEntryPointUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    CurrentToken = Utils.FromTokenResult(response.Content.ReadAsAsync<TokenResult>().Result);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    CurrentToken = null;
                }
            }
        }
    }
}

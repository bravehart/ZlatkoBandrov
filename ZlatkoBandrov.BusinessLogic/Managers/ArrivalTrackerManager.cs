using System;
using System.Net.Http;
using System.Net.Http.Headers;
using ZlatkoBandrov.Common;

namespace ZlatkoBandrov.BusinessLogic.Managers
{
    public class ArrivalTrackerManager
    {
        public void InitializeCommunication()
        {
            string webApiUrl = Utils.GetArrivalTrackerUrl();
            string trackingDate = DateTime.Now.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string requestUrl = string.Format(@"http://localhost:51396/api/clients/subscribe?date={0}&callback={1}", trackingDate, webApiUrl);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(requestUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

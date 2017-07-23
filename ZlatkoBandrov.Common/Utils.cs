using System;
using System.Globalization;
using System.Net.Http;
using System.Web;
using System.Linq;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.Common
{
    public static class Utils
    {
        public static string GetWebAppUrl()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            return url;
        }

        public static string GetArrivalTrackerUrl()
        {
            string baseUrl = GetWebAppUrl();
            string url = string.Format("{0}api/ArrivalTracker/TrackEmployeeArrival", baseUrl).ToLowerInvariant();
            return url;
        }

        public static string GetConfigSetting(string configKey)
        {
            string configValue = System.Configuration.ConfigurationManager.AppSettings[configKey];
            return configValue;
        }

        public static TokenData FromTokenResult(TokenResult result)
        {
            if (result == null)
            {
                return null;
            }

            DateTime tokenExpires = ParseDateFromUniversal(result.Expires);
            TokenData newTokenData = null;
            if (tokenExpires > DateTime.MinValue)
            {
                newTokenData = new TokenData { Token = result.Token, Expires = tokenExpires };
            }

            return newTokenData;
        }

        public static string GetRequestToken(HttpRequestMessage request)
        {
            var token = request.Headers.FirstOrDefault(h => h.Key.Equals("X-Fourth-Token", StringComparison.InvariantCultureIgnoreCase));
            string tokenValue = token.Value != null ? token.Value.FirstOrDefault() : null;

            return tokenValue;
        }

        public static DateTime ParseDateFromUniversal(string dateTimeText)
        {
            string dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
            DateTime convertedDate = DateTime.MinValue;
            DateTime.TryParseExact(dateTimeText, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out convertedDate);
            return convertedDate;
        }
    }
}

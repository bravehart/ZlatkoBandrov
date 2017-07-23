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

            string dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
            DateTime tokenExpires = DateTime.MinValue;
            TokenData newTokenData = null;
            if (DateTime.TryParseExact(result.Expires, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out tokenExpires))
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
    }
}

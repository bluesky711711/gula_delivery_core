using ErpCore.Common.OpenStreetMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace ErpCore.Common
{
    public class OpenStreetMapApi
    {
        const string apiUrl = "https://nominatim.openstreetmap.org";

        public async Task<BaseNominatimResponse> Reverse(decimal lat, decimal lng)
        {
            var res = new BaseNominatimResponse();
            var url = string.Format("{0}/reverse?format=json&lat={1}&lon={2}&zoom=18&addressdetails=1", apiUrl, lat, lng);
            var response = await GetRequestAsync(url);
            res = JsonConvert.DeserializeObject<BaseNominatimResponse>(response);
            return res;
        }


        private async Task<string> GetRequestAsync(string url)
        {
            var result = string.Empty;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("f1ana.Nominatim.API", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
                result = await client.GetStringAsync(url).ConfigureAwait(false);
            }

            return result;
        }
    }
}
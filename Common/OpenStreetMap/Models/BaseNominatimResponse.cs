using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErpCore.Common.OpenStreetMap.Models
{
    public class BaseNominatimResponse
    {
        [JsonProperty("place_id")]
        public int PlaceID { get; set; }

        [JsonProperty("licence")]
        public string License { get; set; }

        [JsonProperty("osm_type")]
        public string OSMType { get; set; }

        [JsonProperty("osm_id")]
        public long OSMID { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("extratags")]
        public IDictionary<string, string> ExtraTags { get; set; }

        [JsonProperty("namedetails")]
        public IDictionary<string, string> AlternateNames { get; set; }

        [JsonProperty("address")]
        public AddressResult Address { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("type")]
        public string ClassType { get; set; }

        [JsonProperty("importance")]
        public double Importance { get; set; }

        [JsonProperty("icon")]
        public string IconURL { get; set; }
    }
}
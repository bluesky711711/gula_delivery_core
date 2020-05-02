using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErpCore.Common.OpenStreetMap.Models
{
    public class AddressResult
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("house_number")]
        public string HouseNumber { get; set; }

        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty("road")]
        public string Road { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("town")]
        public string Town { get; set; }

        [JsonProperty("pedestrian")]
        public string Pedestrian { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("hamlet")]
        public string Hamlet { get; set; }

        [JsonProperty("suburb")]
        public string Suburb { get; set; }

        [JsonProperty("village")]
        public string Village { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("state_district")]
        public string District { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
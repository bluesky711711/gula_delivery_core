using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpCore.EntityFramework.Entities
{
    public class AddressEntity
    {
        public string BlockNo { get; set; }
        public string StreetName { get; set; }
        public string Floor { get; set; }
        public string UnitNo { get; set; }
        public string BuildingName { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }
}

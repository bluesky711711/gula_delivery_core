using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpCore.EntityFramework.Entities
{
    public class GeofenceEntity
    {
        public enum GeofenceTypes {NA, Circle, Polygon, Retangle }

        public GeofenceTypes Type { get; set; } = 0;

        public DbGeography Geofence { get; set; }
    }
}

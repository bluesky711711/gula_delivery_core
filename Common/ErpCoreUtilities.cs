using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpCore.Common
{
    public static class ErpCoreUtilities
    {
        #region Geometry

        public static double ConeVolume(double height, double r1, double r2)
        {
            return (Math.PI * height) / 3 * (r1 * r1 + r2 * r2 + r1 * r2);
        }

        public static double CylinderVolume(double height, double r)
        {
            return Math.PI * r * r * height;
        }

        #endregion

        #region Genfence
        public static DbGeography DbGeoCreatePoint(decimal lat, decimal lng)
        {
            var text = string.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT({0} {1})", lng, lat);
            return DbGeography.PointFromText(text, DbGeography.DefaultCoordinateSystemId);
        }

        //public static DbGeography DbGeoPolygon(List<GeoCoor> points)
        //{
        //    if (points == null || !points.Any())
        //    {
        //        return null;
        //    }

        //    var pointArr = points.Select(t => string.Format("{0} {1}", t.Lng, t.Lat)).ToList();

        //    if (pointArr.First() != pointArr.Last())
        //    {
        //        pointArr.Add(pointArr.First());
        //    }

        //    var pointStr = string.Join(", ", pointArr);
        //    var pointTx = string.Format(CultureInfo.InvariantCulture.NumberFormat, pointStr);

        //    //var ts = SqlGeography.STMPolyFromText(new SqlChars(string.Format("POLYGON(({0}))", pointTx)), DbGeography.DefaultCoordinateSystemId).MakeValid();

        //    return DbGeography.PolygonFromText(string.Format("POLYGON(({0}))", pointTx), DbGeography.DefaultCoordinateSystemId);
        //    //return DbGeography.PointFromText(text, DbGeography.DefaultCoordinateSystemId);
        //    // return DbGeography.PointFromText(text, 4326);
        //}

        public static DbGeography DbGeoPolygon2(List<GeoCoor> points)
        {
            if (points == null || !points.Any())
            {
                return null;
            }

            var pointArr = points.Select(t => string.Format("{0} {1}", t.Lng, t.Lat)).ToList();

            if (pointArr.First() != pointArr.Last())
            {
                pointArr.Add(pointArr.First());
            }

            var pointStr = string.Join(", ", pointArr);
            var pointTx = string.Format(CultureInfo.InvariantCulture.NumberFormat, pointStr);
            var wktStr = string.Format("POLYGON(({0}))", pointTx);

            var sqlGeography = SqlGeography.STGeomFromText(new SqlChars(wktStr), DbGeography.DefaultCoordinateSystemId).MakeValid();
            var invertedSqlGeography = sqlGeography.ReorientObject();
            if (sqlGeography.STArea() > invertedSqlGeography.STArea())
            {
                sqlGeography = invertedSqlGeography;
            }

            var ts = sqlGeography.ToString();

            return DbGeography.PolygonFromText(ts, DbGeography.DefaultCoordinateSystemId);
            //return DbSpatialServices.Default.GeographyFromProviderValue(sqlGeography);
        }


        public static DbGeography DbGeoCircle(decimal lat, decimal lng, decimal raidusKm)
        {
            DbGeography point = DbGeoCreatePoint(lat, lng);
            DbGeography targetCircle = point.Buffer((double)raidusKm);
            return targetCircle;
        }

        public static DbGeography DbGeoRectangle(decimal north, decimal east, decimal south, decimal west)
        {
            var pointStr = string.Format("POLYGON(({0} {2}, {0} {3}, {1} {3}, {1} {2}, {0} {2}))", west, east, north, south);
            return DbGeography.PolygonFromText(pointStr, DbGeography.DefaultCoordinateSystemId);
        }


        #endregion



    }
}

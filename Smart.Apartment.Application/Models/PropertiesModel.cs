using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Apartment.Application.Models
{
    public class PropertiesModel: CommonModels
    {
        public int PropertyID { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}

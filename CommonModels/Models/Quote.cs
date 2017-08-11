using Common.Interfaces;
using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Quote : IQuote
    {
        public int Id { get; set; }
        public ILocation Pickup { get; set; }
        public ILocation Delivery { get; set; }
        public ETrailerType TrailerType { get; set; }
        public bool VehicleIsOperable { get; set; }
        public IVehicleMinimal Vehicle { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

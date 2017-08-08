using System.ComponentModel.DataAnnotations;

namespace Quote.Models
{
    public enum VehicleType
    {
        Unknown,
        ATV,
        Boat,
        Car,
        [Display(Name = "Heavy Equipment")]
        HeavyEquipment,
        [Display(Name = "Large Yacht")]
        LargeYacht,
        Motorcycle,
        Pickup,
        RV,
        SUV,
        [Display(Name = "Travel Trailer")]
        TravelTrailer,
        Van,
        Other

    }
    public enum VehicleSize
    {
        Large,
        Small
    }
}

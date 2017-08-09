using System.ComponentModel.DataAnnotations;

namespace Enums.Models
{
    public enum EVehicleType
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
   
}

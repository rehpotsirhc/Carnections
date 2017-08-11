using Common.Interfaces;

namespace Common.Models
{
    public class Price : IPrice
    {
        public double DriverPay { get; set; }
        public double Deposit { get; set; }

        public double Total
        {
            get
            {
                return DriverPay + Deposit;
            }
        }
    }
}

using Common.Interfaces;
using Common.Models;

namespace Common.Utils
{
    public static class PriceBuilder
    {

        public static IPrice Build(double deposit, double driverPay)
        {
            return new Price()
            {
                DriverPay = driverPay,
                Deposit = deposit
            };
        }
    }
}

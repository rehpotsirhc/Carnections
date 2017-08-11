using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

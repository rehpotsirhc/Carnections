using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Price
    {
        public double DriverPay { get; private set; }
        public double Deposit { get; private set; }

        public double Total
        {
            get
            {
                return DriverPay + Deposit;
            }
        }

        public Price(double driverPay, double deposit)
        {
            this.DriverPay = driverPay;
            this.Deposit = deposit;
        }
    }
}

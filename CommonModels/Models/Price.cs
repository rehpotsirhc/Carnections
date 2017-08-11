using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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

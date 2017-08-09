using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IPrice
    {
        double DriverPay { get; }
        double Deposit { get; }
        double Total { get; }
    }
}

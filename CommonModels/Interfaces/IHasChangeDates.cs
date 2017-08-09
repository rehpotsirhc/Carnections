using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IHasChangeDates
    {
        DateTime ModifiedDate { get; set; }
        DateTime CreatedDate { get; set; }
    }
}

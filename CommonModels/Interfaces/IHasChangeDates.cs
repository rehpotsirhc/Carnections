using System;

namespace Common.Interfaces
{
    public interface IHasChangeDates
    {
        DateTime ModifiedDate { get; set; }
        DateTime CreatedDate { get; set; }
    }
}

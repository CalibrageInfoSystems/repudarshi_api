using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    };
    public enum Roles
    {
        SuperAdmin = 1,
        Vendor =2
    }
    public enum Service
    {
        Gold = 5,
        Silver = 6
    }
    public enum OrderStatus
    {
        Picking=1,
        InTransit=2,
        Delivered=3,
        Cancelled=4,
        Received=5,
        CollectedFromStore=6
    }   
    
    public enum VendorStatus
    {
        ApprovalforPending = 1,
        Approved = 2,
        Declined = 3,
        Deleted = 4
    }
}

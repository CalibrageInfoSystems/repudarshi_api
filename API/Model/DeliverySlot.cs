//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeliverySlot
    {
        public int Id { get; set; }
        public string DayName { get; set; }
        public string Slot { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}

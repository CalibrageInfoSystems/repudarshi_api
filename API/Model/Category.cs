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
    
    public partial class Category
    {
        public int Id { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Nullable<int> ParentCategoryId { get; set; }
        public Nullable<int> CategoryLevel { get; set; }
        public string FileLocation { get; set; }
        public string FileName { get; set; }
        public string FileExtention { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}

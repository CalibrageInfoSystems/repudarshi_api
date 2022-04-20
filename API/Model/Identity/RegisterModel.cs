﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Identity
{
    public class RegisterModel
    {

        //[Display(Name = "Id")]
        //public int? Id { get; set; }

        //[Required]
        //[Display(Name = "First name")]
        //public string FirstName { get; set; }

        //[Display(Name = "Middle name")]
        //public string MiddleName { get; set; }

        //[Required]
        //[Display(Name = "Last name")]
        //public string LastName { get; set; }

        //[Required]
        //[Display(Name = "Contact No")]
        //public string ContactNumber { get; set; }

        //[Required]
        //[Display(Name = "Username")]
        //public string UserName { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }


        //[DataType(DataType.EmailAddress)]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        //[Display(Name = "Address")]
        //public string Address { get; set; }
        //[Display(Name = "Landmark")]
        //public string Landmark { get; set; }
        //public int? CountryId { get; set; }
        //public int? CityId { get; set; }
        //public int? LocationId { get; set; }
        //public string DeviceToken { get; set; }
        [Display(Name = "Id")]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }


        [Display(Name = "Contact No")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "Mobile No")]
        public string MobileNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "GSTIN")]
        public string GSTIN { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Address1")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "Address2")]
        public string Address2 { get; set; }
        
        [Display(Name = "Address3")]
        public string Address3 { get; set; }
        
        [Display(Name = "Landmark")]
        public string Landmark { get; set; }

        [Required]
        [Display(Name = "Pincode")]
        public int? Pincode { get; set; }

        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }

        [Required]
        [Display(Name = "RoleId")]
        public int RoleId { get; set; }

        [Required]
        [Display(Name = "ServiceTypeId")]
        public int ServiceTypeId { get; set; }


        [Display(Name = "Created By UserId")]
        public int? CreatedByUserId { get; set; }

        [Display(Name = "Updated By UserId")]
        public int? UpdatedByUserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

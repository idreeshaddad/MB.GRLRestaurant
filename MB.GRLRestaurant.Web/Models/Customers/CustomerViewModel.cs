﻿using MB.GRLRestaurant.Utils.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MB.GRLRestaurant.Web.Models.Customers
{
    public class CustomerViewModel
    {
        public int Id { get; set; }


        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
    }
}

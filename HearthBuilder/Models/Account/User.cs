using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HearthBuilder.Models.Account
{
    public class User
    {
        public int ID { get; set; }
        public String Fname { get; set; }
        public String Lname { get; set; }

        [Required(ErrorMessage = "An email is required.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        public String Email { get; set; }

        [Required(ErrorMessage = "A password is required.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public User() { }

        public override string ToString()
        {
            return ID + ", " + Fname + ", " + Lname + ", " + Email + ", " + Password;
        }

    }
}
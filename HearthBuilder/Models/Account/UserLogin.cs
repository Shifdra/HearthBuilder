using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.Account
{
    public class UserLogin
    {
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Password is a required field.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public UserLogin() { }

        public override string ToString()
        {
            return ID + ", " + Email + ", " + Password;
        }

    }
}
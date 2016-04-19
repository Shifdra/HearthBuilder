using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HearthBuilder.Models.Account
{
    public class UserRegister
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is a required field.")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last name is a required field.")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Password is a required field.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public UserRegister() { }

        public override string ToString()
        {
            return ID + ", " + FirstName + ", " + LastName + ", " + Email + ", " + Password;
        }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.Account
{
    public class UserLogin : User
    {
        public override int ID { get; set; }
        public override String FirstName { get; set; }
        public override String LastName { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        public override String Email { get; set; }

        [Required(ErrorMessage = "Password is a required field.")]
        [DataType(DataType.Password)]
        public override String Password { get; set; }

        public UserLogin() { }

        public override string ToString()
        {
            return ID + ", " + Email + ", " + Password;
        }

    }
}
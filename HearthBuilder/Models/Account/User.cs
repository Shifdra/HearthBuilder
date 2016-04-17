using System;
using System.ComponentModel.DataAnnotations;

namespace HearthBuilder.Models
{
    public class User
    {
        public int ID { get; set; }
        public String Fname { get; set; }
        public String Lname { get; set; }

        [Required]
        [Display(Name = "Email")]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public User() { }
    }
}
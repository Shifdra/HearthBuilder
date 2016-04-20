using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthBuilder.Models.Account
{
    public class User
    {
        public virtual int ID { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String Email { get; set; }
        public virtual String Password { get; set; }

        public User() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models
{
    public class DeckException : Exception
    {
        public DeckException()
        {
        }

        public DeckException(string message) : base(message)
        {
            
        }
    }
}
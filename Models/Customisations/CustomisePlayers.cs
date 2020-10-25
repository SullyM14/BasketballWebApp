using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballWebApp.Models
{
    public partial class Players
    {
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}

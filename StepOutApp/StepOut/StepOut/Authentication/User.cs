using StepOut.Authorize;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepOut.Authentication
{
    public class UserBO
    {
        public String Naam { get; set; }
        public String Wachtwoord { get; set; }
        public String Email { get; set; }
        public String Land { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StepOutApi.Model
{
    class ReplaceUser
    {

        public String Naam { get; set; }
        public String Wachtwoord { get; set; }
        public String Email { get; set; }
        public String Land { get; set; }
        public TokenModel Token { get; set; }
        public Guid id { get; set; }
    }
}

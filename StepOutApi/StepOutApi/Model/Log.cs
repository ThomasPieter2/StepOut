using System;
using System.Collections.Generic;
using System.Text;

namespace StepOutApi.Model
{
    public class logging
    {
        public List<Log> Logs { get; set; }
    }
    public class Log
    {
        public string ErrorMessage { get; set; }
        public DateTime ErrorDateTime { get; set; }
    }
}

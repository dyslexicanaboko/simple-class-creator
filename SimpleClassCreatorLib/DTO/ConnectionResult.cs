using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.DTO
{
    public class ConnectionResult
    {
        public bool Success { get; set; }
        
        public string Message { get { return ReturnedException == null ? string.Empty : ReturnedException.Message; } }
        
        public Exception ReturnedException { get; set; }
    }
}

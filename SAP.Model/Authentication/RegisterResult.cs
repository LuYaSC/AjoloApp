using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AjoloApp.Model.Authentication
{
    public class RegisterResult
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; } = 0;
    }
}

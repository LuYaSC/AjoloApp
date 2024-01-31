using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AjoloApp.Model.Authentication
{
    public class LoginResult
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }

        public DateTime? Expirate { get; set; }
    }
}

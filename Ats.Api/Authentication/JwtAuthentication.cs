using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ats.Api.Authentication
{
    public class JwtAuthentication : AuthorizeAttribute
    {
        public JwtAuthentication(params string[] roles) : base()
        {
            Roles = string.Join(",", roles); 
        }
    }
}

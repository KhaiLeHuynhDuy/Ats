using Ats.Api.Model;
using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain;
using Ats.Domain.Identity.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Api.Authentication
{
    public class TokenProvider
    {   
        public string LoginUser(string Email, string Password,Guid UserId, string secretKey, string domain)
        {         
         
                var handler = new JwtSecurityTokenHandler();
                

                var key = Encoding.ASCII.GetBytes(secretKey);
                var signingkey = new SymmetricSecurityKey(key);
                //Generate Token for user 
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = domain,
                    Audience = domain,
                    SigningCredentials = new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256),
                    Subject = GetUserClaims(Email, UserId, Password),   
                    Expires = DateTime.Now.Add(TimeSpan.FromDays(1)),
                    NotBefore = DateTime.Now
                });              
                return handler.WriteToken(securityToken);
        }


        private ClaimsIdentity GetUserClaims(string email, Guid userId, string password)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userId.ToString()),
                new[] {
                    new Claim("Password",password),
                    new Claim(ClaimTypes.Email, email)
                });
            return identity;
        }     
    }
}

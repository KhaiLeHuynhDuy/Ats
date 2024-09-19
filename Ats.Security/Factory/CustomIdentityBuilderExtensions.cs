using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Ats.Security.Factory
{
    public static class CustomIdentityBuilderExtensions
    {
        public static IdentityBuilder AddForgotPasswordTotpTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var totpProvider = typeof(PasswordlessLoginTotpTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("ForgotPasswordTotpProvider", totpProvider);
        }
    }
}

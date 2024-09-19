﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Identity.Models
{
    [Table("usertokens")]
    public class UserToken : IdentityUserToken<Guid>
    {
    }
}
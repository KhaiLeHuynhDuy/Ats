﻿using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Member;

namespace Ats.Data.Repositories.Member
{
    public class MemberSegmentRepository : Repository<Ats.Domain.Member.Models.MemberSegment, Guid>, IMemberSegmentRepository
    {
        public MemberSegmentRepository(SCDataContext context) : base(context)
        {
        }
    }
}

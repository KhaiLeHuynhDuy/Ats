using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Member
{
    public class TagKeyRepository : Repository<TagKey, int>, ITagKeyRepository
    {
        public TagKeyRepository(SCDataContext context) : base(context)
        {

        }
    }
}

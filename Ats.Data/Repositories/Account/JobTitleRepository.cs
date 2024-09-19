using Ats.Data.EntityFramework;
using Ats.Domain.Account;
using Ats.Domain.Account.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Account
{
    public class JobTitleRepository : Repository<JobTitle, int>, IJobTitleRepository
    {
        public JobTitleRepository(SCDataContext context) : base(context)
        {
        }
    }
}

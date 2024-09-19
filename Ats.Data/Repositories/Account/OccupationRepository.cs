using Ats.Data.EntityFramework;
using Ats.Domain.Account;
using Ats.Domain.Account.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Account
{
    public class OccupationRepository : Repository<Occupation, int>, IOccupationRepository
    {
        public OccupationRepository(SCDataContext context) : base(context)
        {
            
        }
        public int GetMaxOccupationId()
        {
            try
            {
                return this.GetAll().Max(x=>x.Id);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

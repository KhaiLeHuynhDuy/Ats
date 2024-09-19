using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Domain.Accounts.Models
{ 
    public enum TRANSACTION_TYPE
    {
        USED = 0,
        WITHDRAW = 1,
        TRANSFERED = 2,
        MANUAL = 3,
        REFUNDED = 4
    }
}

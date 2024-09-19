using System;

namespace Ats.Domain
{
    public interface IEntityTrackingModified
    {
        DateTime DateModified { set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain
{
    [Serializable]
    public abstract class EntityBase : IObjectState
    {
        [NotMapped]
        public ObjectState State { get; set; }
    }
}

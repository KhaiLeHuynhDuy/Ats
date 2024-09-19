using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState State { get; set; }
    }
}
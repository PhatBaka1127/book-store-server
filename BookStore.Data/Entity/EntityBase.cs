using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Data.Entity
{
    public class EntityBase
    {
        public EntityEnum Status {get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }

    public enum EntityEnum
    {
        INACTIVE,
        ACTIVE
    }
} 
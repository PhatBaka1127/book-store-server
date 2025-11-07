using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entity
{
    [Table(nameof(Shop))]
    public class Shop : IdentityEntity
    {
        public Shop()
        {
            Users = [];
        }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public decimal Lon { get; set; }
        public decimal Lat { get; set; }

        [InverseProperty(nameof(User.Shops))]
        public ICollection<User> Users { get; set; }
    }
}
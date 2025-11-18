using BookStore.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Dto
{
    public class GetCategoryDTO
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public int? status { get; set; }
        public ICollection<GetBookResponse>? books { get; set; }
    }
}

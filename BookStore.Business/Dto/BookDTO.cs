using BookStore.Data.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Dto
{
    public class GetBookDTO
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public decimal? unitPrice { get; set; }
        public int? stock { get; set; }
        public int? status { get; set; }
        public string? image { get; set; }
        public int? sellerId { get; set; }
        public string? sellerName { get; set; }
        public int? categoryId { get; set; }
        public string? categoryName { get; set; }
    }

    public class CreateBookDTO
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public decimal? unitPrice { get; set; }
        public int? stock { get; set; }
        public int? status { get; set; }
        public IFormFile? image { get; set; }
        public int? categoryId { get; set; }
    }

    public class BookFilter
    {
        public string? name { get; set; }
    }
}

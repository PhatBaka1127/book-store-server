using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Dto
{
    public class ErrorResponse
    {
        public string? code { get; set; }
        public string? message { get; set; }
        public bool result { get; set; }
    }
}

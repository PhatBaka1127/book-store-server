using BookStore.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Dto
{
    public class ThisUserObj
    {
        public int userId { get; set; }
        public string? email { get; set; }
        public int? role { get; set; }
    }

    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Password không được bỏ trống")]
        public string? password { get; set; }
    }

    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Password không được bỏ trống")]
        public string? password { get; set; }
        [Required(ErrorMessage = "Role không được bỏ trống")]
        public int role { get; set; }
    }

    public class AuthDTO
    {
        public int id { get; set; }
        public string? email { get; set; }
        public int role { get; set; } 
        public int status { get; set; }
        public string? accessToken { get; set; }
    }

    public class GetUserDTO
    {
        public int? id { get; set; }
        public string? email { get; set; }
        public int? role { get; set; }
        public int? status { get; set; }
        public string? image { get; set; }
        public ICollection<GetOrderDTO>? orders { get; set; }
        public ICollection<GetBookDTO>? books { get; set; }
    }

    public enum RoleEnum
    {
        BUYER = 0,
        SELLER = 1
    }
}

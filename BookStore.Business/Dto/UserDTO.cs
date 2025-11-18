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

    public class LoginRequest
    {
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Password không được bỏ trống")]
        public string? password { get; set; }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Password không được bỏ trống")]
        public string? password { get; set; }
        [Required(ErrorMessage = "Role không được bỏ trống")]
        public int role { get; set; }
    }

    public class AuthResponse
    {
        public int id { get; set; }
        public string? email { get; set; }
        public int role { get; set; }
        public string? status { get; set; }
        public string? accessToken { get; set; }
        public DateTime? expireAt { get; set; }
        public string? refreshToken { get; set; }
    }
    
    public class TokenRequest
    {
        [Required(ErrorMessage = "Access token is required")]
        public string accessToken { get; set; } = string.Empty;
        [Required(ErrorMessage = "Refresh token is required")]
        public string refreshToken { get; set; } = string.Empty;
    }

    public class UserResponse
    {
        public int? id { get; set; }
        public string? email { get; set; }
        public int? role { get; set; }
        public int? status { get; set; }
        public string? image { get; set; }
        public ICollection<OrderResponse>? orders { get; set; }
        public ICollection<GetBookResponse>? books { get; set; }
    }

    public enum RoleEnum
    {
        BUYER = 0,
        SELLER = 1,
        SHIPPER = 2,
        ADMIN = 3
    }
}

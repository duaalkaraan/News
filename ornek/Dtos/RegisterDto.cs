using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ornek.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "كلمة المرور  مطلوبة!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب!")]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين!")]
        public string ConfirmPassword { get; set; }
    }
}

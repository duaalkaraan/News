using System.ComponentModel.DataAnnotations;

namespace ornek.Models
{
    public class User
    {

        public int Id { get; set; }
        [Required(ErrorMessage ="اسم المستخدم مطلوب!")]
        public string Username { get; set; }
        [Required(ErrorMessage = " كلمة المرور مطلوبة!")]
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LoginPage.Models // Kendi proje adınla düzenlemeyi unutma
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı veya e-posta alanı zorunludur.")]
        [Display(Name = "Kullanıcı Adı veya E-Posta")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}

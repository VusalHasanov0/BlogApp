using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Model 
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }
        [Required]
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }
        
        [Required]
        [StringLength(10,ErrorMessage ="Mak. 10 karakter belirtiniz")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password) , ErrorMessage = "Parolaniz Eslesmiyor")]
        [Display(Name = "Parola Tekrar")]
        public string? ConfirmPassword { get; set; }
    }
}
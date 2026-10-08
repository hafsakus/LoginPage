using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginPage.Models
{
    [Table("Assignment")]
    public class Assignment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Ders adı zorunludur.")]
        [Display(Name = "Ders Adı")]
        public string ClassName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ödev başlığı zorunludur.")]
        [Display(Name = "Ödev Başlığı")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Son Teslim Tarihi")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Yapılacak";
    }
}

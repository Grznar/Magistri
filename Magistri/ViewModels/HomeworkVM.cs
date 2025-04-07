using Magistri.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.ViewModels
{
    public class HomeworkVM
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        [Required]
        [ForeignKey("Class")]
        public int ClassIdKey { get; set; }
        [ValidateNever]
        public Class Class { get; set; }
        [ValidateNever]
        public List<SelectListItem> ClassList { get; set; }
    }
}

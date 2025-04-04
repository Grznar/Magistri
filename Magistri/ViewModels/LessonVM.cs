using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistri.ViewModels
{
    public class LessonVM
    {
        public int Id { get; set; }

        public string? Description { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem>? TeacherList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? SubjectList { get; set; }
        
        
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string TeacherId { get; set; }
    }
}

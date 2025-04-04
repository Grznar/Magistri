using Magistri.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.ViewModels
{
    public class TimeTableEntryVM
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
        [ValidateNever]
        public Class Class { get; set; }


        public DayOfWeek Day { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? ClassList { get; set; }

        public IEnumerable<Lesson>? LessonsForClass { get; set; }

        public int TimeSlot { get; set; }

       
        [ForeignKey("Lesson")]
        public string LessonId { get; set; }
        [ValidateNever]
        public Lesson Lessons { get; set; }


    }
}

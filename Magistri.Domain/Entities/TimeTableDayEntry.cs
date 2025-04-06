using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Magistri.Domain.Entities
{
    public class TimeTableDayEntry
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TimetableEntry")]
        public int TimetableEntryId { get; set; }

        [ValidateNever]
        public TimeTableEntry TimetableEntry { get; set; }

        public string? Day { get; set; }
        
        public int? LessonId { get; set; }

        [ValidateNever]
        public Lesson Lesson { get; set; }
    }


}

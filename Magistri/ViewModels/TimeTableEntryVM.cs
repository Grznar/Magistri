using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Magistri.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
namespace Magistri.ViewModels
{
    public class TimetableDayEntryCreateVM
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        [ValidateNever]
        public Class Class { get; set; }

        // Navigační vlastnost pro denní záznamy
        [ValidateNever]
        public List<TimetableDayEntryItemVM> DayEntries { get; set; } = new List<TimetableDayEntryItemVM>();
        [ValidateNever]

        public List<SelectListItem>? ClassList { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public List<SelectListItem> LessonList { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public Dictionary<int, int> LessonIds { get; set; } = new Dictionary<int, int>();

    }

    public class TimetableDayEntryItemVM
    {
        [Key]   
        public int Id { get; set; }

        [ForeignKey("TimetableEntry")]
        public int TimetableEntryId { get; set; }

        [ValidateNever]
        public TimeTableEntry TimetableEntry { get; set; }

        public string Day { get; set; }
        public List<DayHourVM> Hours { get; set; } = new List<DayHourVM>();
        public List<int>?LessonIds { get; set; } = new List<int>();

        public List<int?> HourNumbers { get; set; } = new List<int?>();
        [ValidateNever]
        public List<SelectListItem> LessonList { get; set; } = new List<SelectListItem>();



    }
    public class DayHourVM
    {
        public int? LessonId { get; set; }
    }
}

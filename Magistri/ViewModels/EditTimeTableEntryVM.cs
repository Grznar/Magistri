using Magistri.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.ViewModels
{
    public class EditTimeTableEntryVM
    {
        
        [ValidateNever]
        public TimeTableEntry TimeTableEntry { get; set; }
        [ValidateNever]
        public List<TimeTableDayEntry> DayEntries { get; set; }
        [ValidateNever]
        public List<SelectListItem> LessonList { get; set; }


    }
}

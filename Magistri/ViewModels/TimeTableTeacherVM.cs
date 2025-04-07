using Magistri.Domain.Entities;

namespace Magistri.ViewModels
{
    public class TimeTableTeacherVM
    {
        public IEnumerable<TimeTableDayEntry> TimeTableEntries { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

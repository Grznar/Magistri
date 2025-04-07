using Magistri.Domain.Entities;

namespace Magistri.ViewModels
{
    public class TimeTableVM
    {
        public IEnumerable<TimeTableDayEntry> TimeTableEntries { get; set; }
        public Class Class { get; set; }
    }
}

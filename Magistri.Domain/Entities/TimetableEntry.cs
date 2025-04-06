using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Magistri.Domain.Entities
{
    public class TimeTableEntry
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        [ValidateNever]
        public Class Class { get; set; }

        // Navigační vlastnost pro denní záznamy
        public List<TimeTableDayEntry> DayEntries { get; set; } = new List<TimeTableDayEntry>();


    }

}

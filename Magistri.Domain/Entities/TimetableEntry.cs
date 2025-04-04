using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Domain.Entities
{
    public class TimetableEntry
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
        [ValidateNever]
        public Class Class { get; set; }

        
        public DayOfWeek Day { get; set; }

       
        public int TimeSlot { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [ValidateNever]
        public Subject Subject { get; set; }
        [ForeignKey("Students")]
        public string TeacherId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}

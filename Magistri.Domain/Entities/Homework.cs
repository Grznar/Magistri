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
    public class HomeWork
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        [ForeignKey("Class")]
        [Required]
        public int ClassIdKey { get; set; }
        [ValidateNever]
        public Class Class { get; set; }
    }
}

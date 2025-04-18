﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Domain.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
       
        public string? Description { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [ValidateNever]
        public Subject Subject { get; set; }
        [ForeignKey("ApplicationUser")]
        public string TeacherId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        

    }
}

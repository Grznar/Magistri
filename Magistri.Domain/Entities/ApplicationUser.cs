
using Microsoft.AspNetCore.Identity;
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
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        [ForeignKey(("Class"))]
        public int? StudentClassId { get; set; } = null;
        [ValidateNever]
        public Class Class { get; set; }


    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string FromId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ToId { get; set; }

        [ValidateNever]
        public ApplicationUser FromUser { get; set; }
        [ValidateNever]
        public ApplicationUser ToUser { get; set; }

        public string Topic { get; set; }
        public string MessageText { get; set; }


    }
}

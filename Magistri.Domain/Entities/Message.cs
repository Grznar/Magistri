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
        public class Message
        {
            [Key]
            public int Id { get; set; }
           
            public string FromUserId { get; set; }
        
            public string ToUserId { get; set; }

        [ForeignKey(nameof(FromUserId))]
        [ValidateNever]
        public ApplicationUser FromUser { get; set; }

        [ForeignKey(nameof(ToUserId))]
        [ValidateNever]
        public ApplicationUser ToUser { get; set; }



        public string Topic { get; set; }
            public string MessageText { get; set; }


        }
    }

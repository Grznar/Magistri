    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Magistri.Domain.Entities
    {
        public class Class
        {
            [Key]
            public int IdKey { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string? Description { get; set; }
            [Required]
            [MaxLength(4)]
            public string ShortName { get; set; }

            public string? HomeClass { get; set; }

        }
    }

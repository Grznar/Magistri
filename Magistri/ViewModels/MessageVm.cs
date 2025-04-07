using Magistri.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magistri.ViewModels
{
    public class MessageVm
    {
        public int Id { get; set; }

        [BindNever]
        [ValidateNever]
        public string FromId { get; set; }

        public string ToId { get; set; }


        public string Topic { get; set; }
        public string MessageText { get; set; }

        [ValidateNever]
        public List<SelectListItem> ApplicationUserList { get; set; }
    }
}

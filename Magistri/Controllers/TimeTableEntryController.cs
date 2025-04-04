using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.Controllers
{
    public class TimeTableEntryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public TimeTableEntryController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var list = _unitOfWork.Classes.GetAll().ToList();
            TimeTableEntryVM tteVm = new TimeTableEntryVM()
            {
                ClassList = list.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.IdKey.ToString()
                }),

            };
                    
                
            return View();
        }
    }
}


using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Collections.Generic;

namespace Magistri.Controllers
{
    public class TimeTableController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimeTableController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            IEnumerable<TimeTableDayEntry> list = new List<TimeTableDayEntry>();
            try
            {
                var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
                var userId = user.Id;
                var timeTableEntry = _unitOfWork.TimeTableEntry.Get(u => u.Class.IdKey == user.StudentClassId);

                 list = _unitOfWork.TimeTableDayEntry.GetAll(u => u.TimetableEntryId == timeTableEntry.Id,includeProperties: "Lesson,Lesson.ApplicationUser,Lesson.Subject");
                var classFromDb = _unitOfWork.Classes.Get(u => u.IdKey == user.StudentClassId);

                TimeTableVM timeTableVM = new TimeTableVM()
                {

                    Class = classFromDb,
                    TimeTableEntries = list
                };

                return View(timeTableVM);
            }
            catch(Exception)
            {
                return View();
            }
        }
    }
}

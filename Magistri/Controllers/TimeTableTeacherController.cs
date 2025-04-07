using Magistri.Application.Common.Interfaces;
using Magistri.Application.Common.Utlity;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Magistri.Controllers
{
    [Authorize(Roles = SD.Role_Teacher)]
    public class TimeTableTeacherController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public TimeTableTeacherController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
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


                list = _unitOfWork.TimeTableDayEntry.GetAll(u => u.Lesson.TeacherId == userId,includeProperties:"Lesson.Subject,TimetableEntry.Class");


                TimeTableTeacherVM timeTableVM = new TimeTableTeacherVM()
                {

                    ApplicationUser=user,
                    TimeTableEntries = list
                };

                return View(timeTableVM);
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}

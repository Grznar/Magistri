using Magistri.Application.Common.Interfaces;
using Magistri.Application.Common.Utlity;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.Controllers
{
    [Authorize(Roles = SD.Role_Teacher)]
    public class LessonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public LessonController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult GetAll()
        {
            List<Lesson> listLessons = _unitOfWork.Lessons.GetAll().ToList();
            foreach (var lesson in listLessons)
            {
                lesson.Subject = _unitOfWork.Subjects.Get(u => u.Id == lesson.SubjectId);
               
                lesson.ApplicationUser = _unitOfWork.Students.Get(u => u.Id == lesson.TeacherId);
            }
            return Json(new { data = listLessons });
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var teacherList = _userManager.GetUsersInRoleAsync(SD.Role_Teacher).Result;
            LessonVM lessonVM = new LessonVM()
            {
                
                TeacherList = teacherList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                SubjectList = _unitOfWork.Subjects.GetAll().Select(x => new SelectListItem
                {
                    Text = x.ShortName,
                    Value = x.Id.ToString()
                }),
                

            };
            return View(lessonVM);
        }
        [HttpPost]
        public IActionResult Create(LessonVM newLessonVM)
        {
            if (ModelState.IsValid)
            {

                Lesson newLesson = new Lesson()
                {
                    
                    Description = newLessonVM.Description,
                    SubjectId = newLessonVM.SubjectId,
                  
                    TeacherId = newLessonVM.TeacherId
                };
                _unitOfWork.Lessons.Add(newLesson);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View(newLessonVM);
            }
        }
        [Authorize]
        [HttpGet]

        
        public IActionResult List()
        {
            List<Lesson> listClasses = _unitOfWork.Lessons.GetAll().ToList();
            return View(listClasses);
        }
        public IActionResult Edit(int LessonId)
        {
            Lesson LessonFromDb = _unitOfWork.Lessons.Get(u => u.Id == LessonId);
            var teacherList = _userManager.GetUsersInRoleAsync(SD.Role_Teacher).Result;
            LessonVM lessonVM = new LessonVM()
            {

                TeacherList = teacherList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                SubjectList = _unitOfWork.Subjects.GetAll().Select(x => new SelectListItem
                {
                    Text = x.ShortName,
                    Value = x.Id.ToString()
                }),
                
                Id = LessonFromDb.Id,
                Description = LessonFromDb.Description,
                SubjectId = LessonFromDb.SubjectId,
              
                TeacherId = LessonFromDb.TeacherId


            };
            
            return View(lessonVM);
        }
        [HttpPost]
        public IActionResult Edit(LessonVM newLessonVM)
        {

            if (ModelState.IsValid)
            {
                Lesson newLesson = new Lesson()
                {
                    Id=newLessonVM.Id,
                    
                    Description = newLessonVM.Description,
                    SubjectId = newLessonVM.SubjectId,
              
                    TeacherId = newLessonVM.TeacherId
                };
                
                _unitOfWork.Lessons.Update(newLesson);
                _unitOfWork.Save();
                return RedirectToAction(nameof(List));
            }
            return View(newLessonVM);
        }
        public IActionResult Delete(int LessonId)
        {
            Lesson LessonFromDb = _unitOfWork.Lessons.Get(u => u.Id == LessonId);
            var teacherList = _userManager.GetUsersInRoleAsync(SD.Role_Teacher).Result;
            LessonVM lessonVM = new LessonVM()
            {

                TeacherList = teacherList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                SubjectList = _unitOfWork.Subjects.GetAll().Select(x => new SelectListItem
                {
                    Text = x.ShortName,
                    Value = x.Id.ToString()
                }),
                
                Id = LessonFromDb.Id,
                Description = LessonFromDb.Description,
                SubjectId = LessonFromDb.SubjectId,
               
                TeacherId = LessonFromDb.TeacherId


            };

            return View(lessonVM);
        }
            [HttpPost]
        public IActionResult Delete(LessonVM newLessonVM)
        {


            
                Lesson newLesson = new Lesson()
                {
                    Id = newLessonVM.Id,

                    Description = newLessonVM.Description,
                    SubjectId = newLessonVM.SubjectId,
                    
                    TeacherId = newLessonVM.TeacherId
                };

                _unitOfWork.Lessons.Delete(newLesson);
                _unitOfWork.Save();
                return RedirectToAction(nameof(List));
            
            


        }
        
    }
}

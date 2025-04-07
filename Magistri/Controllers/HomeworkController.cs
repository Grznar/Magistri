using Magistri.Application.Common.Interfaces;
using Magistri.Application.Common.Utlity;
using Magistri.Domain.Entities;
using Magistri.Infrastracture.Migrations;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQLitePCL;

namespace Magistri.Controllers
{

    public class HomeworkController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeworkController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [Authorize(Roles = SD.Role_Teacher)]
        public IActionResult IndexTeacher()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_Student)]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_Teacher)]
        public IActionResult Create()
        {
            var list = _unitOfWork.Classes.GetAll().ToList();

            var homeworkVM = new HomeworkVM()
            {
                ClassList = list.Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.IdKey.ToString()
                }).ToList()
            };
            return View(homeworkVM);
        }
        [HttpPost]
        public IActionResult Create(HomeworkVM homeworkVM)
        {
            if (ModelState.IsValid)
            {
                if (homeworkVM.DueDate <= DateTime.Now)
                {
                    ModelState.AddModelError("", "Datum musí být budoucí");

                    var list = _unitOfWork.Classes.GetAll().ToList();
                    homeworkVM.ClassList = list.Select(i => new SelectListItem()
                    {
                        Text = i.Name,
                        Value = i.IdKey.ToString()
                    }).ToList();
                    return View(homeworkVM);
                }

                var homework = new HomeWork()
                {
                    Description = homeworkVM.Description,
                    DueDate = homeworkVM.DueDate,
                    ClassIdKey = homeworkVM.ClassIdKey

                };
                _unitOfWork.Homework.Add(homework);
                _unitOfWork.Save();
                return RedirectToAction("IndexTeacher");
            }
            else
            {
                var list = _unitOfWork.Classes.GetAll().ToList();
                homeworkVM.ClassList = list.Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.IdKey.ToString()
                }).ToList();
                return View(homeworkVM);
            }
        }
        
        [Authorize(Roles = SD.Role_Teacher)]
        public IActionResult List()
        {


            return View();
        }
        [Authorize(Roles = SD.Role_Student)]
        public IActionResult MyList(int id)
        {
            




            return View(id);
        }

        [Authorize(Roles = SD.Role_Teacher)]
        public IActionResult Edit(int hwId)
        {
            var fromDb = _unitOfWork.Homework.Get(u => u.Id == hwId,includeProperties:"Class");
            if (fromDb == null) return NotFound();

            HomeworkVM homeworkVM = new HomeworkVM()
            {
                ClassList = _unitOfWork.Classes.GetAll().ToList().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.IdKey.ToString()
                }).ToList(),
                Description=fromDb.Description,
                Id=fromDb.Id,
                ClassIdKey=fromDb.ClassIdKey,
                Class=fromDb.Class,
                DueDate = fromDb.DueDate
            };

            return View(homeworkVM);
        }
        [HttpPost]
        public IActionResult Edit(HomeworkVM homeworkVM)
        {
            if(ModelState.IsValid)
            {
                if (homeworkVM.DueDate <= DateTime.Now)
                {
                    ModelState.AddModelError("", "Datum musí být budoucí");

                    var list = _unitOfWork.Classes.GetAll().ToList();
                    homeworkVM.ClassList = list.Select(i => new SelectListItem()
                    {
                        Text = i.Name,
                        Value = i.IdKey.ToString()
                    }).ToList();
                    return View(homeworkVM);
                }
                var homework = new HomeWork()
                {
                    Id=homeworkVM.Id,
                    Description = homeworkVM.Description,
                    DueDate = homeworkVM.DueDate,
                    ClassIdKey = homeworkVM.ClassIdKey
                    
                };
                _unitOfWork.Homework.Update(homework);
                _unitOfWork.Save();
                return RedirectToAction("IndexTeacher");
            }
            else
            {
                var list = _unitOfWork.Classes.GetAll().ToList();
                homeworkVM.ClassList = list.Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.IdKey.ToString()
                }).ToList();
                return View(homeworkVM);
            

        }
        }
        [Authorize(Roles = SD.Role_Teacher)]
        public IActionResult Delete(int hwId)
        {
            var fromDb = _unitOfWork.Homework.Get(u => u.Id == hwId, includeProperties: "Class");
            if (fromDb == null) return NotFound();

            HomeworkVM homeworkVM = new HomeworkVM()
            {
                ClassList = _unitOfWork.Classes.GetAll().ToList().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.IdKey.ToString()
                }).ToList(),
                Description = fromDb.Description,
                Id = fromDb.Id,
                ClassIdKey = fromDb.ClassIdKey,
                Class = fromDb.Class,
                DueDate = fromDb.DueDate
            };

            return View(homeworkVM);
        }
        [HttpPost]
        public IActionResult Delete(HomeworkVM homeworkVM)
        {
            
                var homework = new HomeWork()
                {
                    Id = homeworkVM.Id,
                    Description = homeworkVM.Description,
                    DueDate = homeworkVM.DueDate,
                    ClassIdKey = homeworkVM.ClassIdKey

                };
                _unitOfWork.Homework.Delete(homework);
                _unitOfWork.Save();
                return RedirectToAction("IndexTeacher");
           

            
        }
        #region API CALLS

        [Authorize(Roles =SD.Role_Teacher)]
        public IActionResult GetAllHomeworks()
        {
            var list = _unitOfWork.Homework.GetAll(includeProperties:"Class").ToList();

            return Json(new { data = list });
        }
        [Authorize(Roles = SD.Role_Student)]

        public IActionResult GetAllMyHomeWorks(int id)
        {
           

            var listOfHws = _unitOfWork.Homework.Get(u => u.ClassIdKey == id);





            return Json(new {data= listOfHws });
        }
        #endregion
    }
}

using Magistri.Application.Common.Interfaces;
using Magistri.Application.Common.Utlity;
using Magistri.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers
{
    [Authorize(Roles = SD.Role_Teacher)]
    public class SubjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubjectController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult GetAll()
        {
            List<Subject> listSubjects = _unitOfWork.Subjects.GetAll().ToList();
            return Json(new { data = listSubjects });
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Subject newSubject)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Subjects.Add(newSubject);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View(newSubject);
            }
        }
        [Authorize]
        [HttpGet]

        
        public IActionResult List()
        {
            List<Subject> listClasses = _unitOfWork.Subjects.GetAll().ToList();
            return View(listClasses);
        }
        public IActionResult Edit(int subjectId)
        {
            Subject subjectFromDb = _unitOfWork.Subjects.Get(u => u.Id == subjectId);
            if (subjectFromDb == null)
            {
                return NotFound();
            }
            return View(subjectFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Subject newSubject)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Subjects.Update(newSubject);
                _unitOfWork.Save();
                return RedirectToAction(nameof(List));
            }
            return View(newSubject);
        }
        public IActionResult Delete(int subjectId)
        {
            Subject subjectFromDb = _unitOfWork.Subjects.Get(u => u.Id == subjectId);
            if (subjectFromDb == null)
            {
                return NotFound();
            }
            return View(subjectFromDb);
        }
        [HttpPost]
        public IActionResult Delete(Subject newSubject)
        {


            _unitOfWork.Subjects.Delete(newSubject);
            _unitOfWork.Save();
            return RedirectToAction(nameof(List));


        }
        
    }
}

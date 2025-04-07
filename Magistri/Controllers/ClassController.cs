    using Magistri.Application.Common.Interfaces;
    using Magistri.Application.Common.Utlity;
    using Magistri.Domain.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    namespace Magistri.Controllers
    {
    [Authorize(Roles =SD.Role_Teacher)]
        public class ClassController : Controller
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly UserManager<ApplicationUser> _userManager;
            public ClassController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
            {
                _unitOfWork = unitOfWork;
                _userManager = userManager;
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
            public IActionResult Create(Class newClass)
            {
                if(ModelState.IsValid)
                {
                    _unitOfWork.Classes.Add(newClass);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newClass);
                }
            }
            [Authorize]
            [HttpGet]
        
            public IActionResult GetAll()
            {
                List<Class> listClasses = _unitOfWork.Classes.GetAll().ToList();
                return Json(new { data = listClasses });
            }
            public IActionResult List()
            {
                List<Class> listClasses = _unitOfWork.Classes.GetAll().ToList();
                return View(listClasses);
            }
            public IActionResult Edit( int classId)
            {
                Class classFromDb = _unitOfWork.Classes.Get(u=>u.IdKey == classId);
                if (classFromDb == null)
                {
                    return NotFound();
                }
                return View(classFromDb);
            }
            [HttpPost]
            public IActionResult Edit(Class newClass)
            {

                if (ModelState.IsValid)
                {
                    _unitOfWork.Classes.Update(newClass);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(List));
                }
                return View(newClass);
            }
            public IActionResult Delete(int classId)
            {
                Class classFromDb = _unitOfWork.Classes.Get(u => u.IdKey == classId);
                if (classFromDb == null)
                {
                    return NotFound();
                }
                return View(classFromDb);
            }
            [HttpPost]
            public IActionResult Delete(Class newClass)
            {

            
                    _unitOfWork.Classes.Delete(newClass);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(List));
            
           
            }
            public IActionResult WhichStudent(int classId)
            {

                return View(classId);
            }
            [HttpPost]
            public IActionResult AddStudent(string userId, int classId)
            {
                var user = _unitOfWork.Students.Get(u => u.Id == userId);
                if (user == null)
                {
                return RedirectToAction(nameof(List));
                }
                else
                {
                    user.StudentClassId = classId;
                    _unitOfWork.Students.Update(user);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(List));
                }
            }

            public async Task<IActionResult> GetAllStudents()
            {
                IList<ApplicationUser> listOfStueds = await _userManager.GetUsersInRoleAsync(SD.Role_Student);


             
                return Json(new { data = listOfStueds });
            }







        }
    }

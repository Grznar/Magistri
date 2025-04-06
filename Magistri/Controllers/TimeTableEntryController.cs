using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Magistri.Controllers
{
    public class TimeTableEntryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimeTableEntryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult List()
        {
            return View();
        }
        public IActionResult GetAllTTE()
        {
            var timeTableEntries = _unitOfWork.TimeTableEntry.GetAll(includeProperties:"Class");
            return Json(new { data = timeTableEntries });
        }
        public IActionResult GetAllTableDetails(int tableId)
        {
            var timeTableEntries = _unitOfWork.TimeTableDayEntry.GetAll(x => x.TimetableEntryId == tableId, includeProperties: "Lesson,Lesson.Subject,Lesson.ApplicationUser");
            return Json(new { data = timeTableEntries });
        }
        public IActionResult TableDetails(int tableId)
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: /TimeTableEntry/Create
        [HttpGet]
        public IActionResult Create()
        {
            var classes = _unitOfWork.Classes.GetAll().ToList();
            var lessonList = _unitOfWork.Lessons.GetAll(includeProperties: "Subject,ApplicationUser")
                .Select(l => new SelectListItem
                {
                    Text = (l.Subject?.ShortName ?? "No Subject") + " / " + (l.ApplicationUser?.Name ?? "No Teacher"),
                    Value = l.Id.ToString()
                }).ToList();

            var vm = new TimetableDayEntryCreateVM
            {
                ClassList = classes.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.IdKey.ToString()
                }).ToList(),
                LessonList = lessonList,
                DayEntries = new List<TimetableDayEntryItemVM>
                {
                    new TimetableDayEntryItemVM { Day = "Pondělí", LessonList = lessonList },
                    new TimetableDayEntryItemVM { Day = "Úterý",   LessonList = lessonList },
                    new TimetableDayEntryItemVM { Day = "Středa",  LessonList = lessonList },
                    new TimetableDayEntryItemVM { Day = "Čtvrtek", LessonList = lessonList },
                    new TimetableDayEntryItemVM { Day = "Pátek",   LessonList = lessonList }
                }
            };

            return View(vm);
        }

        // POST: /TimeTableEntry/Create
        [HttpPost]
        public IActionResult Create(TimetableDayEntryCreateVM vm)
        {
            foreach (var key in ModelState.Keys.Where(k => k.Contains("LessonIds")).ToList())
            {
                ModelState.Remove(key);
            }
            // Kontrola, zda již existuje rozvrh pro vybranou třídu
            var existingTimetable = _unitOfWork.TimeTableEntry.GetAll(x => x.ClassId == vm.ClassId).FirstOrDefault();
            if (existingTimetable != null)
            {
                ModelState.AddModelError("", "Rozvrh pro tuto třídu již existuje.");
                // Obnovení select listů...
                var classes = _unitOfWork.Classes.GetAll().ToList();
                vm.ClassList = classes.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.IdKey.ToString()
                }).ToList();
                var lessonList = _unitOfWork.Lessons.GetAll(includeProperties: "Subject,ApplicationUser")
                    .Select(l => new SelectListItem
                    {
                        Text = (l.Subject?.ShortName ?? "No Subject") + " / " + (l.ApplicationUser?.Name ?? "No Teacher"),
                        Value = l.Id.ToString()
                    }).ToList();
                vm.LessonList = lessonList;
                foreach (var dayEntry in vm.DayEntries)
                {
                    dayEntry.LessonList = lessonList;
                }
                return View(vm);
            }

            if (ModelState.IsValid)
            {
                var timetableEntry = new TimeTableEntry
                {
                    ClassId = vm.ClassId,
                    DayEntries = new List<TimeTableDayEntry>()
                };

                foreach (var dayEntry in vm.DayEntries)
                {
                    if (dayEntry.LessonIds != null)
                    {
                        foreach (var lessonId in dayEntry.LessonIds)
                        {
                            if (lessonId != null && lessonId > 0)
                            {
                                timetableEntry.DayEntries.Add(new TimeTableDayEntry
                                {
                                    Day = dayEntry.Day,
                                    LessonId = lessonId

                                });
                            }
                        }
                    }
                }

                _unitOfWork.TimeTableEntry.Add(timetableEntry);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            // Obnovení select listů při chybě validace
            var classes2 = _unitOfWork.Classes.GetAll().ToList();
            vm.ClassList = classes2.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.IdKey.ToString()
            }).ToList();

            var lessonList2 = _unitOfWork.Lessons.GetAll(includeProperties: "Subject,ApplicationUser")
                .Select(l => new SelectListItem
                {
                    Text = (l.Subject?.ShortName ?? "No Subject") + " / " + (l.ApplicationUser?.Name ?? "No Teacher"),
                    Value = l.Id.ToString()
                }).ToList();
            vm.LessonList = lessonList2;
            foreach (var dayEntry in vm.DayEntries)
            {
                dayEntry.LessonList = lessonList2;
            }
            return View(vm);
        }
    }
}

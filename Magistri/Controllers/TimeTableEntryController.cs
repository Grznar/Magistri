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
        [HttpGet]
        public IActionResult TableDetails(int tableId)
        {
            // Načteme hlavní rozvrh včetně třídy i denních záznamů
            var timetableEntry = _unitOfWork.TimeTableEntry.Get(
                x => x.Id == tableId,
                includeProperties: "Class,DayEntries"
            );
            if (timetableEntry == null)
            {
                return NotFound();
            }

            // Načteme globální seznam lekcí s vnořenými vlastnostmi
            var lessonList = _unitOfWork.Lessons.GetAll(includeProperties: "Subject,ApplicationUser")
                .Select(l => new SelectListItem
                {
                    Text = (l.Subject?.ShortName ?? "No Subject") + " / " + (l.ApplicationUser?.Name ?? "No Teacher"),
                    Value = l.Id.ToString()
                }).ToList();

            EditTimeTableEntryVM vm = new EditTimeTableEntryVM
            {
                TimeTableEntry = timetableEntry,
                DayEntries = timetableEntry.DayEntries.ToList(),
                LessonList = lessonList
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult TableDetails(EditTimeTableEntryVM vm)
        {
            // Načteme hlavní rozvrh včetně aktuálních DayEntries
            var timetableEntry = _unitOfWork.TimeTableEntry.Get(x => x.Id == vm.TimeTableEntry.Id, includeProperties: "DayEntries");
            if (timetableEntry == null)
            {
                return NotFound();
            }

            // Aktualizujeme třídu, pokud se změnila
            timetableEntry.ClassId = vm.TimeTableEntry.ClassId;

            // Smažeme všechny existující denní záznamy z DB
            var existingDayEntries = _unitOfWork.TimeTableDayEntry.GetAll(u => u.TimetableEntryId == timetableEntry.Id);
            foreach (var de in existingDayEntries)
            {
                _unitOfWork.TimeTableDayEntry.Delete(de);
            }

            // Vytvoříme novou kolekci DayEntries podle dat z formuláře (vm.DayEntries)
            if (vm.DayEntries != null)
            {
                foreach (var entry in vm.DayEntries)
                {
                    // Pokud je vybrána lekce (LessonId > 0) a den není prázdný, přidáme záznam
                    if (entry.LessonId > 0 && !string.IsNullOrEmpty(entry.Day))
                    {
                        timetableEntry.DayEntries.Add(new TimeTableDayEntry
                        {
                            Day = entry.Day,
                            LessonId = entry.LessonId,
                            TimetableEntryId = timetableEntry.Id // nastavíme správně FK
                        });
                    }
                }
            }

            _unitOfWork.TimeTableEntry.Update(timetableEntry);
            _unitOfWork.Save();

            return RedirectToAction("Index");
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

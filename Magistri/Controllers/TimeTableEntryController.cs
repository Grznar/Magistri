using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Magistri.Application.Common.Utlity;
using Microsoft.AspNetCore.Authorization;

namespace Magistri.Controllers
{
    [Authorize(Roles = SD.Role_Teacher)]
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
            var timetableEntry = _unitOfWork.TimeTableEntry.Get(
                x => x.Id == tableId,
                includeProperties: "Class,DayEntries"
            );
            if (timetableEntry == null)
            {
                return NotFound();
            }

            var lessonList = _unitOfWork.Lessons.GetAll(includeProperties: "Subject,ApplicationUser")
                .Select(l => new SelectListItem
                {
                    Text = (l.Subject?.ShortName ?? "No Subject") + " / " + (l.ApplicationUser?.Name ?? "No Teacher"),
                    Value = l.Id.ToString()
                }).ToList();

           
            var dayGroups = timetableEntry.DayEntries.GroupBy(de => de.Day);
            foreach (var group in dayGroups)
            {
               
                var assignedHours = group
                    .Where(de => de.HourNumber.HasValue)
                    .Select(de => de.HourNumber.Value)
                    .ToList();

                
                var available = Enumerable.Range(1, 8).Except(assignedHours).ToList();

               
                foreach (var entry in group.Where(de => !de.HourNumber.HasValue))
                {
                    if (available.Any())
                    {
                        entry.HourNumber = available.First();
                        available.RemoveAt(0);
                    }
                }
            }

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
     
            var timetableEntry = _unitOfWork.TimeTableEntry.Get(x => x.Id == vm.TimeTableEntry.Id, includeProperties: "DayEntries");
            if (timetableEntry == null)
            {
                return NotFound();
            }

       
            timetableEntry.ClassId = vm.TimeTableEntry.ClassId;

         
            var existingDayEntries = _unitOfWork.TimeTableDayEntry.GetAll(u => u.TimetableEntryId == timetableEntry.Id);
            foreach (var de in existingDayEntries)
            {
                _unitOfWork.TimeTableDayEntry.Delete(de);
            }

            if (vm.DayEntries != null)
            {
                foreach (var entry in vm.DayEntries)
                {
                    if (entry.LessonId > 0 || entry.HourNumber != null)
                    {
                        timetableEntry.DayEntries.Add(new TimeTableDayEntry
                        {
                            Day = entry.Day,
                            LessonId = entry.LessonId,
                            HourNumber = entry.HourNumber,
                            TimetableEntryId = timetableEntry.Id
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


        [HttpPost]
        public IActionResult Create(TimetableDayEntryCreateVM vm)
        {
            foreach (var key in ModelState.Keys.Where(k => k.Contains("LessonIds")).ToList())
            {
                ModelState.Remove(key);
            }
          
            var existingTimetable = _unitOfWork.TimeTableEntry.GetAll(x => x.ClassId == vm.ClassId).FirstOrDefault();
            if (existingTimetable != null)
            {
                ModelState.AddModelError("", "Rozvrh pro tuto třídu již existuje.");
           
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
                    if (dayEntry.LessonIds != null && dayEntry.HourNumbers != null)
                    {
                        for (int i = 0; i < dayEntry.LessonIds.Count; i++)
                        {
                            int? lessonId = dayEntry.LessonIds[i];
                            int? hourNumber = dayEntry.HourNumbers[i];
                            
                            if ((lessonId != null && lessonId > 0) || hourNumber != null)
                            {
                                timetableEntry.DayEntries.Add(new TimeTableDayEntry
                                {
                                    Day = dayEntry.Day,
                                    LessonId = lessonId,
                                    HourNumber = hourNumber
                                });
                            }
                        }
                    }
                }

                _unitOfWork.TimeTableEntry.Add(timetableEntry);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

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

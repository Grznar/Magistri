﻿using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Cosmos;
using System.ComponentModel.DataAnnotations.Schema;


namespace Magistri.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public MessageController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
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
            MessageVm messageVm = new MessageVm()
            {
                ApplicationUserList = _unitOfWork.Students.GetAll().ToList().Select(u => new SelectListItem()
                {
                    Value=u.Id,
                    Text=u.Name,
                }).ToList()
            };
            return View(messageVm);
        }
        [HttpPost]
        public IActionResult Create(MessageVm messageVm)
        {
            if(ModelState.IsValid)
            {
                Message message = new Message()
                {
                    FromId = _userManager.GetUserAsync(User).GetAwaiter().GetResult().Id,
                    ToId = messageVm.ToId,
                    Topic = messageVm.Topic,
                    MessageText = messageVm.MessageText
                };
                _unitOfWork.Message.Add(message);
                _unitOfWork.Save();
    
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult List()
        {
            var id = _userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;
            return View(id);
        }
        #region API CALSS
        public IActionResult GetAllMyMessages(string id)
        {
            var messages = _unitOfWork.Message.GetAll(u => u.ToId == id);
            return Json(new { data = messages });
        }
        #endregion


    }
}

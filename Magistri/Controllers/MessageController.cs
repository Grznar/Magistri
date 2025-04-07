using Magistri.Application.Common.Interfaces;
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
            messageVm.FromId = _userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;

            if (ModelState.IsValid)
            {
                Message message = new Message()
                {
                    FromUserId = _userManager.GetUserAsync(User).GetAwaiter().GetResult().Id,
                    ToUserId = messageVm.ToId,
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
            return View(model:id);
        }
        #region API CALSS
        public IActionResult GetAllMyMessages(string id)
        {
            var messages = _unitOfWork.Message.GetAll(m => m.ToUserId == id)
                .Select(m => new
                {
                    id = m.Id,
                    topic = m.Topic,
                    messageText = m.MessageText,
                    fromUserName = _unitOfWork.Students.Get(u=>u.Id==m.FromUserId).Name,
                }).ToList();
            return Json(new { data = messages });
        }
        #endregion


    }
}

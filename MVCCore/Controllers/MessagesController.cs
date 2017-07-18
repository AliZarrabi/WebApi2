using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models;
using Microsoft.Extensions.Configuration;
using MVCCore.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MVCCore.Controllers
{
    public class MessagesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        //Access appsettings.json from Controller
        private IConfiguration Configuration;
        //Access To Database
        //private ApplicationDbContext dbContext;
        private MessageRepository messageRepo;

        public MessagesController(UserManager<ApplicationUser> userMgr, IConfiguration configuration, MessageRepository mRepo)
        {
            this.Configuration = configuration;
            this.messageRepo = mRepo;
            //this.dbContext = adbc;
            userManager = userMgr;
        }

        [Route("/list")]
        public IActionResult Index()
        {
            ViewBag.AppSetting1 = Configuration["AliOption1"];
            ViewBag.AppSetting2 = Configuration.GetConnectionString("InfoConnection");

            return View(messageRepo.GetAll());
        }

        [HttpGet("/add")]
        public IActionResult Create()
        {
            ViewBag.Titr = "Title From Action";
            return View();
        }

        [HttpPost("/add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Message msg)
        {
            // Force To Enter Email (Email is not required in model)
            if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(msg.Email))
                ModelState.AddModelError("EmailRequired", "لطفا ایمیل را وارد نمایید");
            else
            {
                var user = await userManager.GetUserAsync(User);
                msg.UserID = user.Id;
                msg.Email = user.Email;
            }

            if (!ModelState.IsValid)
                return View(msg);

            messageRepo.Add(msg);

            ViewBag.Info = "Data Registered!";
            return View("Registerd", msg);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("[action]/{id}", Order = 1)]
        public IActionResult Edit(int id)
        {
            var msg = messageRepo.GetItem(id);
            if (msg == null)
            {
                return NotFound();
            }

            return View(msg);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("[action]/{id}", Order = 2)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Message msg)
        {
            if (!ModelState.IsValid)
                return View(msg);

            messageRepo.Update(msg);

            ViewBag.Info = "Data Updated!";
            return View("Registerd", msg);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public IActionResult Delete([FromBody]int modelID)
        {
            string result = string.Empty;
            try
            {
                messageRepo.Delete(modelID);
                result = "OK";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{email}")]
        public IActionResult DisplayByEmail(string email)
        {
            return View(messageRepo.GetByEmail(email));
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{id:int}")]
        public IActionResult DisplayMessage(int id)
        {
            return View(messageRepo.GetItem(id));
        }

        //==================== Web API
        [Route("~/api/messages")]
        public IEnumerable<MessageViewModel> GetAllMsg()
        {
            return messageRepo.GetAll();
        }

        [Route("~/api/messages/{email}")]
        public IActionResult GetByEmail(string email)
        {
            var msg = messageRepo.GetByEmail(email);
            if (msg.Count() == 0)
                return NotFound();
            else
                return new JsonResult(msg);
        }

        [Route("~/api/messages/{id:int}")]
        public IActionResult GetByID(int id)
        {
            var msg = messageRepo.GetItem(id);
            if (msg == null)
                return NotFound();
            else
                return new JsonResult(msg);
        }
    }
}
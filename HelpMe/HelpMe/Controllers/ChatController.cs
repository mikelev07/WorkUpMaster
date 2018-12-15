using HelpMe.Hubs;
using HelpMe.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HelpMe.Controllers
{
    public class ChatController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chat
        public async Task<ActionResult> Index(string userToId=null)
        {
            if (userToId != null)
            {
                string requestId = User.Identity.GetUserId();
                var userTo = db.Users.Include(u => u.Messages).Where(u => u.Id == userToId).FirstOrDefault();
                var userFrom = db.Users.Include(u => u.Messages).Where(u => u.Id == requestId).FirstOrDefault();

                var messages = db.Messages.Include(u => u.UserFrom)
                                          .Include(u => u.UserTo)
                                          .Where(m => (m.UserFromId == userFrom.Id && m.UserToId == userTo.Id) ||
                                          (m.UserFromId == userTo.Id) && m.UserToId == userFrom.Id).OrderBy(m => m.DateSend);

                ViewBag.Dialogs = await db.Messages.Include(u => u.UserFrom)
                                               .Include(u => u.UserTo)
                                               .Where(u => u.UserToId != requestId)
                                               .GroupBy(car => car.UserToId)
                                               .Select(g => g.FirstOrDefault()).ToListAsync();

                return View(await messages.ToListAsync());
            }
            else
            {
                string requestId = User.Identity.GetUserId();
                ViewBag.Dialogs = await db.Messages.Include(u => u.UserFrom)
                                              .Include(u => u.UserTo)
                                              .Where(u => u.UserToId != requestId)
                                              .GroupBy(car => car.UserToId)
                                              .Select(g => g.FirstOrDefault()).ToListAsync();
                return View();
            }
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMessage([Bind(Include = "Id,Description,DateSend,UserFromId,UserToId")] MessageStoreViewModel message)
        {
            if (ModelState.IsValid)
            {
                /*
                if (message.UserDialogId == null)
                {
                    UserDialog dialog = new UserDialog();
                    dialog.UserFromId = message.UserFromId;
                    dialog.UserToId = message.UserToId;
                    db.Dialogs.Add(dialog);
                    db.SaveChanges();
                   // message.UserDialogId = dialog.Id;
                } */
                message.DateSend = DateTime.Now;
                db.Messages.Add(message);
                await db.SaveChangesAsync();
                return RedirectToAction("Index","Chat", new { userToId = message.UserToId });
            }

            return View(message);
        }

       

        [HttpGet]
        public async Task<ActionResult> LoadHistory(string userName)
        {
            string requestId = User.Identity.GetUserId();
            var userTo = db.Users.Include(u => u.Messages).Where(u => u.UserName == userName).FirstOrDefault();
            var userFrom = db.Users.Include(u => u.Messages).Where(u => u.Id == requestId).FirstOrDefault();
           

            var messages = db.Messages.Include(u => u.UserFrom)
                                      .Include(u => u.UserTo)
                                      .Where(m => (m.UserFromId == userFrom.Id && m.UserToId == userTo.Id) || 
                                      (m.UserFromId == userTo.Id) && m.UserToId == userFrom.Id).OrderBy(m => m.DateSend);

            return PartialView(await messages.ToListAsync());
        }
    }
}
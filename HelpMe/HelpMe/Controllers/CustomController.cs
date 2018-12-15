using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelpMe.Models;
using Microsoft.AspNet.Identity;
using HelpMe.Hubs;
using System.IO;
using PagedList;

namespace HelpMe.Controllers
{
    public class CustomController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      
        // GET: Custom
        public ActionResult Index(int page = 1)
        {  
            var customViewModels = db.Customs.Include(c => c.Comments).Include(c => c.User).OrderBy(x => x.Id);
            int pageSize = 9; // количество объектов на страницу
            IEnumerable<CustomViewModel> customPerPages = customViewModels.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = customViewModels.Count() };
            CustomIndexViewModel ivm = new CustomIndexViewModel { PageInfo = pageInfo, Customs = customPerPages };
            return View(ivm);
        }

        public async Task<ActionResult> ChooseExecutor(int? id, string userId)
        {
            if (id == null && userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomViewModel customViewModel = await db.Customs.Include(c => c.Comments).Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            db.Entry(customViewModel).State = EntityState.Modified;

            if (customViewModel.UserId != customViewModel.ExecutorId)
            {
                customViewModel.ExecutorId = userId;
                customViewModel.Status = CustomStatus.Progress; // заявка выполняется
            }
           
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Custom", new { id = customViewModel.Id });
        }

        [HttpPost]
        public async Task<ActionResult> CustomSearch(string name)
        {
            var allCustoms = await db.Customs.Where(a => a.Name.Contains(name)).ToListAsync();
            if (allCustoms.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(allCustoms);
        }

        private void SendMessage(string message)
        {
            // Получаем контекст хаба
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            // отправляем сообщение
            context.Clients.All.displayMessage(message);
        }


        [HttpPost]
        public async Task<ActionResult> SendSolution(int? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomViewModel customViewModel = await db.Customs.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);

            if (customViewModel.ExecutorId == User.Identity.GetUserId())
            {
                if (ModelState.IsValid && upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    string path = Server.MapPath("~/Files/" + fileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(path);
                    db.Entry(customViewModel).State = EntityState.Modified;
                    customViewModel.FilePath = path;
                    customViewModel.Status = CustomStatus.Check; // заявка на проверке
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Details", "Custom", new { id = customViewModel.Id }); ;
        }

    
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetComment(int id)
        {
            var results = db.Comments.Select(e => new
            {
                e.Id,
                e.Name,
                e.Description
            }).Where(e => e.Id == id).FirstOrDefault();

            return new JsonResult() { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult People(string Description, int CustomViewModelId, int OfferPrice, int Days)
        {
            var comment = new CommentViewModel { Description = Description, OfferPrice = OfferPrice, Days= Days, UserId = User.Identity.GetUserId(), CustomViewModelId = CustomViewModelId };
            CustomViewModel customViewModel = db.Customs.Include(c => c.Comments).FirstOrDefault(c => c.Id == CustomViewModelId);
            if (customViewModel.Comments.Where(c => c.UserId == User.Identity.GetUserId()).Count() >= 1)
            {
                return null;
            }
            else
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return null;
            }
        }


        public FileResult GetFile(string path)
        {
            // Тип файла - content-type
            string file_type = "application/" + Path.GetExtension(path);
            string file_name = Path.GetFileName(path);

            return File(path,file_type,file_name);
        }

    
        // GET: Custom/Details/5
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // optimaize
            CustomViewModel customViewModel = await db.Customs.Include(c => c.Comments)
                                                              .Include(c => c.CategoryTask)
                                                              .Include(c => c.TypeTask)
                                                              .Include(c => c.User)
                                                              .FirstOrDefaultAsync(c => c.Id == id);

            if (customViewModel == null)
            {
                return HttpNotFound();
            }

            return View(customViewModel);
        }

        // GET: Custom/Create
        public ActionResult Create()
        {
            SelectList types = new SelectList(db.CustomTypes, "Id", "Name"); // выбор типа задачи
            ViewBag.Types = types;
            SelectList categories = new SelectList(db.TaskCategories, "Id", "Name"); // выбор типа задачи
            ViewBag.Categories = categories;
            return View();
        }

        // POST: Custom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,AttachFilePath,AttachFile,UserId,TypeTaskId,CategoryTaskId,EndingDate,MinPrice,MaxPrice")] CustomViewModel customViewModel)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileName(customViewModel.AttachFile.FileName);
                string path = Server.MapPath("~/Files/" + fileName);
                // сохраняем файл в папку Files в проекте
                customViewModel.AttachFile.SaveAs(path);
                customViewModel.AttachFilePath = "~/Files/" + fileName;
                customViewModel.Status = CustomStatus.Open; // открытая заявка
                customViewModel.UserId = User.Identity.GetUserId();
                db.Customs.Add(customViewModel);
                await db.SaveChangesAsync();
                SendMessage("Добавлен новый заказ");
                return RedirectToAction("Index");
            }
        
            return View(customViewModel);
        }

        // GET: Custom/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomViewModel customViewModel = await db.Customs.FindAsync(id);

            if (customViewModel == null)
            {
                return HttpNotFound();
            }

            if (customViewModel.UserId == User.Identity.GetUserId())
            {
                return View(customViewModel);
            }
            else { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
        }

        // POST: Custom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,AttachFilePath,AttachFile,UserId,TypeTaskId,CategoryTaskId,EndingDate,MinPrice,MaxPrice")] CustomViewModel customViewModel)
        { 
            if (ModelState.IsValid)
            {
                db.Entry(customViewModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
           
            return View(customViewModel);
        }

        // GET: Custom/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomViewModel customViewModel = await db.Customs.FindAsync(id);
            if (customViewModel == null)
            {
                return HttpNotFound();
            }

            if (customViewModel.UserId == User.Identity.GetUserId())
            {
                return View(customViewModel);
            }
            else { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
        }

        // POST: Custom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CustomViewModel customViewModel = await db.Customs.FindAsync(id);
            db.Customs.Remove(customViewModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

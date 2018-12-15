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

namespace HelpMe.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comment
        public async Task<ActionResult> Index()
        {
            var comments = db.Comments.Include(c => c.CustomViewModel).Include(u => u.User);
            return View(await comments.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentViewModel commentViewModel = await db.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (commentViewModel == null)
            {
                return HttpNotFound();
            }
            return View(commentViewModel);
        }

        
        // GET: Comment/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CustomViewModelId = id;
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,CustomViewModelId,UserId")] CommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                commentViewModel.UserId = User.Identity.GetUserId();
                db.Comments.Add(commentViewModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","Custom", new { id = commentViewModel.CustomViewModelId });
            }

            return View(commentViewModel);
        }

        // GET: Comment/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentViewModel commentViewModel = await db.Comments.FindAsync(id);
            if (commentViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomViewModelId = new SelectList(db.Customs, "Id", "Name", commentViewModel.CustomViewModelId);
            return View(commentViewModel);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,UserId,OfferPrice,Days,CustomViewModelId")] CommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentViewModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomViewModelId = new SelectList(db.Customs, "Id", "Name", commentViewModel.CustomViewModelId);
            return View(commentViewModel);
        }

        // GET: Comment/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentViewModel commentViewModel = await db.Comments.FindAsync(id);
            if (commentViewModel == null)
            {
                return HttpNotFound();
            }
            return View(commentViewModel);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CommentViewModel commentViewModel = await db.Comments.FindAsync(id);
            db.Comments.Remove(commentViewModel);
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

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

namespace HelpMe.Controllers
{
    public class TaskCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TaskCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.TaskCategories.ToListAsync());
        }

        // GET: TaskCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskCategory taskCategory = await db.TaskCategories.FindAsync(id);
            if (taskCategory == null)
            {
                return HttpNotFound();
            }
            return View(taskCategory);
        }

        // GET: TaskCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] TaskCategory taskCategory)
        {
            if (ModelState.IsValid)
            {
                db.TaskCategories.Add(taskCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(taskCategory);
        }

        // GET: TaskCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskCategory taskCategory = await db.TaskCategories.FindAsync(id);
            if (taskCategory == null)
            {
                return HttpNotFound();
            }
            return View(taskCategory);
        }

        // POST: TaskCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] TaskCategory taskCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taskCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(taskCategory);
        }

        // GET: TaskCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskCategory taskCategory = await db.TaskCategories.FindAsync(id);
            if (taskCategory == null)
            {
                return HttpNotFound();
            }
            return View(taskCategory);
        }

        // POST: TaskCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TaskCategory taskCategory = await db.TaskCategories.FindAsync(id);
            db.TaskCategories.Remove(taskCategory);
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

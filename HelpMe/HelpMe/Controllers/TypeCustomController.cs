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
using System.IO;

namespace HelpMe.Controllers
{
    public class TypeCustomController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TypeCustom
        public async Task<ActionResult> Index()
        {
            return View(await db.CustomTypes.ToListAsync());
        }

        // GET: TypeCustom/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeCustomViewModel typeCustomViewModel = await db.CustomTypes.FindAsync(id);
            if (typeCustomViewModel == null)
            {
                return HttpNotFound();
            }
            return View(typeCustomViewModel);
        }

        // GET: TypeCustom/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeCustom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create([Bind(Include = "Id,Name,ImagePath,ImageFile")] TypeCustomViewModel typeTaskViewModel)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileName(typeTaskViewModel.ImageFile.FileName);
                string path = Server.MapPath("~/Files/" + fileName);
                // сохраняем файл в папку Files в проекте
                typeTaskViewModel.ImageFile.SaveAs(path);
                typeTaskViewModel.ImagePath = "~/Files/" + fileName;
                db.CustomTypes.Add(typeTaskViewModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(typeTaskViewModel);
        }


        // GET: TypeCustom/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeCustomViewModel typeCustomViewModel = await db.CustomTypes.FindAsync(id);
            if (typeCustomViewModel == null)
            {
                return HttpNotFound();
            }
            return View(typeCustomViewModel);
        }

        // POST: TypeCustom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ImagePath")] TypeCustomViewModel typeCustomViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeCustomViewModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(typeCustomViewModel);
        }

        // GET: TypeCustom/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeCustomViewModel typeCustomViewModel = await db.CustomTypes.FindAsync(id);
            if (typeCustomViewModel == null)
            {
                return HttpNotFound();
            }
            return View(typeCustomViewModel);
        }

        // POST: TypeCustom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TypeCustomViewModel typeCustomViewModel = await db.CustomTypes.FindAsync(id);
            db.CustomTypes.Remove(typeCustomViewModel);
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

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
    public class WalletController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Wallet
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            Wallet wallet =  db.Wallets.Where(x => x.UserId == id).FirstOrDefault();

          
            return View(wallet);
        }

        // GET: Wallet/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = await db.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return HttpNotFound();
            }
            return View(wallet);
        }

        // GET: Wallet/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Description");
            return View();
        }

        // GET: Wallet/AddSumm
        public ActionResult AddSumm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSumm([Bind(Include = "Id,Summ")] Wallet wallet)
        {
            string id = User.Identity.GetUserId();
            Wallet walletBalance = db.Wallets.Where(x => x.UserId == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                wallet.UserId = id;
                if (walletBalance == null)
                {
                    db.Wallets.Add(wallet);
                }
                else
                {
                    walletBalance.Summ += wallet.Summ;
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(wallet);
        }

        // POST: Wallet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Summ,UserId")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                db.Wallets.Add(wallet);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Description", wallet.UserId);
            return View(wallet);
        }

        // GET: Wallet/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = await db.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Description", wallet.UserId);
            return View(wallet);
        }

        // POST: Wallet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Summ,UserId")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wallet).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Description", wallet.UserId);
            return View(wallet);
        }

        // GET: Wallet/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = await db.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return HttpNotFound();
            }
            return View(wallet);
        }

        // POST: Wallet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Wallet wallet = await db.Wallets.FindAsync(id);
            db.Wallets.Remove(wallet);
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

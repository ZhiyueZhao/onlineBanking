using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankOfBIT.Models;

namespace BankOfBIT.Controllers
{
    public class AccountStatusController : Controller
    {
        private BankOfBITContext db = new BankOfBITContext();

        //
        // GET: /AccountStatus/

        public ActionResult Index()
        {
            return View(db.AccountStatus.ToList());
        }

        //
        // GET: /AccountStatus/Details/5

        public ActionResult Details(int id = 0)
        {
            AccountStatus accountstatus = db.AccountStatus.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            return View(accountstatus);
        }

        //
        // GET: /AccountStatus/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AccountStatus/Create

        [HttpPost]
        public ActionResult Create(AccountStatus accountstatus)
        {
            if (ModelState.IsValid)
            {
                db.AccountStatus.Add(accountstatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accountstatus);
        }

        //
        // GET: /AccountStatus/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AccountStatus accountstatus = db.AccountStatus.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            return View(accountstatus);
        }

        //
        // POST: /AccountStatus/Edit/5

        [HttpPost]
        public ActionResult Edit(AccountStatus accountstatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountstatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accountstatus);
        }

        //
        // GET: /AccountStatus/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AccountStatus accountstatus = db.AccountStatus.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            return View(accountstatus);
        }

        //
        // POST: /AccountStatus/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountStatus accountstatus = db.AccountStatus.Find(id);
            db.AccountStatus.Remove(accountstatus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
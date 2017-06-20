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
    public class SavingsAccountController : Controller
    {
        private BankOfBITContext db = new BankOfBITContext();

        //
        // GET: /SavingsAccount/

        public ActionResult Index()
        {
            var bankaccounts = db.SavingsAccounts.Include(s => s.AccountStatus).Include(s => s.Client);
            return View(bankaccounts.ToList());
        }

        //
        // GET: /SavingsAccount/Details/5

        public ActionResult Details(int id = 0)
        {
            SavingsAccount savingsaccount = db.SavingsAccounts.Find(id);
            if (savingsaccount == null)
            {
                return HttpNotFound();
            }
            return View(savingsaccount);
        }

        //
        // GET: /SavingsAccount/Create

        public ActionResult Create()
        {
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description");
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName");
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName");
            return View();
        }

        //
        // POST: /SavingsAccount/Create

        [HttpPost]
        public ActionResult Create(SavingsAccount savingsaccount)
        {
            //calling the appropriate SetNext method 
            savingsaccount.SetNextAccountNumber();
            if (ModelState.IsValid)
            {
                db.BankAccounts.Add(savingsaccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", savingsaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", savingsaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", savingsaccount.ClientId);
            return View(savingsaccount);
        }

        //
        // GET: /SavingsAccount/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SavingsAccount savingsaccount = db.SavingsAccounts.Find(id);
            if (savingsaccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", savingsaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", savingsaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", savingsaccount.ClientId);
            return View(savingsaccount);
        }

        //
        // POST: /SavingsAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(SavingsAccount savingsaccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(savingsaccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", savingsaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", savingsaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", savingsaccount.ClientId);
            return View(savingsaccount);
        }

        //
        // GET: /SavingsAccount/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SavingsAccount savingsaccount = db.SavingsAccounts.Find(id);
            if (savingsaccount == null)
            {
                return HttpNotFound();
            }
            return View(savingsaccount);
        }

        //
        // POST: /SavingsAccount/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SavingsAccount savingsaccount = db.SavingsAccounts.Find(id);
            db.BankAccounts.Remove(savingsaccount);
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
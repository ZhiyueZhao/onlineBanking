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
    public class ChequingAccountController : Controller
    {
        private BankOfBITContext db = new BankOfBITContext();

        //
        // GET: /ChequingAccount/

        public ActionResult Index()
        {
            var bankaccounts = db.ChequingAccounts.Include(c => c.AccountStatus).Include(c => c.Client);
            return View(bankaccounts.ToList());
        }

        //
        // GET: /ChequingAccount/Details/5

        public ActionResult Details(int id = 0)
        {
            ChequingAccount chequingaccount = db.ChequingAccounts.Find(id);
            if (chequingaccount == null)
            {
                return HttpNotFound();
            }
            return View(chequingaccount);
        }

        //
        // GET: /ChequingAccount/Create

        public ActionResult Create()
        {
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description");
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName");
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName");
            return View();
        }

        //
        // POST: /ChequingAccount/Create

        [HttpPost]
        public ActionResult Create(ChequingAccount chequingaccount)
        {
            //calling the appropriate SetNext method 
            chequingaccount.SetNextAccountNumber();
            if (ModelState.IsValid)
            {
                db.BankAccounts.Add(chequingaccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", chequingaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", chequingaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", chequingaccount.ClientId);
            return View(chequingaccount);
        }

        //
        // GET: /ChequingAccount/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ChequingAccount chequingaccount = db.ChequingAccounts.Find(id);
            if (chequingaccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", chequingaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", chequingaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", chequingaccount.ClientId);
            return View(chequingaccount);
        }

        //
        // POST: /ChequingAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(ChequingAccount chequingaccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chequingaccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", chequingaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", chequingaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", chequingaccount.ClientId);
            return View(chequingaccount);
        }

        //
        // GET: /ChequingAccount/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ChequingAccount chequingaccount = db.ChequingAccounts.Find(id);
            if (chequingaccount == null)
            {
                return HttpNotFound();
            }
            return View(chequingaccount);
        }

        //
        // POST: /ChequingAccount/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ChequingAccount chequingaccount = db.ChequingAccounts.Find(id);
            db.BankAccounts.Remove(chequingaccount);
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
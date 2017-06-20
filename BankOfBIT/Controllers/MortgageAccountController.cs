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
    public class MortgageAccountController : Controller
    {
        private BankOfBITContext db = new BankOfBITContext();

        //
        // GET: /MortgageAccount/

        public ActionResult Index()
        {
            var bankaccounts = db.MortgageAccounts.Include(m => m.AccountStatus).Include(m => m.Client);
            return View(bankaccounts.ToList());
        }

        //
        // GET: /MortgageAccount/Details/5

        public ActionResult Details(int id = 0)
        {
            MortgageAccount mortgageaccount = db.MortgageAccounts.Find(id);
            if (mortgageaccount == null)
            {
                return HttpNotFound();
            }
            return View(mortgageaccount);
        }

        //
        // GET: /MortgageAccount/Create

        public ActionResult Create()
        {
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description");
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName");
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName");
            return View();
        }

        //
        // POST: /MortgageAccount/Create

        [HttpPost]
        public ActionResult Create(MortgageAccount mortgageaccount)
        {
            //calling the appropriate SetNext method 
            mortgageaccount.SetNextAccountNumber();
            if (ModelState.IsValid)
            {
                db.BankAccounts.Add(mortgageaccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", mortgageaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", mortgageaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", mortgageaccount.ClientId);
            return View(mortgageaccount);
        }

        //
        // GET: /MortgageAccount/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MortgageAccount mortgageaccount = db.MortgageAccounts.Find(id);
            if (mortgageaccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", mortgageaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", mortgageaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", mortgageaccount.ClientId);
            return View(mortgageaccount);
        }

        //
        // POST: /MortgageAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(MortgageAccount mortgageaccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mortgageaccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountStatusId = new SelectList(db.AccountStatus, "AccountStatusId", "Description", mortgageaccount.AccountStatusId);
            //display fullnam instead of firstname
            //ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FirstName", mortgageaccount.ClientId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", mortgageaccount.ClientId);
            return View(mortgageaccount);
        }

        //
        // GET: /MortgageAccount/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MortgageAccount mortgageaccount = db.MortgageAccounts.Find(id);
            if (mortgageaccount == null)
            {
                return HttpNotFound();
            }
            return View(mortgageaccount);
        }

        //
        // POST: /MortgageAccount/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MortgageAccount mortgageaccount = db.MortgageAccounts.Find(id);
            db.BankAccounts.Remove(mortgageaccount);
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
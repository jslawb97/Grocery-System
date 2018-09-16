using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataTransferObjects;
using GroceryWeb.Models;
using LogicLayer;

namespace GroceryWeb.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CustomerManager _usrMgr = new CustomerManager();

        // GET: Customers
        public ActionResult Index()
        {
            try
            {
                // Orders the items by the DepartmentID
                var pdList = _usrMgr.RetrieveCustomerList();
                return View(pdList);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = (_usrMgr.RetrieveCustomerList().Find(e => e.CustomerID == id));
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,PhoneNumber,FirstName,LastName,Email,Active")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _usrMgr.SaveNewCustomer(customer);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(ex);
                }
            }

            return View();
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = (_usrMgr.RetrieveCustomerList().Find(e => e.CustomerID == id));
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "CustomerID,PhoneNumber,FirstName,LastName,Email,Active")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Customer customerOld = (_usrMgr.RetrieveCustomerList().Find(e => e.CustomerID == id));

                    _usrMgr.EditCustomer(customer, customerOld);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = (_usrMgr.RetrieveCustomerList().Find(e => e.CustomerID == id));
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _usrMgr.DeactivateCustomer(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

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
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserManager _usrMgr = new UserManager();

        // GET: Employees
        public ActionResult Index()
        {
            try
            {
                // Orders the items by the DepartmentID
                var employees = _usrMgr.RetrieveEmployeeList();
                return View(employees);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (_usrMgr.RetrieveEmployeeList().Find(e => e.EmployeeID == id));
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,PhoneNumber,FirstName,LastName,Email,Active")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _usrMgr.SaveNewEmployee(employee);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(ex);
                }
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (_usrMgr.RetrieveEmployeeList().Find(e => e.EmployeeID == id));
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [Bind(Include = "EmployeeID,PhoneNumber,FirstName,LastName,Email,Active")] Employee employee)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    Employee employeeOld = (_usrMgr.RetrieveEmployeeList().Find(e => e.EmployeeID == id));

                    _usrMgr.EditEmployee(employee, employeeOld);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (_usrMgr.RetrieveEmployeeList().Find(e => e.EmployeeID == id));
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                _usrMgr.DeactivateEmployee(id);

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

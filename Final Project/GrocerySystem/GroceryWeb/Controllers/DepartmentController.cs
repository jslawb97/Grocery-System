using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataTransferObjects;

namespace GroceryWeb.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class DepartmentController : Controller
    {
        private DepartmentManager _dptMgr = new DepartmentManager();

        // GET: Department
        public ActionResult Index()
        {
            try
            {
                // Orders the items by the DepartmentID
                var dptList = _dptMgr.RetrieveDepartmentList().OrderBy(n => n.DepartmentID);
                return View(dptList);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            Department dprt = (_dptMgr.RetrieveDepartmentList().Find(e => e.DepartmentID == id));

            return View(dprt);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(DataTransferObjects.Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dptMgr.CreateNewDepartment(department);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(ex);
                }
            }
            else
            {
                return View();
            }
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            Department dpt = (_dptMgr.RetrieveDepartmentList().Find(e => e.DepartmentID == id));

            return View(dpt);
        }

        // POST: Department/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Department department = (_dptMgr.RetrieveDepartmentList().Find(e => e.DepartmentID == id));
                    Department departmentOld = (_dptMgr.RetrieveDepartmentList().Find(e => e.DepartmentID == id));

                    department.Name = Request.Form["Name"];
                    department.Description = Request.Form["Description"];

                    _dptMgr.EditDepartment(department, departmentOld);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            Department dpt = (_dptMgr.RetrieveDepartmentList().Find(e => e.DepartmentID == id));

            return View(dpt);
        }

        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _dptMgr.DeactivateDepartment(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

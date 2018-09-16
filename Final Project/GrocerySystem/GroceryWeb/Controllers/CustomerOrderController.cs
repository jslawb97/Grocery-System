using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataTransferObjects;

namespace GroceryWeb.Controllers
{
    public class CustomerOrderController : Controller
    {
        private ProductManager _itMgr = new ProductManager();

        // GET: CustomerOrder
        public ActionResult Index()
        {
            try
            {
                // Orders the items by the DepartmentID
                var ordList = _itMgr.RetrieveOrdersList().OrderBy(n => n.CustomerID);
                return View(ordList);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: CustomerOrder/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerOrder/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerOrder/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOrder/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerOrder/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOrder/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerOrder/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

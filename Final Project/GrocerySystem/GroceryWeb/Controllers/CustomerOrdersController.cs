using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataTransferObjects;

namespace GroceryWeb.Controllers
{
    [Authorize]
    public class CustomerOrdersController : Controller
    {
        CustomerOrderManager _ordMgr = new CustomerOrderManager();

        // GET: CustomerOrders
        public ActionResult Index()
        {
            try
            {
                // Orders the items by the DepartmentID
                var ordList = _ordMgr.RetrieveOrdersList().OrderBy(e => e.CustomerID);
                return View(ordList);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: CustomerOrders
        public ActionResult IndexEmployee()
        {
            try
            {
                // Orders the items by the DepartmentID
                var ordList = _ordMgr.RetrieveOrdersList().OrderBy(e => e.CustomerID);
                return View(ordList);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: CustomerOrders
        public ActionResult IndexCompleted()
        {
            try
            {
                // Orders the items by the DepartmentID
                var ordList = _ordMgr.RetrieveCompletedOrdersList().OrderBy(e => e.CustomerID);
                return View(ordList);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: CustomerOrders/Details/5
        public ActionResult Details(int id)
        {
            CustomerOrder order = (_ordMgr.RetrieveOrdersList().Find(e => e.OrderID == id));

            return View(order);
        }

        // GET: CustomerOrders/Create
        public ActionResult Create()
        {
            var customer = new List<DataTransferObjects.Customer>();
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100000,
                FirstName = "Irene"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100001,
                FirstName = "Ila"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100002,
                FirstName = "John"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100003,
                FirstName = "Josh"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100004,
                FirstName = "Selina"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100005,
                FirstName = "Marshall"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100006,
                FirstName = "Eric"
            });
            ViewBag.CustomerNames = customer;

            var depType = new List<DataTransferObjects.Department>();
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100000,
                Name = "Meat"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100001,
                Name = "Produce"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100002,
                Name = "Dairy"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100003,
                Name = "Frozen"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100004,
                Name = "Grocery"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100005,
                Name = "HBC"
            });

            ViewBag.DepartmentTypes = depType;
            return View();
        }

        // POST: CustomerOrders/Create
        [HttpPost]
        public ActionResult Create(DataTransferObjects.CustomerOrder order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ordMgr.SaveNewOrder(order);

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

        // GET: CustomerOrders/Edit/5
        public ActionResult Edit(int id)
        {
            CustomerOrder order = (_ordMgr.RetrieveOrdersList().Find(e => e.OrderID == id));

            var customer = new List<DataTransferObjects.Customer>();
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100000,
                FirstName = "Irene"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100001,
                FirstName = "Ila"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100002,
                FirstName = "John"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100003,
                FirstName = "Josh"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100004,
                FirstName = "Selina"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100005,
                FirstName = "Marshall"
            });
            customer.Add(new DataTransferObjects.Customer()
            {
                CustomerID = 100006,
                FirstName = "Eric"
            });
            ViewBag.CustomerNames = customer;

            var depType = new List<DataTransferObjects.Department>();
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100000,
                Name = "Meat"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100001,
                Name = "Produce"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100002,
                Name = "Dairy"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100003,
                Name = "Frozen"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100004,
                Name = "Grocery"
            });
            depType.Add(new DataTransferObjects.Department()
            {
                DepartmentID = 100005,
                Name = "HBC"
            });

            ViewBag.DepartmentTypes = depType;

            return View(order);
        }

        // POST: CustomerOrders/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CustomerOrder order = _ordMgr.RetrieveOrdersList().Find(e => e.OrderID == id);
                    CustomerOrder orderOld = _ordMgr.RetrieveOrdersList().Find(e => e.OrderID == id);

                    order.CustomerID = Int32.Parse(Request.Form["CustomerID"]);
                    order.DepartmentID = Int32.Parse(Request.Form["DepartmentID"]);
                    order.Description = Request.Form["Description"];
                    order.PickupDate = Request.Form["PickupDate"];

                    _ordMgr.EditOrder(order, orderOld);

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

        // GET: CustomerOrders/Delete/5
        public ActionResult Delete(int id)
        {
            CustomerOrder order = (_ordMgr.RetrieveOrdersList().Find(e => e.OrderID == id));

            return View(order);
        }

        // POST: CustomerOrders/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _ordMgr.DeleteOrder(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOrders/Delete/5
        public ActionResult Deactivate(int id)
        {
            CustomerOrder order = (_ordMgr.RetrieveOrdersList().Find(e => e.OrderID == id));

            return View(order);
        }

        // POST: CustomerOrders/Delete/5
        [HttpPost]
        public ActionResult Deactivate(int id, FormCollection collection)
        {
            try
            {
                _ordMgr.DeactivateOrder(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

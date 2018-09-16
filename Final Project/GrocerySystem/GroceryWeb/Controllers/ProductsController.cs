using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataTransferObjects;
using System.Net;

namespace GroceryWeb.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class ProductsController : Controller
    {
        private ProductManager _itMgr = new ProductManager();

        // GET: Products
        public ActionResult Index()
        {
            try
            {
                // Orders the items by the DepartmentID
                var pdList = _itMgr.RetrieveProductsList().OrderBy(n => n.DepartmentID);
                return View(pdList);
            }
            catch (Exception ex)
            {
                ViewBag.E = ex;
                return View("ErrorView");
            }
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            Products prd = (_itMgr.RetrieveProductsList().Find(e => e.UPC == id));

            ViewBag.Dep = prd.DepartmentID;

            return View(prd);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
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

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(DataTransferObjects.Products products)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _itMgr.SaveNewProduct(products);

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

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            Products prd = (_itMgr.RetrieveProductsList().Find(e => e.UPC == id));

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

            ViewBag.Dep = prd.DepartmentID;

            return View(prd);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Products product = _itMgr.RetrieveProductDetail(id);
                    Products productOld = _itMgr.RetrieveProductDetail(id);

                    product.UPC = Int32.Parse(Request.Form["UPC"]);
                    product.DepartmentID = Int32.Parse(Request.Form["DepartmentID"]);
                    product.Name = Request.Form["Name"];
                    product.Manufacturer = Request.Form["Manufacturer"];
                    product.OnHand = Int32.Parse(Request.Form["OnHand"]);
                    product.Cost = Decimal.Parse(Request.Form["Cost"]);

                    _itMgr.EditProduct(product, productOld);

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

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            Products prd = (_itMgr.RetrieveProductsList().Find(e => e.UPC == id));

            ViewBag.Dep = prd.DepartmentID;

            return View(prd);
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _itMgr.DeactivateProduct(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using DataAccessLayer;

namespace LogicLayer
{
    public class ProductManager
    {
        // Retrieve Products List
        public List<Products> RetrieveProductsList(bool active = true)
        {
            List<Products> productsList = null;

            try
            {
                productsList = ProductAccessor.RetrieveProductsByActive(active);
            }
            catch (Exception)
            {
                throw;
            }

            return productsList;
        }

        // Retrieve Frozen Product
        public List<Products> RetrieveProductsListByDepartment(int department)
        {
            List<Products> productsList = null;

            try
            {
                productsList = ProductAccessor.RetrieveProductsByDepartment(department);
            }
            catch (Exception)
            {
                
                throw;
            }

            return productsList;
        }

        // Retrieve Product Details
        public Products RetrieveProductDetail(int id)
        {
            Products products = null;

            try
            {
                products = ProductAccessor.RetrieveProductsByID(id);

            }
            catch (Exception)
            {
                throw;
            }

            return products;
        }

        // Edit Product
        public bool EditProduct(Products product, Products oldProduct)
        {
            var result = false;

            if (product.UPC == null || product.DepartmentID == null || product.Name == "" || product.OnHand == null || product.Manufacturer == "")
            {
                throw new ApplicationException("You must fill out everything.");
            }
            try
            {
                result = (0 != ProductAccessor.UpdateProduct(product, oldProduct));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Add New Product
        public bool SaveNewProduct(Products product)
        {
            var result = false;

            if (product.UPC == null || product.DepartmentID == null || product.Name == "" || product.OnHand == null || product.Manufacturer == "" || product.Cost == null)
            {
                throw new ApplicationException("You must fill out everything.");
            }
            try
            {
                result = (0 != ProductAccessor.AddNewProduct(product));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        // Deactivate Product
        public bool DeactivateProduct(int id)
        {
            bool result = false;

            try
            {
                result = (1 == ProductAccessor.DeactivateProduct(id));
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
    }
}

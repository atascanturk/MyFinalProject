using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductTest();
            //CategoryTest();

        }

        private static void CategoryTest()
        {
            CategoryManager categorymanager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categorymanager.GetAll().Data)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            ProductManager productManager = new ProductManager(new EfProductDal(),categoryManager);

            var result = productManager.GetProductDetails();
            if (result.Success) 
            {
                Console.WriteLine(result.Message+"\n--------------------");
                foreach (var item in result.Data)
                {
                    Console.WriteLine(item.ProductName + "-----" + item.CategoryName);
                    
                }

               
            }

            else 
            {
                Console.WriteLine(result.Message);
            }
           
        }
    }
}

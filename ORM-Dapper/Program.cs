using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace ORM_Dapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            //new sql access
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);


            //adding and reviewing departments
            var depRepo = new DapperDepartmentRepository(conn);
            var departments = depRepo.GetAllDepartments();

            Console.WriteLine("Give me name for a new Department.");
            depRepo.InsertDepartment(Console.ReadLine());

            Console.WriteLine($"Departments: ");
            foreach (var department in departments)
            {
                Console.WriteLine(department.Name);
            }


            //adding a new Product and reviewing products
            var productRepo = new DapperProductRepository(conn);
            var products = productRepo.GetAllProducts();

            Console.WriteLine("Give me name for a new Product.");
            var newProductName = Console.ReadLine();

            Console.WriteLine("Now give me a price for the product.");
            var inputValidater = double.TryParse(Console.ReadLine(), out var newProductPrice);

            while (inputValidater == false)
            {
                Console.WriteLine("Not a valid entry for product price.");

                inputValidater = double.TryParse(Console.ReadLine(), out newProductPrice);
            }

            Console.WriteLine("Now give me a category ID for the product.");
            inputValidater = int.TryParse(Console.ReadLine(), out var newProductCategoryID);

            while (inputValidater == false)
            {
                Console.WriteLine("Not a valid entry for product Category ID.");

                inputValidater = int.TryParse(Console.ReadLine(), out newProductCategoryID);
            }

            productRepo.CreateProduct(newProductName, newProductPrice, newProductCategoryID);

            Console.WriteLine($"Products:");
            foreach (var product in products)
            {
                Console.WriteLine($"Product Name {product.Name} : Product Price {product.Price} : Product CategoryID {product.CategoryID} " +
                $": ProductID {product.ProductID}");
            }
        }
    }
}

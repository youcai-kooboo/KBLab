using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppEF6.Infrastructure;
using ConsoleAppEF6.Models;

namespace ConsoleAppEF6
{
    class Program
    {
        static void Main(string[] args)
        {
            //Asychoronize();

        }

        #region Asychoronize
        private static void Asychoronize()
        {
            var task = PerformDatabaseOperations();

            Console.WriteLine();
            Console.WriteLine("Quote of the day");
            Console.WriteLine(" Don't worry about the world coming to an end today... ");
            Console.WriteLine(" It's already tomorrow in Australia.");

            task.Wait();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task PerformDatabaseOperations()
        {
            Product product = new Product();
            product.CreateTime = DateTime.Now;
            product.Name = "Pear";
            product.Price = 3.02M;
            product.Published = true;
            product.Storage = 1000;

            using (EF6Context db = new EF6Context())
            {
                db.Products.Add(product);
                Console.WriteLine("Calling SaveChanges...");
                await db.SaveChangesAsync();
                Console.WriteLine("SaveChanges completed.");


                Console.WriteLine("Executing query.");
                var products = await (from b in db.Products
                                      orderby b.Name
                                      select b).ToListAsync();

                // Write all blogs out to Console
                Console.WriteLine("Query completed with following results:");
                foreach (var p in products)
                {
                    Console.WriteLine(" - " + p.Name);
                }

            }
        } 
        #endregion
    }
}

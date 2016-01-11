using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;

namespace DynamicLinq
{
    class Program
    {
        /// <summary>
        ///Requires: Sql Server or SQL Server Express Edition : http://www.microsoft.com/en-us/download/details.aspx?id=22973
        ///Download Northwind:http://www.microsoft.com/en-us/download/details.aspx?id=23654
        ///Install Northwind: http://msdn.microsoft.com/en-us/library/vstudio/ff851969.aspx
        ///License:http://msdn.microsoft.com/en-US/vstudio/bb894665.aspx
        ///Original Source:http://weblogs.asp.net/scottgu/archive/2008/01/07/dynamic-linq-part-1-using-the-linq-dynamic-query-library.aspx
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            NorthwindDataContext northwind = new NorthwindDataContext();
            var result = northwind.Products
                                 .Where("CategoryID = 3 AND UnitPrice > 3")
                                 .OrderBy("SupplierID");
            Console.WriteLine("==================One=========================");
            Console.WriteLine("Count {0}",result.Count());
            Console.WriteLine("==============================================");
            result.ToList().ForEach(r => Console.WriteLine("Supplier: {0}", r.SupplierID));
            
            IQueryable<Product> queryableData = northwind.Products.AsQueryable<Product>();
            var externals = new Dictionary<string, object>();
            externals.Add("Products", queryableData);

            Console.WriteLine("===================Two========================");
            string query = "Products.Where(Product => (Product.CategoryID = 3 And Product.UnitPrice > 3)).OrderBy(Product=>(Product.SupplierID)).Select(Product=>(Product))";
            var expression = System.Linq.Dynamic.DynamicExpression.Parse(typeof(IQueryable<Product>), query, new[] { externals });
            result = queryableData.Provider.CreateQuery<Product>(expression);
            Console.WriteLine("Count {0}", result.Count());
            Console.WriteLine("=============================================");
            result.Select(r => r).ToList().ForEach(r => Console.WriteLine("Supplier: {0}", r.SupplierID));
            

            Console.WriteLine("==================Thee========================");
            query = "Products.Where(Product => (Product.CategoryID = 3 And Product.UnitPrice > 10)).OrderBy(Product=>(Product.SupplierID)).Take(10)";
            expression = System.Linq.Dynamic.DynamicExpression.Parse(typeof(IQueryable<Product>), query, new[] { externals });
            result = queryableData.Provider.CreateQuery<Product>(expression);
            Console.WriteLine("Count {0}", result.Count());
            Console.WriteLine("=============================================");
            result.ToList().ForEach(r => Console.WriteLine("Supplier: {0}", r.SupplierID));
            


            Console.WriteLine("==================Four========================");
            query = "Products.Where(Product => (Product.CategoryID = 3 And Product.UnitPrice > 10)).OrderBy(Product=>(Product.SupplierID)).Take(3).Union(Products.Where(Product => (Product.CategoryID = 4 And Product.UnitPrice > 3)).OrderBy(Product=>(Product.SupplierID)).Take(2))";
            expression = System.Linq.Dynamic.DynamicExpression.Parse(typeof(IQueryable<Product>), query, new[] { externals });
            result = queryableData.Provider.CreateQuery<Product>(expression);
            Console.WriteLine("Count {0}", result.Count());
            Console.WriteLine("=============================================");
            result.ToList().ForEach(r => Console.WriteLine("Supplier: {0} , Category: {1}", r.SupplierID, r.CategoryID));
            


            Console.WriteLine("==================Five========================");
            query = "Products.Where(Product => (Product.CategoryID = 3 And Product.UnitPrice > 10)).OrderBy(Product=>(Product.SupplierID)).Union(Products.Where(Product => (Product.CategoryID = 4 And Product.UnitPrice > 3)).OrderBy(Product=>(Product.SupplierID)))";
            expression = System.Linq.Dynamic.DynamicExpression.Parse(typeof(IQueryable<Product>), query, new[] { externals });
            result = queryableData.Provider.CreateQuery<Product>(expression);
            Console.WriteLine("Count {0}", result.Count());
            Console.WriteLine("=============================================");
            result.ToList().ForEach(r => Console.WriteLine("Supplier: {0} , Category: {1}", r.SupplierID, r.CategoryID));
            


            Console.WriteLine("===================Six========================");
            query = "Products.Where(Product => Product.CategoryID = 3 And Product.UnitPrice > 3).OrderByDescending(Product=>Product.SupplierID)";
            expression = System.Linq.Dynamic.DynamicExpression.Parse(typeof(IQueryable<Product>), query, new[] { externals });
            result = queryableData.Provider.CreateQuery<Product>(expression);
            Console.WriteLine("Count {0}", result.Count());
            Console.WriteLine("=============================================");
            result.Select(r => r).ToList().ForEach(r => Console.WriteLine("Supplier: {0}", r.SupplierID));

            Console.WriteLine("=============================================");

            Console.ReadLine();
        }
    }
}

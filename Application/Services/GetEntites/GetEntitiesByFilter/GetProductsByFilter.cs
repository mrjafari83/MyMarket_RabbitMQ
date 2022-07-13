using Application.Context;
using Common.Enums;
using Common.ViewModels.SearchViewModels;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.GetEntites.GetEntitiesByFilter
{
    internal class GetProductsByFilter
    {
        public static IQueryable<Product> GetProducts(IDataBaseContext db, ProducsSearchViewModel filters)
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Visits).Include(p => p.Inventories).Include(p => p.ProductInCarts).AsQueryable();

            if (!string.IsNullOrEmpty(filters.SearchKey))
                switch (filters.SearchBy)
                {
                    case Enums.PageFilterCategory.Name:
                        {
                            products = products.Where(p => p.Name.Contains(filters.SearchKey));
                            break;
                        }
                    case Enums.PageFilterCategory.Brand:
                        {
                            products = products.Where(p => p.Brand.Contains(filters.SearchKey));
                            break;
                        }
                    case Enums.PageFilterCategory.CategoryName:
                        {
                            products = products.Where(p => p.Category.Name.Contains(filters.SearchKey));
                            break;
                        }
                }

            switch (filters.OrderBy)
            {
                case Enums.ProductsFilter.LessViewed:
                    {
                        products = products.OrderBy(p => p.Visits.Count());
                        break;
                    }
                case Enums.ProductsFilter.MostViewed:
                    {
                        products = products.OrderByDescending(p => p.Visits.Count());
                        break;
                    }
                case Enums.ProductsFilter.Newest:
                    {
                        products = products.OrderByDescending(p => p.CreateDate);
                        break;
                    }
                case Enums.ProductsFilter.Oldest:
                    {
                        products = products.OrderBy(p => p.CreateDate);
                        break;
                    }
                case Enums.ProductsFilter.MostSelled:
                    {
                        products = products.OrderByDescending(p => p.ProductInCarts.Where(p => !p.IsShow)
                        .Sum(c => c.Count * c.ProductInventoryAndPrice.Price));
                        break;
                    }
                case Enums.ProductsFilter.LessSelled:
                    {
                        products = products.OrderBy(p => p.ProductInCarts.Where(p => !p.IsShow)
                        .Sum(c => c.Count * c.ProductInventoryAndPrice.Price));
                        break;
                    }
            }

            if (filters.StartPrice != 0)
                products = products.Where(p => p.Inventories.Where(c => c.Price > filters.StartPrice).Count() != 0);

            if (filters.EndPrice != 0)
                products = products.Where(p => p.Inventories.Where(c => c.Price < filters.EndPrice).Count() != 0);

            return products;
        }
    }
}

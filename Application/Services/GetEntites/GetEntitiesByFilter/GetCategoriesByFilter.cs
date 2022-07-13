using Application.Context;
using Common.ViewModels.SearchViewModels;
using Domain.Entities.BlogPages;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.GetEntites.GetEntitiesByFilter
{
    internal class GetCategoriesByFilter
    {
        public static IQueryable<Category<BlogPage>> GetBlogCategories(IDataBaseContext db, BlogCategoryViewModel searchModel)
        {
            var data = db.BlogPageCategories.AsQueryable();
            if (searchModel != null)
            {
                if (searchModel.SearchKey != null && searchModel.SearchKey != "")
                    data = data.Where(c => c.Name.Contains(searchModel.SearchKey));

                if (searchModel.ParentId != 0)
                    data = data.Where(c => c.Parent.Id == searchModel.ParentId);
            }

            return data;
        }

        public static IQueryable<Category<Product>> GetProductCategories(IDataBaseContext db, ProductCategoryViewModel searchModel)
        {
            var data = db.ProductCategories.AsQueryable();
            if (searchModel != null)
            {
                if (searchModel.SearchKey != null && searchModel.SearchKey != "")
                    data = data.Where(c => c.Name.Contains(searchModel.SearchKey));

                if (searchModel.ParentId != 0)
                    data = data.Where(c => c.Parent.Id == searchModel.ParentId);
            }

            return data;
        }
    }
}

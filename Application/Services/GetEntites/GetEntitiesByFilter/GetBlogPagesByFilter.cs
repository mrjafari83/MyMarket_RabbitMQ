using Application.Context;
using Common.Enums;
using Common.ViewModels.SearchViewModels;
using Domain.Entities.BlogPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.GetEntites.GetEntitiesByFilter
{
    internal class GetBlogPagesByFilter
    {
        public static IQueryable<BlogPage> GetBlogPages(IDataBaseContext db, BlogPageSearchViewModel searchModel)
        {
            var data = db.BlogPages.Include(b => b.Category).AsQueryable();

            data = searchModel.OrderBy switch
            {
                Enums.BlogPagesFilter.Newest => data.OrderBy(b => b.CreateDate),
                Enums.BlogPagesFilter.Oldest => data.OrderByDescending(b => b.CreateDate),
                Enums.BlogPagesFilter.MostViewed => data.OrderBy(b => b.Visits.Count()),
                Enums.BlogPagesFilter.LessViewed => data.OrderByDescending(b => b.Visits.Count()),
                _ => data.OrderBy(b => b.CreateDate),
            };

            if (!String.IsNullOrEmpty(searchModel.SearchKey))
                data = data.Where(b => b.Title.Contains(searchModel.SearchKey) || b.Category.Name.Contains(searchModel.SearchKey));

            if (searchModel.CategoryId != 0)
                data = data.Where(b => b.CategoryId == searchModel.CategoryId);

            return data;
        }
    }
}

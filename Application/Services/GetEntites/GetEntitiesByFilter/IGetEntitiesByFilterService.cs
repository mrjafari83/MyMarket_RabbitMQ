using Application.Context;
using Application.Services.Admin.Options.Queries.GetEntitiesByFilter.Dtos;
using Common.Dto;
using Common.Enums;
using Common.Utilities;
using Common.ViewModels.SearchViewModels;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Common.ViewModels.ExcelViewModels;
using Microsoft.Extensions.Logging;

namespace Application.Services.GetEntites.GetEntitiesByFilter
{
    public interface IGetEntitiesByFilterService
    {
        ResultDto<IEnumerable<object>> Execute(int filterId);
    }

    public class GetEntitiesByFilterService : IGetEntitiesByFilterService
    {
        private readonly IDataBaseContext db;
        private readonly SaveLogInFile _saveLogInFile;
        private readonly IGetUserByFilter _getUserByFilter;
        public GetEntitiesByFilterService(IDataBaseContext context, SaveLogInFile saveLogInFile, IGetUserByFilter getUserByFilter)
        {
            db = context;
            _saveLogInFile = saveLogInFile;
            _getUserByFilter = getUserByFilter;
        }

        public ResultDto<IEnumerable<object>> Execute(int filterId)
        {
            try
            {
                var filter = db.SearchFilter.Find(filterId);
                IEnumerable<object> result = filter.SearchType switch
                {
                    //Products
                    Domain.Entities.Option.SearchItemType.Product => GetProductsByFilter.GetProducts(db , JsonConvertor<ProducsSearchViewModel>.LoadFromJsonString(filter.FilterJson))
                    .Select(p => new GetAllProductDetailsDto
                    {
                        Name = p.Name,
                        Brand = p.Brand,
                        CategoryName = p.Category.Name,
                        VisitNumber = p.Visits.Count(),
                        SellsCount = p.ProductInCarts.Where(p => !p.IsShow).Count()
                    }).AsNoTracking().ToList(),

                    //Blog Category
                    Domain.Entities.Option.SearchItemType.BlogCategory => GetCategoriesByFilter.GetBlogCategories(db,JsonConvertor<BlogCategoryViewModel>.LoadFromJsonString(filter.FilterJson))
                    .Select(c=> new ExcelCategoryViewModel
                    {
                        Name=c.Name,
                        ParentName = c.Parent.Name ?? "ندارد",
                        ChildrenCount = c.Children.Count(),
                    }),

                    //Product Category
                    Domain.Entities.Option.SearchItemType.ProductCategory => GetCategoriesByFilter.GetProductCategories(db,JsonConvertor<ProductCategoryViewModel>.LoadFromJsonString(filter.FilterJson))
                    .Select(c => new ExcelCategoryViewModel
                    {
                        Name = c.Name,
                        ParentName = c.Parent.Name ?? "ندارد",
                        ChildrenCount = c.Children.Count(),
                    }),

                    //Blog Pages
                    Domain.Entities.Option.SearchItemType.BlogPages=> GetBlogPagesByFilter.GetBlogPages(db,JsonConvertor<BlogPageSearchViewModel>.LoadFromJsonString(filter.FilterJson))
                    .Include(b=> b.Category).Select(b=> new ExcelBlogPagesViewModel
                    {
                        Title = b.Title,
                        ShortDescription = b.ShortDescription,
                        CategoryName = b.Category.Name,
                        VisitNumber = b.Visits.Count(),
                        CreateDate = b.CreateDate.ToShamsi()
                    }),

                    //Messages
                    Domain.Entities.Option.SearchItemType.Message=>GetMessagesByFilter.GetMessages(db,JsonConvertor<MessageSearchViewModel>.LoadFromJsonString(filter?.FilterJson))
                    .Select(m=> new ExcelMessageViewModel
                    {
                        Name =m.Name,
                        Email = m.Email,
                        Website = m.Website,
                        Text = m.Message
                    }),

                    //Users
                    Domain.Entities.Option.SearchItemType.User => _getUserByFilter.GetUsers(JsonConvertor<UserSearchVIewModel>.LoadFromJsonString(filter?.FilterJson))
                    .Select(u=> new ExcelUserViewModel
                    {
                        UserName = u.UserName ?? "ندارد",
                        Email = u.Email ?? "ندارد",
                        Name = u.Name ?? "ندارد",
                        Family = u.Family ?? "ندارد",
                        PhoneNumber = u.PhoneNumber ?? "ندارد",
                    })
                };

                return new ResultDto<IEnumerable<object>>
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _saveLogInFile.Log(LogLevel.Error, ex.Message, "Get Entities By search in Application");
                return new ResultDto<IEnumerable<object>>
                {
                    Data = new List<object>(),
                    IsSuccess = false,
                    Message = "خطایی رخ داده است."
                };
            }
        }
    }
}

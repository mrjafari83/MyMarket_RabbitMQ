using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Application.Context;

namespace Application.Services.Excel.Commands.DeleteAll
{
    public interface IDeleteAllService
    {
        ResultDto Execute();
    }

    public class DeleteAllService : IDeleteAllService
    {
        private readonly IDataBaseContext db;
        public DeleteAllService(IDataBaseContext context)
        {
            db = context;
        }

        public ResultDto Execute()
        {
            var entities = db.ExcelStatuses.ToList();
            foreach(var entity in entities)
            {
                entity.IsRemoved = true;
                entity.RemoveTime = DateTime.Now;
            }
            db.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
            };
        }
    }
}

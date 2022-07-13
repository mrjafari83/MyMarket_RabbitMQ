using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Excel.Commands.CreateExcelKey;
using Application.Services.Excel.Commands.DeleteAll;
using Application.Services.Excel.Commands.SetFileName;
using Application.Services.Excel.Commands.UpdateStatus;
using Application.Services.Excel.Queries.GetAllExcels;
using Common.Enums;

namespace Application.Facade.Interfaces
{
    public interface IExcelFacade
    {
        ICreateExcelKeyService CreateExcelKey { get; }
        IUpdateStatusService UpdateStatus { get; }
        ISetFileNameService SetFileName { get; }
        IDeleteAllService DeleteAll { get; }
        IGetAllExcelsService GetAllExcels { get; }
    }
}

using Application.Context;
using Application.Facade.Interfaces;
using Application.Services.Excel.Commands.CreateExcelKey;
using Application.Services.Excel.Commands.DeleteAll;
using Application.Services.Excel.Commands.SetFileName;
using Application.Services.Excel.Commands.UpdateStatus;
using Application.Services.Excel.Queries.GetAllExcels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Facade.Classes
{
    public class ExcelFacade : IExcelFacade
    {
        private readonly IDataBaseContext db;
        public ExcelFacade(IDataBaseContext context)
        {
            db = context;
        }

        private CreateExcelKeyService _createExcelKeyService;
        public ICreateExcelKeyService CreateExcelKey
        {
            get => _createExcelKeyService ?? new CreateExcelKeyService(db);
        }

        private UpdateStatusService _updateStatusService;
        public IUpdateStatusService UpdateStatus
        {
            get => _updateStatusService ?? new UpdateStatusService(db);
        }

        private SetFileNameService _setFileNameService;
        public ISetFileNameService SetFileName
        {
            get => _setFileNameService ?? new SetFileNameService(db);
        }

        private DeleteAllService _deleteAllService;
        public IDeleteAllService DeleteAll
        {
            get => _deleteAllService ?? new DeleteAllService(db);
        }

        private GetAllExcelsService _getAllExcelsService;
        public IGetAllExcelsService GetAllExcels
        {
            get => _getAllExcelsService ?? new GetAllExcelsService(db);
        }
    }
}

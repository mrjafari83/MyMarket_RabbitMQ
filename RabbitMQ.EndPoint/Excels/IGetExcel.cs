using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.EndPoint.Excels
{
    internal interface IGetExcel
    {
        string GetExcelFile<Type>(List<Type> source, string address, string prefixFileName);
    }
}

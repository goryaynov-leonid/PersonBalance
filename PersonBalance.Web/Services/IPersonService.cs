using PersonBalance.Web.Models;
using PersonBalance.Web.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonBalance.Web.Services
{
    public interface IPersonService
    {
        Task<Guid> CreateNewPerson(PersonDto dto);
        Task<decimal> CheckBalance(Guid id);
        Task ChangeBalance(Guid id, decimal balance);
        Task<byte[]> GetPersonVersion(Guid id);
    }
}

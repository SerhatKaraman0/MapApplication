using System;
using MapApplication.Data;
using MapApplication.Models;

namespace MapApplication.Interfaces
{
    public interface IDatabaseOperationsService
    {
        Task<Response> FindByName(string name);
        Task<Response> FindById(int id);
        Task<Response> Count();
        Task<Response> Update(int id, PointDb Point);
        Task<Response> UpdateByName(string name, PointDb Point);
        Task<Response> Delete(int id);
        Task<Response> DeleteByName(string name);
        Task<Response> DeleteAll();
        Task<Response> Create(PointDb Point);
        Task<Response> SelectAll();
    }
}


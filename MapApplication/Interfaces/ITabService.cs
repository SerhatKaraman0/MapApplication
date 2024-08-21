using System;
using MapApplication.Data;
using MapApplication.Models;

namespace MapApplication.Interfaces
{
	public interface ITabService
	{
        List<TabsDb> GetAllTabs();
        Task<List<TabsDb>> GetTabsByUserId(int userId);
        Task<List<TabsDb>> SaveTab(int userId, TabsDb tab);
        Task<List<TabsDb>> RemoveTabById(int userId, int id);
        Task<List<TabsDb>> UpdateTabById(int userId, int id, TabsDb updatedTab);
    }
}


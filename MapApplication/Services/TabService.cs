using System;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapApplication.Services
{
    public class TabService : ITabService
    {
        private readonly AppDbContext _context;
        private readonly IUserResponseService _userResponseService;

        public TabService(AppDbContext context, IUserResponseService userResponseService)
        {
            _context = context;
            _userResponseService = userResponseService;
        }

        public List<TabsDb> GetAllTabs()
        {
            try
            {
                var allTabs = _context.Tabs.ToList();
                return allTabs;
            }
            catch (Exception ex)
            {
                // Log the exception
                return new List<TabsDb>();
            }
        }

        public async Task<List<TabsDb>> GetTabsByUserId(int userId)
        {
            try
            {
                var tabs = await _context.Tabs
                                         .Where(p => p.OwnerId == userId)
                                         .ToListAsync();

                return tabs.Any() ? tabs : new List<TabsDb>();
            }
            catch (Exception ex)
            {
                // Log the exception
                return new List<TabsDb>();
            }
        }

        public async Task<List<TabsDb>> RemoveTabById(int userId, int tabId)
        {
            try
            {
                var tab = await _context.Tabs
                                        .FirstOrDefaultAsync(p => p.OwnerId == userId && p.TabId == tabId);

                if (tab == null)
                {
                return new List<TabsDb>();

                }

                _context.Tabs.Remove(tab);
                await _context.SaveChangesAsync();

                return new List<TabsDb> { tab };
            }
            catch (Exception ex)
            {
                return new List<TabsDb>();

            }
        }

        public async Task<List<TabsDb>> SaveTab(int userId, TabsDb tab)
        {
            try
            {
                tab.OwnerId = userId;
                tab.createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                await _context.Tabs.AddAsync(tab);
                await _context.SaveChangesAsync();

                return new List<TabsDb> { tab };
            }
            catch (Exception ex)
            {
                return new List<TabsDb>();
            }
        }

        public async Task<List<TabsDb>> UpdateTabById(int userId, int tabId, TabsDb updatedTab)
        {
            try
            {
                var tab = await _context.Tabs.FirstOrDefaultAsync(t => t.OwnerId == userId && t.TabId == tabId);
                if (tab == null)
                {
                    return new List<TabsDb>();
                }

                tab.TabName = updatedTab.TabName;
                tab.TabColor = updatedTab.TabColor;
                tab.createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                _context.Tabs.Update(tab);
                await _context.SaveChangesAsync();

                return new List<TabsDb> { tab };
            }
            catch (Exception ex)
            {
                return new List<TabsDb>();
            }
        }
    }
}

using System;
using MapApplication.Data;
using MapApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TabsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPointService _pointService;
        private readonly IWktService _wktService;
        private readonly IUserService _userService;
        private readonly ITabService _tabService;

        public TabsController(IUnitOfWork unitOfWork, IPointService pointService, IWktService wktService, ITabService tabService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _pointService = pointService;
            _wktService = wktService;
            _userService = userService;
            _tabService = tabService;

        }

        [HttpGet("tabs/all")]
        public List<TabsDb> GetAllTabs()
        {
            var response = _tabService.GetAllTabs();
            return response;
        }

        [HttpGet("{ownerId}/tabs")]
        public Task<List<TabsDb>> GetTabsOfUser([FromRoute] int ownerId)
        {
            var response = _tabService.GetTabsByUserId(ownerId);
            return response;
        }

        [HttpGet("{ownerId}/tabs/save")]
        public Task<List<TabsDb>> SaveTabOfUser([FromRoute] int ownerId, [FromBody] TabsDb tab)
        {
            var response = _tabService.SaveTab(ownerId, tab);
            return response;
        }

        [HttpDelete("{ownerId}/tabs/remove/{tabId}")]
        public Task<List<TabsDb>> RemoveUsersTabById([FromRoute] int ownerId, [FromRoute] int tabId)
        {
            var response = _tabService.RemoveTabById(ownerId, tabId);
            return response;
        }

        [HttpPut("{ownerId}/tabs/update/{tabId}")]
        public Task<List<TabsDb>> UpdateTabOfUserById([FromRoute] int ownerId, [FromRoute] int tabId, [FromBody] TabsDb updatedTab)
        {
            var response = _tabService.UpdateTabById(ownerId, tabId, updatedTab);
            return response;
        }
    }
}

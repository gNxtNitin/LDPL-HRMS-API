using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuManagementLib;
using MenuManagementLib.Models;
using AuthLibrary.Models;
using Services.Interfaces;
using ModelsLibrary.Models;

namespace Services.Implementation
{
    public class MenuService: IMenuService
    {
        private readonly IMenuManagementService _menuManagementService;
        public MenuService(IMenuManagementService menuManagementService)
        {
            _menuManagementService = menuManagementService;
        }
        public async Task<ResponseModel> GetAllMenu()
        {

            ResponseModel response = await _menuManagementService.GetMenuMaster();
            return response;
        }
        public async Task<ResponseModel> GetMenuById(int? userId, int? menuId)
        {

            ResponseModel response = await _menuManagementService.GetMenuByUserIdOrMenuId(userId, menuId);
            return response;
        }
        public async Task<ResponseModel> CreateMenu(MenuMaster mn)
        {
            
            ResponseModel response = await _menuManagementService.CreateNewMenu(mn);
            return response;
        }
        public async Task<ResponseModel> UpdateMenu(MenuMaster mn)
        {

            ResponseModel response = await _menuManagementService.UpdateMenu(mn);
            return response;
        }
        public async Task<ResponseModel> DeleteMenu(int menuId)
        {
            ResponseModel response = await _menuManagementService.DeleteMenu(menuId);
            return response;
        }

        public async Task<ResponseModel> GetMenuFeatureMaster()
        {
            ResponseModel response = await _menuManagementService.GetMenuFeatureMaster();
            return response;
        }

        public async Task<ResponseModel> GetMenuFeatures(int userId, int menuId)
        {
            ResponseModel response = await _menuManagementService.GetMenuFeaturesByUserIdMenuId(userId, menuId);
            return response;
        }

        public async Task<ResponseModel> AddMenuFeatureMasterRec(MenuFeatureMaster menuFeatureMaster)
        {
            ResponseModel response = await _menuManagementService.AddMenuFeatureMasterRecord(menuFeatureMaster);
            return response;
        }

        public async Task<ResponseModel> UpdateMenuFeatureMasterRec(MenuFeatureMaster menuFeatureMaster)
        {
            ResponseModel response = await _menuManagementService.UpdateMenuFeatureMasterRecord(menuFeatureMaster);
            return response;
        }

        public async Task<ResponseModel> DeleteMenuFeatureMasterRec(int featureId)
        {
            ResponseModel response = await _menuManagementService.DeleteMenuFeatureMasterRecord(featureId);
            return response;
        }

        public async Task<ResponseModel> AddMenuFeatureRec(MenuFeatures menuFeatures)
        {
            ResponseModel response = await _menuManagementService.AddMenuFeature(menuFeatures);
            return response;
        }
        public async Task<ResponseModel> DeleteMenuFeatureRec(MenuFeatures menuFeatures)
        {
            ResponseModel response = await _menuManagementService.DeleteMenuFeature(menuFeatures);
            return response;
        }

        public async Task<ResponseModel> GetMenusByRole(int roleId)
        {
            ResponseModel response = await _menuManagementService.GetRoleMenuAccess(roleId);
            return response;
        }

        public async Task<ResponseModel> AddOrUpdateRoleMenuAccess(int roleId, int menuId)
        {
            ResponseModel response = await _menuManagementService.AddOrUpdateRoleMenuAccess(roleId, menuId);
            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuManagementLib.Models;
using AuthLibrary.Models;
using ModelsLibrary.Models;

namespace Services.Interfaces
{
    public interface IMenuService
    {
        public Task<ResponseModel> GetAllMenu();
        public Task<ResponseModel> GetMenuById(int? userId, int? menuId);
        public Task<ResponseModel> CreateMenu(MenuMaster mn);
        public Task<ResponseModel> UpdateMenu(MenuMaster mn);
        public Task<ResponseModel> DeleteMenu(int menuId);

        public Task<ResponseModel> GetMenuFeatureMaster();
        public Task<ResponseModel> GetMenuFeatures(int userId, int menuId);

        public Task<ResponseModel> AddMenuFeatureMasterRec(MenuFeatureMaster menuFeatureMaster);
        public Task<ResponseModel> UpdateMenuFeatureMasterRec(MenuFeatureMaster menuFeatureMaster);
        public Task<ResponseModel> DeleteMenuFeatureMasterRec(int featureId);

        public Task<ResponseModel> AddMenuFeatureRec(MenuFeatures menuFeatures);
        public Task<ResponseModel> DeleteMenuFeatureRec(MenuFeatures menuFeatures);

        public Task<ResponseModel> GetMenusByRole(int roleId);
        public Task<ResponseModel> AddOrUpdateRoleMenuAccess(int roleId, int menuId);
    }
}

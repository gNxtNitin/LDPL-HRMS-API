using MenuManagementLib.Models;
using AuthLibrary.Models;
using ModelsLibrary.Models;

namespace MenuManagementLib
{
    public interface IMenuManagementService
    {
        public Task<ResponseModel> GetMenuMaster();
        public Task<ResponseModel> GetMenuByUserIdOrMenuId(int? UserId, int? menuId);
        public Task<ResponseModel> CreateNewMenu(MenuMaster menu);
        public Task<ResponseModel> UpdateMenu(MenuMaster menu);
        public Task<ResponseModel> DeleteMenu(int menuId);

        public Task<ResponseModel> GetMenuFeatureMaster();
        public Task<ResponseModel> GetMenuFeaturesByUserIdMenuId(int userId, int menuId);

        public Task<ResponseModel> AddMenuFeatureMasterRecord(MenuFeatureMaster menuFeatureMaster);
        public Task<ResponseModel> DeleteMenuFeatureMasterRecord(int featureId);
        public Task<ResponseModel> UpdateMenuFeatureMasterRecord(MenuFeatureMaster menuFeatureMaster);

        public Task<ResponseModel> AddMenuFeature(MenuFeatures menuFeatures);
        public Task<ResponseModel> DeleteMenuFeature(MenuFeatures menuFeatures);
        public Task<ResponseModel> GetRoleMenuAccess(int roleId);

        public Task<ResponseModel> AddOrUpdateRoleMenuAccess(int roleId, int menuId);
    }

}

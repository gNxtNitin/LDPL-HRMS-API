using AuthLibrary.Models;
using ErrorAndExceptionHandling.Library;
using MenuManagementLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;
using Services.Implementation;
using Services.Interfaces;


namespace LDPLWEBAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuManagementController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IErrorLoggingService _errorLoggingService;
        public MenuManagementController(IMenuService menuService, IErrorLoggingService errorLoggingService)
        {
            _menuService = menuService;
            _errorLoggingService = errorLoggingService;
        }
        [HttpGet("GetAllMenu")]
        public async Task<IActionResult> GetAllMenu()
        {
            try
            {

                ResponseModel response = await _menuService.GetAllMenu();
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }


        [HttpGet("GetMenuById")]
        public async Task<IActionResult> GetMenuById(int? userId = 0, int? menuId = 0)
        {
            try
            {

                ResponseModel response = await _menuService.GetMenuById(userId, menuId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPost("CreateMenu")]
        public async Task<IActionResult> CreateMenu([FromBody] MenuMaster mn)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _menuService.CreateMenu(mn);
                return Ok(res);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

        [HttpPost("UpdateMenu")]
        public async Task<IActionResult> UpdateMenu([FromBody] MenuMaster mn)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _menuService.UpdateMenu(mn);
                return Ok(res);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

        [HttpGet("DeleteMenu")]
        public async Task<IActionResult> UpdateMenu(string id)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _menuService.DeleteMenu(int.Parse(id));
                return Ok(res);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

        [HttpGet("GetMenuFeatureMaster")]
        public async Task<IActionResult> GetMenuFeatureMaster()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.GetMenuFeatureMaster();
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpGet("GetMenuFeatures")]
        public async Task<IActionResult> GetMenuFeatures(int userId, int menuId)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.GetMenuFeatures(userId, menuId);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }


        [HttpPost("CreateMenuFeatureMaster")]
        public async Task<IActionResult> CreateMenuFeatureMaster([FromBody] MenuFeatureMaster menuFeatureMaster)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.AddMenuFeatureMasterRec(menuFeatureMaster);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPost("UpdateMenuFeatureMaster")]
        public async Task<IActionResult> UpdateMenuFeatureMaster([FromBody] MenuFeatureMaster menuFeatureMaster)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.UpdateMenuFeatureMasterRec(menuFeatureMaster);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpGet("UpdateMenuFeatureMaster")]
        public async Task<IActionResult> DeleteMenuFeatureMaster(int featureId)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.DeleteMenuFeatureMasterRec(featureId);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPost("CreateMenuFeature")]
        public async Task<IActionResult> CreateMenuFeature([FromBody] MenuFeatures menuFeatures)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.AddMenuFeatureRec(menuFeatures);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPost("DeleteMenuFeature")]
        public async Task<IActionResult> DeleteMenuFeature([FromBody] MenuFeatures menuFeatures)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.DeleteMenuFeatureRec(menuFeatures);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }


        [HttpGet("GetMenuByRole")]
        public async Task<IActionResult> GetMenuByRole(int roleId)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.GetMenusByRole(roleId);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPost("AssignRoleMenuAccess")]
        public async Task<IActionResult> AssignRoleMenuAccess([FromBody] RoleMenuAssignment roleMenuAssignment)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _menuService.AddOrUpdateRoleMenuAccess(roleMenuAssignment.roleId, roleMenuAssignment.menuId);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }
    }
}

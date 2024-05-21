using System.Text.Json.Nodes;
using ccytet.Server.Services;
using ccytet.Server.ViewModels.Req.Noticias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workcube.Libraries;

namespace ccytet.Server.Controllers
{
    [Route("api/noticias")]
    public class NoticiasController : ControllerBase
    {
        private readonly NoticiasService _noticiasService;

        public NoticiasController(NoticiasService noticiasService)
        {
            _noticiasService = noticiasService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("create")]
        public async Task<ActionResult<dynamic>> Create([FromBody] CreateNoticiaReq.Root data)
        {
            JsonReturn objReturn = new();
            try
            {
                await _noticiasService.Store(User, data);
                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch(AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch(Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("DataSource")]
        public async Task<ActionResult<dynamic>> DataSource([FromBody] JsonObject data)
        {
            JsonReturn objReturn = new();
            try
            {
                objReturn.Result = await _noticiasService.DataSource(Globals.JsonData(data));
                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch(AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch (Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("update")]
        public async Task<ActionResult<dynamic>> Update([FromBody] ActualizarNoticiaReq.Root data)
        {
            JsonReturn objReturn = new();
            try
            {
                await _noticiasService.Update(data, User);
                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch(AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch(Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("ToggleVisibility")]
        public async Task<ActionResult<dynamic>> ToggleVisibility([FromBody] JsonObject data)
        {
            JsonReturn objReturn = new();
            try
            {
                await _noticiasService.ToggleVisibility(User, Globals.JsonData(data));
                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch(AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch(Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("watch")]
        public async Task<ActionResult<dynamic>> Watch([FromBody] JsonObject data)
        {
            JsonReturn objReturn = new();
            try
            {
                objReturn.Result = await _noticiasService.Watch(Globals.JsonData(data));
                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch(AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch (Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }  
    }
}
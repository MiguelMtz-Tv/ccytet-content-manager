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
    }
}
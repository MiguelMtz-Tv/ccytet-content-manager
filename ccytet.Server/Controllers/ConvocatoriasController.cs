using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ccytet.Server.Services;
using ccytet.Server.ViewModels.Req.Convocatorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Workcube.Libraries;

namespace ccytet.Server.Controllers
{
    [Route("api/convocatorias")]
    public class ConvocatoriasController : ControllerBase
    {
        private readonly ConvocatoriasService _convocatoriasService;

        public ConvocatoriasController(ConvocatoriasService convocatoriasService)
        {
            _convocatoriasService = convocatoriasService;
        }

        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<dynamic>> Create([FromBody] CrearConvocatoriaReq.Root data)
        {
            JsonReturn objReturn = new();
            try
            {
                await _convocatoriasService.Store(User, data);
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
                objReturn.Result = await _convocatoriasService.DataSource(Globals.JsonData(data));
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
        [HttpPost("Watch")]
        public async Task<ActionResult<dynamic>> Watch([FromBody] JsonObject data)
        {
            JsonReturn objReturn = new();
            try
            {
                objReturn.Result = await _convocatoriasService.Watch(Globals.JsonData(data));
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
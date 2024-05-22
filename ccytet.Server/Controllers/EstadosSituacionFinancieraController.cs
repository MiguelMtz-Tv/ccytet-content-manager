using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ccytet.Server.Services;
using ccytet.Server.ViewModels.Req.ESF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workcube.Libraries;

namespace ccytet.Server.Controllers
{
    [Route("api/estadossituacionfinanciera")]
    public class EstadosSituacionFinancieraController : ControllerBase
    {
        private readonly EstadosSituacionFinancierosService _esfService;
        public EstadosSituacionFinancieraController(EstadosSituacionFinancierosService esfService)
        {
            _esfService = esfService;
        }

        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<dynamic>> Create([FromBody] ReqCreateESF data)
        {
            JsonReturn objReturn = new();
            try
            {
                await _esfService.Store(User, data);
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

        [HttpPost("delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<dynamic>> Delete([FromBody] JsonObject data)
        {
            JsonReturn objReturn = new();
            try
            {
                await _esfService.Delete(User, Globals.JsonData(data));
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
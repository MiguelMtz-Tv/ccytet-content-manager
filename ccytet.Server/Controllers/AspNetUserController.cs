using System.Text.Json.Nodes;
using ccytet.Server.Models;
using ccytet.Server.Services;
using ccytet.Server.Tokens;
using ccytet.Server.ViewModels.Req.Auth;
using ccytet.Server.ViewModels.Res;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Workcube.Libraries;

namespace ccytet.Server.Controllers
{
    [Route("api/aspNetUser")]
    public class AspNetUserController : ControllerBase
    {
        private readonly AspNetUserService _aspNetUserService;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly JwtHandler _jwtTokenHandler;

        public AspNetUserController(AspNetUserService aspNetUserService, SignInManager<AspNetUser> signInManager, JwtHandler jwtTokenHandler)
        {
            _aspNetUserService = aspNetUserService;
            _signInManager = signInManager;
            _jwtTokenHandler =jwtTokenHandler;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<dynamic>> SignIn([FromBody] LoginReq data)
        {
            try
            {
                string matricula = data.matricula;
                string password = data.password;

                AspNetUser user = await _aspNetUserService.FindLogin(matricula);

                if (user == null) { return BadRequest("Usuario no encontrado"); }

                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

                if (result.Succeeded)
                {
                    return _jwtTokenHandler.GenerateToken(user);
                }
                else
                {
                    return BadRequest("Usuario o contraseña incorrecta");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace + ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task Create([FromBody] JsonObject data)
        {
            await _aspNetUserService.Create(Globals.JsonData(data));
        }

        [HttpGet("status")]
        public ActionResult Status()
        {
            return Ok("Everything is ok!");
        }

    }
}
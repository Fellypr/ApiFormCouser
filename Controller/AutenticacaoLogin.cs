using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoLogin : ControllerBase
    {
        private readonly IConfiguration _config;
        public AutenticacaoLogin(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        [HttpPost("login")]

        public ActionResult Login([FromBody] Usuarios Autenticacao)
        {
            if(Autenticacao.Email == "admin" && Autenticacao.Senha == "admin")
            {
                return Ok(new { token = "21232f297a57a5a743894a0e4a801fc3" });
            }
            else
            {
                return BadRequest();
            }
        }

        

    }
}
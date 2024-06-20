using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Helpers;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUtil _util;

        private readonly string _destino = "Perfil";

        public AccountController(IAccountService accountService,
                                 ITokenService tokenService,
                                 IUtil util)
        {
            _util = util;
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userName = User.GetUserName();
            var user = await _accountService.GetUserByUserNameAsync(userName);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (await _accountService.UserExists(userDto.UserName))
                return BadRequest("Usuário já existe");

            var user = await _accountService.CreateAccountAsync(userDto);
            if (user != null)
                return Ok(new
                {
                    userName = user.UserName,
                    PrimeroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });

            return BadRequest("Usuário não criado, tente novamente mais tarde!");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            var user = await _accountService.GetUserByUserNameAsync(userLogin.Username);
            if (user == null) return Unauthorized("Usuário ou Senha está errado");

            var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
            if (!result.Succeeded) return Unauthorized();

            return Ok(new
            {
                userName = user.UserName,
                PrimeroNome = user.PrimeiroNome,
                token = _tokenService.CreateToken(user).Result
            });
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto.UserName != User.GetUserName())
                return BadRequest("Usuário Inválido");

            var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return BadRequest("Usuário Inválido");

            var userReturn = await _accountService.UpdateAccount(userUpdateDto);
            if (userReturn == null) return NoContent();

            return Ok(new
            {
                userName = userReturn.UserName,
                PrimeroNome = userReturn.PrimeiroNome,
                token = _tokenService.CreateToken(userReturn).Result
            });
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
            if (user == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                _util.DeleteImage(user.ImagemURL, _destino);
                user.ImagemURL = await _util.SaveImage(file, _destino);
            }
            var userRetorno = await _accountService.UpdateAccount(user);

            return Ok(userRetorno);
        }
    }
}
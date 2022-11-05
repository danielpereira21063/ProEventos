using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using System;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUtil _util;
        private readonly string _destino = "perfil";

        public AccountController(ITokenService tokenService, IAccountService accountService, IUtil util)
        {
            _tokenService = tokenService;
            _accountService = accountService;
            _util = util;
        }

        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar usuário: Erro -> {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.UserName);

                if (user == null) return Unauthorized("Usuário ou senha incorretos");

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);

                if (!result.Succeeded) return Unauthorized("Usuário ou senha incorretos");

                return Ok(new
                {
                    UserName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    Token = await _tokenService.CreateToken(user)
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao recuperar usuário: Erro -> {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _accountService.UserExists(userDto.UserName))
                {
                    return BadRequest("Usuário já existe");
                }

                var user = await _accountService.CreateAccountAsync(userDto);

                if (user != null)
                {
                    return Ok(new
                    {
                        primeiroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result,
                        userName = user.UserName
                    });
                }

                return BadRequest("Erro ao registrar usuário, tente novamente mais tarde.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("Update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                if (userUpdateDto.UserName != User.GetUserName())
                {
                    return Unauthorized("Usuário inválido.");
                }

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if (user == null) return Unauthorized("Usuário inválido");

                var userUpdated = await _accountService.UpdateAccountAsync(userUpdateDto);

                if (userUpdated == null) return BadRequest("Erro ao atualizar informações do usuário, tente novamente mais tarde.");

                return Ok(new
                {
                    primeiroNome = userUpdated.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result,
                    userName = userUpdated.UserName
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if (user == null) return NoContent();

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    _util.DeleteImage(user.ImagemUrl, _destino);
                    user.ImagemUrl = await _util.SaveImage(file, _destino);
                }

                var eventoRetorno = await _accountService.UpdateAccountAsync(user);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"Erro ao adicionar evento. \n{ex.Message}");
            }
        }
    }
}

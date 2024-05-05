using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using VendingMachine.API.Dto.Users;
using VendingMachine.Core.Exceptions;
using VendingMachine.Core.Models;
using VendingMachine.Core.Models.Exceptions;
using VendingMachine.Core.Services;
using VendingMachine.Infrastructure;

namespace VendingMachine.API.Controllers.Http
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(
        UsersService usersService,
        IOptions<JwtOptions> options) : Controller
    {
        private readonly UsersService usersService = usersService;

        [HttpPost("registration")]
        public async Task<ActionResult<GetUserResponse>> Registration([FromBody] RegisterUsersRequest RegisterUsersRequest)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var (user, token) = await usersService.Registration(RegisterUsersRequest.UserName, RegisterUsersRequest.Password);

                var userReponse = new GetUserResponse(user.Id, user.Name, user.AmountMoney, user.Role);

                HttpContext.Response.Cookies.Append(
                    options.Value.Cookie,
                    token,
                    new CookieOptions { MaxAge = TimeSpan.FromHours(options.Value.ExpiresHours) });

                return Ok(userReponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<GetUserResponse>> Login([FromBody] LoginUserRequest LoginUserRequest)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var (user, token) = await usersService.Login(LoginUserRequest.UserName, LoginUserRequest.Password);

                var userReponse = new GetUserResponse(user.Id, user.Name, user.AmountMoney, user.Role);

                HttpContext.Response.Cookies.Append(
                    options.Value.Cookie,
                    token,
                    new CookieOptions { MaxAge = TimeSpan.FromHours(options.Value.ExpiresHours) });

                return Ok(userReponse);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [Authorize, HttpPatch]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateUserRequest req)
        {
            try
            {
                var userId = await usersService.Update(
                    req.UserId,
                    req.Name,
                    req.Password,
                    req.AmountMoney, 
                    req.Role);

                return Ok(userId);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize, HttpGet("refresh")]
        public async Task<ActionResult<GetUserResponse>> Refresh()
        {
            try
            {
                var cookie = HttpContext.Request.Cookies["default-name"];

                if (cookie == null)
                    return Unauthorized(new ExceptionResponse((int)HttpStatusCode.Unauthorized, "Не авторизован"));

                var (user, token) = await usersService.Refresh(cookie);

                var userReponse = new GetUserResponse(user.Id, user.Name, user.AmountMoney, user.Role);

                HttpContext.Response.Cookies.Append(
                    options.Value.Cookie,
                    token,
                    new CookieOptions { MaxAge = TimeSpan.FromHours(options.Value.ExpiresHours) });

                return Ok(userReponse);
            }

            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}

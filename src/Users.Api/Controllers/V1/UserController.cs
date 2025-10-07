using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.Response;
using Users.Application.Services.Interfaces;

namespace Users.Api.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin,Usuario")]
        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmail([FromQuery] GetUserByEmailRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userServices.GetByEmailAsync(request);
            return Ok(result);
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin,Usuario")]
        [HttpGet("GetByNickName")]
        public async Task<IActionResult> GetNickName([FromQuery] GetUserByNickNameRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userServices.GetByNickNameAsync(request);
            return Ok(result);
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin,Usuario")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] GetUserByIdRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userServices.GetByIdAsync(request);
            return Ok(result);
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        [HttpGet("ListUsersNoTwoFactor")]
        public async Task<IActionResult> ListUsersNoTwoFactor(CancellationToken cancellationToken = default)
        {
            var result = await _userServices.ListUsersNoTwoFactor();
            return Ok(result);
        }

    }
}

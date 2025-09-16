using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Users.Application.Dtos.Requests;
using Users.Application.Services.Interfaces;

namespace Users.Api.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AdminController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public AdminController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        [HttpPatch("User/Block")]
        public async Task<IActionResult> BlockUser([FromBody] BlockUserRequest request, CancellationToken cancellationToken = default)
        {
            await _userServices.BlockUserAsync(request);
            return NoContent();
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("User/Delete")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request, CancellationToken cancellationToken = default)
        {
            await _userServices.DeleteAsync(request);
            return NoContent();
        }
    }
}

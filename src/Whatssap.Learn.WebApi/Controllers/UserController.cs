using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Whatssap.Learn.Entities;
using Whatssap.Learn.Repository;
using Whatssap.Learn.Services;

namespace Whatssap.Learn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;

        public UserController(IConfiguration config, ILogger<UserController> logger, IUserService userService)
        {
            _config = config;
            _logger = logger;
            _userService = userService;
          
        }

        [HttpPost]
        public ServiceResponse<User> SignUpUser(NewUser user)
        {
            _logger.LogInformation("create new user request recieved username = {username}", user.Username);
            var result = _userService.SignUpUser(user);
            if (!result.Success) _logger.LogError("Error recieved saving username = {username} and details = {details}", user.Username, result.ErrorMessage);
            return result;
        }

        [HttpPost("personalinfo/{userId:int}")]
        public ServiceResponse<PersonalInfo> UpdatePersonalInfo([FromRoute]int userId, [FromBody]PersonalInfo personalInfo)
        {
            _logger.LogInformation("updating personal info of user id = {userId}", userId);
            var result = _userService.UpdatePersonalInfo(personalInfo, userId);
            if (!result.Success) _logger.LogError("Error recieved updating personalInfo of userid = {userId} and details = {details}", userId, result.ErrorMessage);
            return result;
        }
    }
}

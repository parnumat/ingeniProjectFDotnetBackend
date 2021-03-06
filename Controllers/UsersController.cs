using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Methods;
using WebApi.Models;
using WebApi.Services;
using WebApi.Services.DataServices;

namespace WebApi.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class UsersController : ControllerBase {
        private IUserService _userService;
        private readonly IMapper _mapper;
        public UsersController (IUserService userService, IMapper mapper) {
            _userService = userService;
            _mapper = mapper;
        }

        // [HttpPost ("test")]
        // public IActionResult test (UserProfileFromSQl model) {
        //     var result = _mapper.Map<UserProfile> (model);
        //     var response = _userService.Authenticate (result);
        //     if (response == null)
        //         return BadRequest (new { message = "Data could not be null" });
        //     return Ok (response);
        // }

        [HttpPost ("Login")]
        public ActionResult<string> Login (AuthenticateRequest model) {
            // Authenticate users FROM ingeni Database
            Boolean status = AuthenticateUsers.AuthenticateUser (model);
            //You're our employee
            if (status == true) {
                //Get your profile from base SQL 
                var response = new MappingUserProfile (_mapper).UserProfileMapped (model);
                //Get JWT [Json Web Token]
                var responses = _userService.Authenticate (response,model.Username);

                return Ok (responses);
            }
            //You're not our employee
            return Unauthorized (new { message = "Username or password is incorrect" });
            //"{ \"message\": \"Username or password is incorrect\" }"

        }

        [Authorize]
        [HttpPost ("getProflie")]
        public IActionResult GetAllProfile (UserInputModel model) {
            var userProfike = _userService.DecodeToken (model.token);
            return Ok (userProfike);
        }

        [Authorize]
        [HttpPost ("check")]
        public IActionResult PostAll () {
            return Ok ("users");
        }
        /*
                // [Authorize]
                // [HttpPost ("authenticate")]
                // public IActionResult Authenticate (UserProfileFromSQl model) {
                //     var result = _mapper.Map<UserProfile> (model);
                // var response = _userService.Authenticate (result);

                // if (response == null)
                //     return BadRequest (new { message = "Username or password is incorrect" });

                //     return Ok (result);
                // }
                // [HttpPost]
                // public IActionResult Posttest (AuthenticateRequest model) {
                //     Boolean status = AuthenticateUsers.AuthenticateUser (model);
                //     if (status == true) {
                //         var response = ConnectionProfile.SetProfile (model.Username);

                //         return Ok (response);
                //     }
                //     return BadRequest (new { message = "Username or password is incorrect" });

                // }
                // [HttpGet ("test")] //Test connect DB
                // public ActionResult<string> Getstrings () {
                //     if (client != null) {
                //         return "Database connected";
                //     } else {
                //         return "Please try again later !!";
                //     }
                // }

                // [HttpGet ("data")] //response all USERs
                // public async Task<List<Register>> Getdata () {
                //     FirebaseResponse response = await client.GetTaskAsync ("Users/");
                //     List<Register> _allUser = response.ResultAs<List<Register>> ();
                //     return _allUser;
                // }

                // [HttpGet ("cnt")] //auto ID
                // public async Task<int> Getcnt () {
                //     FirebaseResponse res = await client.GetTaskAsync ("auto/id");
                //     AutoIncrement get = res.ResultAs<AutoIncrement> ();
                //     int i = get.cnt + 1;
                //     return i;
                // }

                // [HttpPost ("Register")]
                // public async Task<string> Register (Register model) {
                //     FirebaseResponse res = await client.GetTaskAsync ("auto/id");
                //     AutoIncrement get = res.ResultAs<AutoIncrement> ();
                //     model.Id = get.cnt + 1;
                //     SetResponse response = await client.SetTaskAsync ("Users/" + model.Id, model);
                //     var obj = new AutoIncrement {
                //         cnt = model.Id
                //     };
                //     FirebaseResponse resp = await client.UpdateTaskAsync ("auto/id", obj);
                //     return response.Body;
                // }

                // [HttpPost ("AddProfile")]
                // public async Task<ActionResult> AddProfile (UserProfile model) {
                //     FirebaseResponse res = await client.GetTaskAsync ("auto/profile");
                //     AutoIncrement get = res.ResultAs<AutoIncrement> ();
                //     model.ORG_ID = get.cnt + 1;
                //     SetResponse response = await client.SetTaskAsync ("Profiles/" + model.ORG_ID, model);
                //     var obj = new AutoIncrement {
                //         cnt = model.ORG_ID
                //     };
                //     FirebaseResponse resp = await client.UpdateTaskAsync ("auto/profile", obj);
                //     return Ok (response.Body);
                // }

                // [HttpPost ("Search/{id?}")]
                // public async Task<User> Search (int id) {
                //     FirebaseResponse response = await client.GetTaskAsync ("Users/" + id);
                //     User _user = response.ResultAs<User> ();
                //     return _user;
                // }

                // [HttpPost ("testcon")]
                // public ActionResult Testcon (AuthenticateRequest model) {
                //     string _message;
                //     Boolean status = AuthenticateUsers.AuthenticateUser (model);
                //     if (status == true) {
                //         _message = ConnectionProfile.SetProfile (model.Username);
                //         return Ok (_message);
                //     }
                //     return BadRequest (new { message = "Username or password is incorrect" });
                // }*/
    }
}
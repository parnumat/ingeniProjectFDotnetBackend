using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ingeniProjectFDotnetBackend.Models.Profiles;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Security;

namespace WebApi.Services {
    public interface IUserService {
        AuthenticateResponse Authenticate (UserProfile model);
        IEnumerable<UserProfile> GetAll ();
        UserProfile GetByOrg (string org);
    }

    public class UserService : IUserService {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        // DataBaseHostEnum _users = new DataBaseHostEnum();
        List<UserProfile> _users = new List<UserProfile> {
            new UserProfile {
            ORG_ID = "OPPN"
            },
            new UserProfile {
            ORG_ID = "LAP"
            },
            new UserProfile {
            ORG_ID = "KPP"
            },
            new UserProfile {
            ORG_ID = "KPR"
            },
            new UserProfile {
            ORG_ID = "IGS"
            }
        };
        private readonly AppSettings _appSettings;

        public UserService (IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
            // client = new FireSharp.FirebaseClient (config);
            // FirebaseResponse response = client.Get ("Users/");
            // _users = response.ResultAs<List<User>> ();
        }

        public AuthenticateResponse Authenticate (UserProfile model) {

            // var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (model == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken (model);

            return new AuthenticateResponse (model, token);
        }

        public IEnumerable<UserProfile> GetAll () {
            return _users;
        }

        public UserProfile GetByOrg (string org) {
            return _users.FirstOrDefault (x => x.ORG_ID == org);
        }

        // helper methods

        private string generateJwtToken (UserProfile user) {
            // generate token that is valid for 1 days
            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (new [] { new Claim ("ORG_ID", user.ORG_ID) }),
                Expires = DateTime.UtcNow.AddDays (1),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            return tokenHandler.WriteToken (token);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;
using TestTask.Models;
using System.Security.Claims;

namespace TestTask.Controllers
{
    public class ParserJWT
    {
        public static CurrentUserDTO parse(string token)
        {
            // var jwtToken = Request.Cookies["jwtToken"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var firstName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var lastName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            var isAdmin = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;


            var currentUser = new CurrentUserDTO(userId, firstName, lastName, isAdmin);

            return currentUser;
        }
    }

    public class CurrentUserDTO
    {
        public CurrentUserDTO(string userId, string firstName, string lastName, string isAdmin)
        {
            this.userId = Int32.Parse(userId);
            this.firstName = firstName;
            this.lastName = lastName;
            this.isAdmin = Convert.ToBoolean(isAdmin);
        }

        public int userId;
        public string firstName;
        public string lastName;
        public bool isAdmin;
    }
}
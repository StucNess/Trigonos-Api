using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Logic
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;


        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token:Key"]));
              
        }

        public string CreateToken(Usuarios usuarios,string rol)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuarios.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuarios.Nombre),
                new Claim(JwtRegisteredClaimNames.FamilyName, usuarios.Apellido),
                new Claim("username",usuarios.UserName),
                new Claim("ID",usuarios.Id),
            };
            if (!string.IsNullOrEmpty(rol))
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            var credentials = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
            var tokenConfiguration = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1440),
                SigningCredentials = credentials,
                Issuer = _config["AppSettings:Token:Issuer"]

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfiguration); 
            return tokenHandler.WriteToken(token);
        }
        public string CreateToken(Usuarios usuarios)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuarios.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuarios.Nombre),
                new Claim(JwtRegisteredClaimNames.FamilyName, usuarios.Apellido),
                new Claim("username",usuarios.UserName),

            };
            claims.Add(new Claim(ClaimTypes.Role, ""));
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenConfiguration = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = credentials,
                Issuer = _config["AppSettings:Token:Issuer"]
//string accessTokenExpire = await HttpContext.GetTokenAsync("expires_at");
        };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfiguration);
            return tokenHandler.WriteToken(token);
        }
    }
}

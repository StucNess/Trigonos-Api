﻿using Core.Entities;
using Core.Interface;
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

        public string CreateToken(Usuarios usuarios)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuarios.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuarios.Nombre),
                new Claim(JwtRegisteredClaimNames.FamilyName, usuarios.Apellido),
                new Claim(JwtRegisteredClaimNames.Email, usuarios.Email),
                new Claim("username",usuarios.UserName),
            };
            var credencials = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
            var tokenConfiguration = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = credencials,
                Issuer = _config["Token:Issuer"]

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfiguration); 
            return tokenHandler.WriteToken(token);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
  public class TokenService
  {
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config)
    {
      _config = config;
    }

    public string CreateToken(AppUser user)
    {
      //　トークンに渡す値
      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

      //　salt の生成  トークンのメタ情報の作成準備
      // Nuget  Microsoft.AspNetCore.Authentication.JwtBearer
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      // トークンのメタ情報の作成
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        //　トークンが使用する値
        Subject = new ClaimsIdentity(claims),
        //　有効期限　7日間
        Expires = DateTime.Now.AddDays(7),
        //　トークンキーメタ
        SigningCredentials = creds
      };

      //　メタ情報を元にトークンの作成
      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      // tokenHandler で書き出さないと有効でない
      return tokenHandler.WriteToken(token);
    }
  }
}
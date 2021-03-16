using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [AllowAnonymous]
  [ApiController]
  [Route("api/[controller]")]
  public class AccountController : Controller
  {
    private readonly UserManager<AppUser> _userManagr;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly TokenService _tokenService;

    public AccountController(UserManager<AppUser> userManagr,
        SignInManager<AppUser> signInManager, TokenService tokenService)
    {
      _tokenService = tokenService;
      _userManagr = userManagr;
      _signInManager = signInManager;
    }

    // 戻り値　UserDto　　引数 LoginDto
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      //AspNetUsers  大文字のEmail
      var user = await _userManagr.FindByEmailAsync(loginDto.Email);
      // FindByEmailAsync に対するエラー処理
      if (user == null) return Unauthorized();

      // ログイン方法 パスワード方式   ユーザー, パスワード, 失敗時の値 
      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

      // ログイン成功時  戻り値 UserDto  に値を入れて返す
      if (result.Succeeded)
      {
        return CreateUserObject(user);
      }
      // 全部失敗した場合
      return Unauthorized();
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if (await _userManagr.Users.AnyAsync(x => x.Email == registerDto.Email))
      {
        return BadRequest("Email taken");
      }
      if (await _userManagr.Users.AnyAsync(x => x.UserName == registerDto.Username))
      {
        return BadRequest("UserName taken");
      }

      var user = new AppUser
      {
        DisplayName = registerDto.DisplayName,
        Email = registerDto.Email,
        UserName = registerDto.Username
      };

      var result = await _userManagr.CreateAsync(user, registerDto.Password);

      if (result.Succeeded)
      {
        return CreateUserObject(user);
      }

      return BadRequest("Problem registering user");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
      var user = await _userManagr.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

      return CreateUserObject(user);
    }

    private UserDto CreateUserObject(AppUser user)
    {
      return new UserDto
      {
        DisplayName = user.DisplayName,
        Image = null,
        Token = _tokenService.CreateToken(user),
        Username = user.UserName
      };
    }

  }
}
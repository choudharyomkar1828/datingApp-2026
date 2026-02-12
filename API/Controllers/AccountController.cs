using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entitties;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;


namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService):BaseAPIController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDto>>Register(RegisterDto  registerdto)
    {
        if( await EmailExist(registerdto.Email))return BadRequest ("Email taken");
       using var hmac = new HMACSHA512();
       var user = new AppUser
       {
           DisplayName = registerdto.DisplayName,
           Email = registerdto.Email,
           PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
           PasswordSalt = hmac.Key
       };
       context.Users.Add(user);
       await context.SaveChangesAsync();

       return user.ToDto(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x =>x.Email == loginDto.Email);

        if(user == null)return Unauthorized("Invalid Email Address");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for(var i =0 ; i< ComputeHash.Length; i++)
        {
            if(ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
         return user.ToDto(tokenService);
        
    }

    private async Task<bool>EmailExist(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public class LoginDto
    {
        public string Email{get;set;} = "";
    public string Password {get;set;} = "";

    }
}


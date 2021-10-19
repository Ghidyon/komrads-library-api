﻿using AutoMapper;
using LibraryApi.Models.Dtos;
using LibraryApi.Models.Entities;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IRegisterLoginService _userService;
        public AccountController(IMapper mapper, UserManager<User> userManager,
            IRegisterLoginService userService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;

        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDTO userForRegistration)
        {
            //var users = _userService.Add(userForRegistration);
            //var user = _mapper.Map<User>(users);
            //var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            /**
            var user = new UserForRegistrationDTO
            {
                UserName = userForRegistration.UserName,
                Email = userForRegistration.Email,
                Password = userForRegistration.Password,
            };
            **/
            //await Task.FromResult(_userService.Add(user));
            
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            //await _userManager.CreateAsync(user, userForRegistration.Password);
            
            //await _userManager.CreateAsync(user, userForRegistration.Password);
            return Ok(result);

        }
    }
}

using AutoMapper;
using LibraryApi.Data.Interfaces;
using LibraryApi.Models.Dtos;
using LibraryApi.Models.Entities;

using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public UserService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper,
            ILoggerManager loggerManager
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<(IdentityResult, User)> CreateUserAsync([FromBody] UserForRegistrationDTO userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var createUser = await _userManager.CreateAsync(user, userForRegistration.Password);

            await _userManager.AddToRoleAsync(user, userForRegistration.Role.ToString());
            return (createUser, user);

        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<User> PatchUserAsync(string id, [FromBody] JsonPatchDocument<UserForUpdateDTO> userForUpdate)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _loggerManager.LogError("User does not exist");
                return user;
            }
            var userToUpdate = _mapper.Map<UserForUpdateDTO>(user);
            userForUpdate.ApplyTo(userToUpdate);
            _mapper.Map(userToUpdate, user);

            return (user);
        }

        public async Task<User> FindUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _loggerManager.LogError("User does not exist");
                return user;
            }
            return user;
        }
        public async Task<User> DeleteUserAsync(string id)
        {
            var user = await FindUserAsync(id);
            if (user == null)
            {
                _loggerManager.LogError("User does not exist");
                return user;
            }
            await _userManager.GetRolesAsync(user);
            await _userManager.DeleteAsync(user);
            return user;


        }
    }
}

//I don't know why, but it appears injecting a IRepository<User> into a system containing IUserManager will cause the system to crash and burn
// Of course, you can't implement CRUD operations from Repository Class with UserIdentity in this scenario. - Gideon Akunana

﻿using Users.Core.DTOs;
using Users.Core.Models;
using Users.Core.Responses;

namespace Users.Core.Interfaces
{
    public interface IUserService
    {
        public Task<User> Register(RegistrationDTO user);
        public Task<string> Login(LoginDTO user);
        public Task<UserInfoResponse> GetUser(string id);
    }
}

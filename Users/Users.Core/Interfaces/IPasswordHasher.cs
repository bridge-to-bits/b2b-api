﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Core.Interfaces
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);

        public bool VerifyHashedPassword(string providedPassword, string hashedPassword);
    }
}
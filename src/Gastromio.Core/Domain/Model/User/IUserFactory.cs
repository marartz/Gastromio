﻿using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.User
{
    public interface IUserFactory
    {
        Result<User> Create(
            Role role,
            string email,
            string password,
            bool checkPasswordPolicy,
            UserId createdBy
        );
    }
}

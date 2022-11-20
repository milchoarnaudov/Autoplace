﻿namespace Autoplace.Common.Services.Identity
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        string Username { get; }

        bool IsAdministrator { get; }
    }
}

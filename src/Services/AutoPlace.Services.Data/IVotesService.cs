﻿namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;
    using AutoPlace.Services.Data.Models.Votes;

    public interface IVotesService : ITransientService
    {
        Task CreateAsync(CreateVote vote);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        T GetVote<T>(string forUserId, string voterId);
    }
}

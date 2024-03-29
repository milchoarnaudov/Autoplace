﻿namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.Models.Comments;

    public interface ICommentsService
    {
        Task<int> CreateAsync(CreateComment comment);
    }
}

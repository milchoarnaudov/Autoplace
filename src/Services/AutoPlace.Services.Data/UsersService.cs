﻿namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.usersRepository.AllAsNoTracking().To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            return this.usersRepository.AllAsNoTracking().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public T GetByUsername<T>(string username)
        {
            return this.usersRepository.AllAsNoTracking().Where(x => x.UserName == username).To<T>().FirstOrDefault();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;

namespace API.Services.Logics
{
    public abstract class ServiceBase
    {
        protected readonly IRepositoryManager _repository;

        public ServiceBase(IRepositoryManager repository)
        {
            _repository = repository;
        }  
    }
}
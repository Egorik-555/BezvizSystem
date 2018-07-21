﻿using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Mapper;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services
{
    public class AnketaService : IService<AnketaDTO>
    {
        IUnitOfWork _database;
        IMapper _mapper;

        public AnketaService(IUnitOfWork db)
        {
            _database = db;

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FromDALToBLLProfile(_database));
            });

            _mapper = config.CreateMapper();
        }

        public IEnumerable<AnketaDTO> GetAll()
        {        
            var groups = _database.GroupManager.GetAll().ToList();         
            var anketaGroup = _mapper.Map<IEnumerable<GroupVisitor>, IEnumerable<AnketaDTO>>(groups);
            List<AnketaDTO> result = new List<AnketaDTO>(anketaGroup);
            return result;
        }

        public async Task<IEnumerable<AnketaDTO>> GetForUserAsync(string username)
        {                   
            var user = await _database.UserManager.FindByNameAsync(username);
            if (user != null)
            {             
                var groups = _database.GroupManager.GetAll().ToList();

                if (user.OperatorProfile.Role.ToUpper() != "ADMIN")
                    groups = groups.Where(g => g.User.OperatorProfile.UNP == user.OperatorProfile.UNP).ToList();              

                var anketaGroup = _mapper.Map<IEnumerable<GroupVisitor>, IEnumerable<AnketaDTO>>(groups);
                return anketaGroup;
            }
            else return null;
        }    

        public AnketaDTO GetById(int id)
        {
            var group = _database.GroupManager.GetById(id);
            var anketa = _mapper.Map<GroupVisitor, AnketaDTO>(group);
            return anketa;
        }

        public async Task<AnketaDTO> GetByIdAsync(int id)
        {
            var group = await _database.GroupManager.GetByIdAsync(id);
            var anketa = _mapper.Map<GroupVisitor, AnketaDTO>(group);
            return anketa;
        }  

        public void Dispose()
        {
            _database.Dispose();
        }

        public Task<OperationDetails> Update(AnketaDTO visitor)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> Create(AnketaDTO visitor)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> Delete(int id)
        {
            throw new NotImplementedException();
        }     
    }
}

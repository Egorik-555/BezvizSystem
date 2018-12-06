using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BezvizSystem.BLL.Mapper;

namespace BezvizSystem.BLL.Services
{
    public class VisitorService : IService<VisitorDTO>
    {
        IUnitOfWork _database;
        IMapper _mapper;
        IXMLDispatcher _xmlDispatcher;

        public VisitorService(IUnitOfWork uow)
        {
            _database = uow;
            _xmlDispatcher = new XMLDispatcher(_database);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfile(_database))).CreateMapper();
        }

        public async Task<OperationDetails> Create(VisitorDTO visitor)
        {
            try
            {              
                var model = _mapper.Map<VisitorDTO, Visitor>(visitor);
                await _xmlDispatcher.New(model);
                var newVisitor = _database.VisitorManager.Create(model);          
                return new OperationDetails(true, "Турист создан", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public async Task<OperationDetails> Delete(int id)
        {
            try
            {
                var visitor = await _database.VisitorManager.GetByIdAsync(id);
                if (visitor != null)
                {
                    await _xmlDispatcher.Remove(visitor);
                    _database.VisitorManager.Delete(id);                 
                    return new OperationDetails(true, "Турист удален", "");
                }
                else return new OperationDetails(false, "Турист не найден", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public async Task<OperationDetails> Update(VisitorDTO visitor)
        {
            try
            {
                var model = await _database.VisitorManager.GetByIdAsync(visitor.Id);
                if (model != null)
                {
                    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelVisitor(_database, model))).CreateMapper();
                    var m = mapper.Map<VisitorDTO, Visitor>(visitor);

                    await _xmlDispatcher.Edit(model);
                    _database.VisitorManager.Update(m);
                    
                    return new OperationDetails(true, "Турист изменен", "");
                }
                else return new OperationDetails(false, "Турист не найден", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public IEnumerable<VisitorDTO> GetAll()
        {
            var visitors = _database.VisitorManager.GetAll().ToList();
            var visitorsDto = _mapper.Map<IEnumerable<Visitor>, IEnumerable<VisitorDTO>>(visitors);
            return visitorsDto;
        }

        public async Task<VisitorDTO> GetByIdAsync(int id)
        {
            var visitor = await _database.VisitorManager.GetByIdAsync(id);           
            var visitorDto = _mapper.Map<Visitor, VisitorDTO>(visitor);
            return visitorDto;
        }

        public VisitorDTO GetById(int id)
        {
            var visitor = _database.VisitorManager.GetById(id);
            var visitorDto = _mapper.Map<Visitor, VisitorDTO>(visitor);
            return visitorDto;
        }

        public Task<IEnumerable<VisitorDTO>> GetForUserAsync(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VisitorDTO> GetForUser(string username)
        {
            throw new NotImplementedException();
        }
    }
}

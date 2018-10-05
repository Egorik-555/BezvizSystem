using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Mapper;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services
{
    public class GroupService : IService<GroupVisitorDTO>
    {
        IUnitOfWork _database;
        IMapper _mapper;
        IXMLDispatcher _xmlDispatcher;

        public GroupService(IUnitOfWork uow)
        {
            _database = uow;
            _xmlDispatcher = new XMLDispatcher(_database);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfile(_database))).CreateMapper();
        }

        private void DateAndUserCreate(GroupVisitor model, string user)
        {
            model.DateInSystem = DateTime.Now;
            foreach (var visitor in model.Visitors)
            {
                if (visitor != null)
                {
                    visitor.DateInSystem = DateTime.Now;
                    visitor.UserInSystem = user;
                }
            }
        }

        private void DateAndUserUpdate(GroupVisitor model, /*string userCreate,*/ string userEdit)
        {
         //   model.
            model.DateEdit = DateTime.Now;
            foreach (var visitor in model.Visitors)
            {
                if (visitor != null)
                {
                    visitor.DateEdit = DateTime.Now;
                    visitor.UserEdit = userEdit;
                }
            }
        }

        public async Task<OperationDetails> Create(GroupVisitorDTO group)
        {
            try
            {
                var model = _mapper.Map<GroupVisitorDTO, GroupVisitor>(group);
                var user = await _database.UserManager.FindByNameAsync(group.UserInSystem);

                if (user != null)
                {
                    model.TranscriptUser = user.OperatorProfile.Transcript;
                    //system data
                    DateAndUserCreate(model, model.UserInSystem);                 
                    //create visitor
                    _database.GroupManager.Create(model);
                    //xml dispatch
                    await _xmlDispatcher.New(model.Visitors);
                    return new OperationDetails(true, "Группа туристов создана", "");
                }
                else return new OperationDetails(false, "Пользователь не найден", "");
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
                var group = await _database.GroupManager.GetByIdAsync(id);
                ICollection<Visitor> oldVisitors = new List<Visitor>(group.Visitors);
                if (group != null)
                {                 
                    _database.GroupManager.Delete(group.Id);
                    await _xmlDispatcher.Remove(oldVisitors);

                    return new OperationDetails(true, "Группа туристов удалена", "");
                }
                else return new OperationDetails(false, "Группа туристов не найдена", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public async Task<OperationDetails> Update(GroupVisitorDTO group)
        {
            try
            {
                var model = await _database.GroupManager.GetByIdAsync(group.Id);
                ICollection<Visitor> oldVisitors = new List<Visitor>(model.Visitors);

                if (model != null)
                {                  
                    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ProfileGroupDtoToDao(_database, model))).CreateMapper();
                    var modelNew = mapper.Map<GroupVisitorDTO, GroupVisitor>(group);

                    DateAndUserUpdate(modelNew, group.UserEdit);
                    _database.GroupManager.Update(modelNew);
                    await _xmlDispatcher.Edit(oldVisitors, modelNew.Visitors);

                    return new OperationDetails(true, "Группа туристов изменена", "");
                }
                else return new OperationDetails(false, "Группа туристов не найдена", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public IEnumerable<GroupVisitorDTO> GetAll()
        {
            var groups = _database.GroupManager.GetAll().ToList();
            var groupsDto = _mapper.Map<IEnumerable<GroupVisitor>, IEnumerable<GroupVisitorDTO>>(groups);

            return groupsDto;
        }

        public async Task<GroupVisitorDTO> GetByIdAsync(int id)
        {
            var group = await _database.GroupManager.GetByIdAsync(id);
            var groupDto = _mapper.Map<GroupVisitor, GroupVisitorDTO>(group);
            return groupDto;
        }

        public GroupVisitorDTO GetById(int id)
        {
            var group = _database.GroupManager.GetById(id);
            var groupDto = _mapper.Map<GroupVisitor, GroupVisitorDTO>(group);
            return groupDto;
        }

        public Task<IEnumerable<GroupVisitorDTO>> GetForUserAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}

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
    public delegate Task<OperationDetails> XMLAction(int id);

    public class GroupService : IService<GroupVisitorDTO>
    {
        IUnitOfWork _database;
        IMapper _mapper;
        IXMLDispatcher _xmlDispatcher;

        public GroupService(IUnitOfWork uow, IXMLDispatcher xmlDispatcher)
        {
            _database = uow;
            _xmlDispatcher = xmlDispatcher;
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfile(_database))).CreateMapper();
        }

        private void DateAndUserFroVisitors(ICollection<Visitor> visitors, string user, DateTime? date)
        {
            foreach (var visitor in visitors)
            {
                visitor.DateInSystem = date;
                visitor.UserInSystem = user;
            }
        }

        private void XMLActionForVisitors(ICollection<Visitor> visitors, XMLAction action)
        {
            foreach (var visitor in visitors)
            {
                action(visitor.Id);
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
                    //data for visitors
                    DateAndUserFroVisitors(model.Visitors, model.UserInSystem, model.DateInSystem);
                    ///
                    //create
                    _database.GroupManager.Create(model);
                    ///
                    //xml dispatch
                    XMLActionForVisitors(model.Visitors, _xmlDispatcher.New);
                    ///
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
                if (group != null)
                {
                    XMLActionForVisitors(group.Visitors, _xmlDispatcher.Remove);
                    _database.GroupManager.Delete(group.Id);
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

                if (model != null)
                {
                    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelGroup(_database, model))).CreateMapper();
                    var modelNew = mapper.Map<GroupVisitor>(group);

                    _database.GroupManager.Update(modelNew);
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

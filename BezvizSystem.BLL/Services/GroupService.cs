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

        public GroupService(IUnitOfWork uow)
        {
            _database = uow;

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfile(_database))).CreateMapper();
        }

        public async Task<OperationDetails> Create(GroupVisitorDTO group)
        {
            try
            {
                var model = _mapper.Map<GroupVisitorDTO, GroupVisitor>(group);
                var user = await _database.UserManager.FindByNameAsync(group.UserInSystem);

                //data of visitors
                foreach (var visitor in model.Visitors)
                {
                    visitor.StatusOfRecord = StatusOfRecord.Save;
                    visitor.StatusOfOperation = StatusOfOperation.Add; // new record
                    visitor.DateInSystem = DateTime.Now;
                    visitor.UserInSystem = model.UserInSystem;
                }

                //data for group of visitors
                model.User = user;
                model.DateInSystem = DateTime.Now;

                _database.GroupManager.Create(model);
                return new OperationDetails(true, "Группа туристов создана", "");
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
                    var visitors = group.Visitors.ToList();

                    int k = 0;

                    foreach (var item in visitors)
                    {
                        //if item have status code = 1 (new)
                        if (item.StatusOfRecord == StatusOfRecord.Save)
                        {
                            _database.VisitorManager.Delete(item.Id); // remove item
                            k++;
                        }
                        // if item send to pogran
                        else
                        {
                            item.StatusOfRecord = StatusOfRecord.Save; // mark to upload
                            item.StatusOfOperation = StatusOfOperation.Remove; //mark to delete
                            _database.VisitorManager.Update(item);
                        }
                    }

                    //if deleted all visitors in group
                    if (group.Visitors.Count() == k)
                    {
                        _database.GroupManager.Delete(group.Id);
                        
                    }

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

                    var modelNew = mapper.Map<GroupVisitorDTO, GroupVisitor>(group); 
                    
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

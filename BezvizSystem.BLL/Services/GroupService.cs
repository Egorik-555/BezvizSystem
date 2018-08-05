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
                var status = _database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();

                //data of visitors
                foreach (var visitor in model.Visitors)
                {
                    visitor.Status = status;
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
                    var status = _database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();
                    var visitors = group.Visitors.ToList();

                    int k = 0;

                    foreach (var item in visitors)
                    {
                        //if item have status code = 1 (new)
                        if (item.Status == null || item.Status.Code == 1)
                        {
                            _database.VisitorManager.Delete(item.Id); // remove item
                            k++;
                        }
                        // if item send to pogran
                        else
                        {
                            item.Status = status; // mark to upload
                            item.StatusOfOperation = StatusOfOperation.Remove; //mark to delete
                            _database.VisitorManager.Update(item);
                        }
                    }

                    //if deleted all visitors in group
                    if (group.Visitors.Count() == k)
                    {
                        _database.GroupManager.Delete(group.Id);
                        return new OperationDetails(true, "Группа туристов удалена", "");
                    }
                    // if group send to pogran
                    else
                    {                      
                        return new OperationDetails(true, "Группа туристов помечена к удалению", "");
                    }
                }
                else return new OperationDetails(false, "Группа туристов не найдена", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        private ICollection<Visitor> UpdateVisitors(string userName, IEnumerable<Visitor> newVisitors, IEnumerable<Visitor> oldVisitors)
        {            
            var result = new List<Visitor>();
            var visitorNew = newVisitors;
            var visitorOld = oldVisitors;
            var statusForNewItem = _database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();

            foreach (var itemNew in visitorNew)
            {
                itemNew.StatusOfOperation = StatusOfOperation.Add;
                itemNew.Status = statusForNewItem;            

                var itemOld = visitorOld.Where(v => v.Id == itemNew.Id).FirstOrDefault();

                // if item new
                if (itemOld == null)
                {
                    itemNew.DateInSystem = DateTime.Now;
                    itemNew.UserInSystem = userName;
                    result.Add(itemNew);
                }
                else
                {
                    //if item have status code = 1 (new)
                    if (itemOld.Status.Code == 1)
                    {
                        //edit old item
                        if (!itemOld.Equals(itemNew))
                        {
                            itemNew.UserEdit = userName;
                            itemNew.DateEdit = DateTime.Now;
                        }

                        result.Add(itemNew);
                    }
                    //if group have status code = 2, 3 (send or recieve pogran)
                    else
                    {
                        itemNew.Status = itemOld.Status;
                        //edit old item
                        if (!itemOld.Equals(itemNew))
                        {
                            itemNew.StatusOfOperation = StatusOfOperation.Edit;
                            itemNew.Status = statusForNewItem;
                            itemNew.UserEdit = userName;
                            itemNew.DateEdit = DateTime.Now;
                        }

                        result.Add(itemNew);
                    }
                }              
            }

            //delete item in visitorsOld if thier send to pogran
            foreach (var itemOld in visitorOld)
            {
                if (itemOld.Status.Code != 1)
                {
                    var itemNew = visitorNew.Where(v => v.Id == itemOld.Id).FirstOrDefault();
                    if (itemNew == null)
                    {
                        itemOld.Status = statusForNewItem;
                        itemOld.StatusOfOperation = StatusOfOperation.Remove;
                        result.Add(itemOld);
                    }
                }
            }

            return result;
        }

        public async Task<OperationDetails> Update(GroupVisitorDTO group)
        {
            try
            {
                var model = await _database.GroupManager.GetByIdAsync(group.Id);            
                if (model != null)
                {               
                    var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelGroup(_database, model))).CreateMapper();

                    var oldVisitors = model.Visitors;
                    var modelNew = mapper.Map<GroupVisitorDTO, GroupVisitor>(group);
                    modelNew.DateEdit = DateTime.Now;

                    var newVisitors = UpdateVisitors(modelNew.UserEdit, modelNew.Visitors, oldVisitors);

                    //add new visitors
                    modelNew.Visitors = newVisitors;
                    //var statusForNewItem = Database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();
                    //modelNew.Status = statusForNewItem;
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

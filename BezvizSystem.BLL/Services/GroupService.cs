using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
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
        IUnitOfWork Database;
        IMapper mapper;

        public GroupService(IUnitOfWork uow)
        {
            Database = uow;

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupVisitorDTO, GroupVisitor>().
                     ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => Database.CheckPointManager.GetAll().Where(n => n.Name == src.CheckPoint).FirstOrDefault()));
                cfg.CreateMap<VisitorDTO, Visitor>().
                    ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => Database.NationalityManager.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault())).
                    ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Database.Genders.GetAll().Where(n => n.Name == src.Gender).FirstOrDefault()));

                cfg.CreateMap<GroupVisitor, GroupVisitorDTO>().
                    ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name));
                cfg.CreateMap<Visitor, VisitorDTO>().
                    ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name)).
                    ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.Name));

            }).CreateMapper();
        }

        public async Task<OperationDetails> Create(GroupVisitorDTO group)
        {
            try
            {
                var model = mapper.Map<GroupVisitorDTO, GroupVisitor>(group);
                var user = await Database.UserManager.FindByNameAsync(group.UserInSystem);
                var status = Database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();

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
                model.Status = status;

                Database.GroupManager.Create(model);
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
                var group = await Database.GroupManager.GetByIdAsync(id);
                if (group != null)
                {
                    var status = Database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();
                    var visitors = group.Visitors.ToList();

                    int k = 0;

                    foreach (var item in visitors)
                    {
                        //if group have status code = 1 (new)
                        if (item.Status == null || item.Status.Code == 1)
                        {
                            Database.VisitorManager.Delete(item.Id); // remove item
                            k++;
                        }
                        // if group send to pogran
                        else
                        {
                            item.Status = status; // mark to upload
                            item.StatusOfOperation = StatusOfOperation.Remove; //mark to delete
                            Database.VisitorManager.Update(item);
                        }
                    }

                    //if group have status code = 1 (new) and count deleted item = count of visitors
                    if (group.Visitors.Count() == k && (group.Status.Code == 1 || group.Status == null))
                    {
                        Database.GroupManager.Delete(group.Id);
                        return new OperationDetails(true, "Группа туристов удалена", "");
                    }
                    // if group send to pogran
                    else
                    {
                        group.Status = status; // mark to upload
                        Database.GroupManager.Update(group);
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

        private ICollection<Visitor> UpdateVisitors(GroupVisitorDTO groupDTO, GroupVisitor group)
        {            
            var result = new List<Visitor>();
            var visitorNew = mapper.Map<IEnumerable<VisitorDTO>, IEnumerable<Visitor>>(groupDTO.Visitors);
            var visitorOld = group.Visitors;
            var statusForNewItem = Database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();

            foreach (var itemNew in visitorNew)
            {
                itemNew.StatusOfOperation = StatusOfOperation.Add;
                itemNew.Status = statusForNewItem;
                itemNew.DateInSystem = itemNew.DateInSystem ?? DateTime.Now;
                itemNew.UserInSystem = itemNew.UserInSystem ?? groupDTO.UserInSystem;

                var itemOld = visitorOld.Where(v => v.Id == itemNew.Id).FirstOrDefault();

                // if item new
                if (itemOld == null)
                {
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
                            itemNew.UserEdit = groupDTO.UserEdit;
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
                            itemNew.UserEdit = groupDTO.UserEdit;
                            itemNew.DateEdit = DateTime.Now;
                        }

                        result.Add(itemNew);
                    }
                }              
            }

            //delete item in visitorsOld if thier item send to pogran
            foreach (var itemOld in visitorOld)
            {
                if (itemOld.Status.Code != 1)
                {
                    var itemNew = visitorNew.Where(v => v.Id == itemOld.Id).FirstOrDefault();
                    if (itemNew == null)
                    {
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
                var model = await Database.GroupManager.GetByIdAsync(group.Id);
                if (model != null)
                {
                  
                    var mapper = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<GroupVisitorDTO, GroupVisitor>().ConstructUsing(v => model).
                            ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => Database.CheckPointManager.GetAll().Where(n => n.Name == src.CheckPoint).FirstOrDefault()));
                        cfg.CreateMap<VisitorDTO, Visitor>().
                            ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => Database.NationalityManager.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault())).
                            ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Database.Genders.GetAll().Where(n => n.Name == src.Gender).FirstOrDefault()));
                    }
                    ).CreateMapper();

                    var m = mapper.Map<GroupVisitorDTO, GroupVisitor>(group);
                    m.DateEdit = DateTime.Now;

                    var newVisitors = UpdateVisitors(group, model);
                   
                    //add new visitors
                    m.Visitors = newVisitors;

                    Database.GroupManager.Update(m);

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
            Database.Dispose();
        }

        public IEnumerable<GroupVisitorDTO> GetAll()
        {
            var groups = Database.GroupManager.GetAll().ToList();
            var groupsDto = mapper.Map<IEnumerable<GroupVisitor>, IEnumerable<GroupVisitorDTO>>(groups);

            return groupsDto;
        }

        public async Task<GroupVisitorDTO> GetByIdAsync(int id)
        {
            var group = await Database.GroupManager.GetByIdAsync(id);
            var groupDto = mapper.Map<GroupVisitor, GroupVisitorDTO>(group);
            return groupDto;
        }

        public GroupVisitorDTO GetById(int id)
        {
            var group = Database.GroupManager.GetById(id);
            var groupDto = mapper.Map<GroupVisitor, GroupVisitorDTO>(group);
            return groupDto;
        }

        public Task<IEnumerable<GroupVisitorDTO>> GetForUserAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}

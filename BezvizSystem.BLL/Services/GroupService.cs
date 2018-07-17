using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Entities;
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
                model.User = user;
                model.DateInSystem = DateTime.Now;
                model.Status = await Database.StatusManager.GetByIdAsync(1);

                //data of visitors
                foreach (var visitor in model.Visitors)
                {
                    visitor.StatusOfOperation = 1; // new record
                    visitor.DateInSystem = model.DateInSystem;
                    visitor.UserInSystem = model.UserInSystem;
                }

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
                    var visitors = group.Visitors.ToList();
                    foreach (var item in visitors)
                    {
                        //if group have status code = 1 (new)
                        if (group.Status.Code == 1)
                        {
                            Database.VisitorManager.Delete(item.Id);
                        }
                        // if group send to pogran
                        else
                        {
                            item.StatusOfOperation = 3;
                            Database.VisitorManager.Update(item);
                        }
                    }

                    //if group have status code = 1 (new)
                    if (group.Status.Code == 1)
                    {
                        Database.GroupManager.Delete(group.Id);
                        return new OperationDetails(true, "Группа туристов удалена", "");
                    }
                    // if group send to pogran
                    else
                    {
                        group.Status = Database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();
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

        private IEnumerable<Visitor> UpdateVisitors(GroupVisitorDTO groupDTO, GroupVisitor group)
        {
            var visitorNew = mapper.Map<IEnumerable<VisitorDTO>, IEnumerable<Visitor>>(groupDTO.Visitors);
            var visitor = group.Visitors;
          
            //if group have status code = 1 (new)
            if(group.Status.Code == 1)
            {
                foreach(var itemNew in visitorNew)
                {
                    var item = visitor.Where(v => v.Id == itemNew.Id).FirstOrDefault();
                    if(item == null)
                    {

                    }
                }
            }
            //if group have status code = 2, 3 (send or recieve pogran)
            else
            {

            }
        }

        public async Task<OperationDetails> Update(GroupVisitorDTO group)
        {
            try
            {
                var model = await Database.GroupManager.GetByIdAsync(group.Id);
                if (model != null)
                {
                    //delete old visitors             
                    //var visitors = model.Visitors.ToList();
                    //foreach (var item in visitors)
                    //    Database.VisitorManager.Delete(item.Id);

                    //if group have status code = 1 (new)
                    if (model.Status.Code == 1)
                    {

                    }
                    //if group have status code = 2, 3 (send or recieve pogran)
                    else
                    {

                    }


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

                    //add new visitors
                    foreach (var item in m.Visitors)
                    {
                        item.UserInSystem = group.UserInSystem;
                        item.DateInSystem = group.DateInSystem;
                        item.UserEdit = group.UserEdit;
                        item.DateEdit = DateTime.Now;
                    }

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

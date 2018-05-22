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
                cfg.CreateMap<GroupVisitorDTO, GroupVisitor>();
                cfg.CreateMap<VisitorDTO, Visitor>();
                cfg.CreateMap<GroupVisitor, GroupVisitorDTO>();
                cfg.CreateMap<Visitor, VisitorDTO>();

            }).CreateMapper();
        }

        public async Task<OperationDetails> Create(GroupVisitorDTO group)
        {
            try
            {
                var model = mapper.Map<GroupVisitorDTO, GroupVisitor>(group);
                var user = await Database.UserManager.FindByNameAsync(group.UserInSystem);
                model.User = user;
                model.Status = Database.StatusManager.GetAll().Where(s => s.Code == 1).FirstOrDefault();
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
                        Database.VisitorManager.Delete(item.Id);
                    Database.GroupManager.Delete(group.Id);
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
                var model = await Database.GroupManager.GetByIdAsync(group.Id);
                if (model != null)
                {
                    //удаляем старые visitors
                    var visitors = model.Visitors.ToList();
                    foreach (var item in visitors)
                        Database.VisitorManager.Delete(item.Id);
               
                    var mapper = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<GroupVisitorDTO, GroupVisitor>().ConstructUsing(v => model);
                        cfg.CreateMap<VisitorDTO, Visitor>();
                    }
                    ).CreateMapper();

                    var m = mapper.Map<GroupVisitorDTO, GroupVisitor>(group);

                    //добавляем новые visitors
                    foreach (var item in m.Visitors)
                    {
                        item.UserInSystem = group.UserInSystem;
                        item.DateInSystem = DateTime.Now;
                    //    Database.VisitorManager.Create(item);
                    }

                    var user = await Database.UserManager.FindByNameAsync(group.UserInSystem);
                    m.User = user;              
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
    }
}

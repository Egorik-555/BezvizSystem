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
    public class XMLDispatcher : IXMLDispatcher
    {
        IUnitOfWork _database;

        public XMLDispatcher(IUnitOfWork uow)
        {
            _database = uow;
        }

        public Task<OperationDetails> New(Visitor visitor)
        {
            var task = Task<OperationDetails>.Factory.StartNew(() =>
            {
                try
                {
                    var dispatch = new XMLDispatch
                    {
                        Id = visitor.Id,
                        Status = Status.New,
                        Operation = Operation.Add,
                        DateInSystem = DateTime.Now
                    };

                    _database.XMLDispatchManager.Create(dispatch);
                    return new OperationDetails(true, "Запись готова для передачи");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, ex.Message);
                }
            }
            );

            return task;
        }

        public async Task<List<OperationDetails>> New(ICollection<Visitor> visitors)
        {
            List<OperationDetails> list = new List<OperationDetails>();
            foreach (var visitor in visitors)
            {
                var result = await New(visitor);
                list.Add(result);
            }

            return list;
        }

        public async Task<OperationDetails> Send(Visitor visitor)
        {
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(visitor.Id);
            if (dispatch != null)
            {
                try
                {
                    dispatch.Status = Status.Send;
                    dispatch.Operation = Operation.Done;
                    dispatch.DateEdit = DateTime.Now;
                    _database.XMLDispatchManager.Update(dispatch);
                    return new OperationDetails(true, "Запись передана");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, ex.Message);
                }
            }
            else
            {
                return new OperationDetails(false, "Отчет xml передачи не найден");
            }
        }

        public async Task<List<OperationDetails>> Send(ICollection<Visitor> visitors)
        {
            List<OperationDetails> list = new List<OperationDetails>();
            foreach (var visitor in visitors)
            {
                var result = await Send(visitor);
                list.Add(result);
            }

            return list;
        }

        public async Task<OperationDetails> Recd(Visitor visitor)
        {
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(visitor.Id);
            if (dispatch != null)
            {
                try
                {
                    dispatch.Status = Status.Recd;
                    dispatch.Operation = Operation.Done;
                    dispatch.DateEdit = DateTime.Now;
                    _database.XMLDispatchManager.Update(dispatch);
                    return new OperationDetails(true, "Запись принята");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, ex.Message);
                }
            }
            else
            {
                return new OperationDetails(false, "Отчет xml не найден");
            }
        }

        public async Task<List<OperationDetails>> Recd(ICollection<Visitor> visitors)
        {
            List<OperationDetails> list = new List<OperationDetails>();
            foreach (var visitor in visitors)
            {
                var result = await Recd(visitor);
                list.Add(result);
            }

            return list;
        }

        public async Task<OperationDetails> Edit(Visitor visitor)
        {            
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(visitor.Id);
            if (dispatch != null)
            {
                try
                {
                    if (dispatch.Status != Status.New)
                    {
                        dispatch.Operation = Operation.Edit;
                        dispatch.DateEdit = DateTime.Now;
                        _database.XMLDispatchManager.Update(dispatch);
                    }
                    return new OperationDetails(true, "Запись готова для редактирования");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, ex.Message);
                }
            }
            else
            {
                return new OperationDetails(false, "Отчет xml не найден");
            }
        }

        public async Task<List<OperationDetails>> Edit(ICollection<Visitor> oldVisitors, ICollection<Visitor> newVisitors)
        {
            List<OperationDetails> list = new List<OperationDetails>();
          
            foreach (var visitor in newVisitors)
            {
                //add visitors are not in old visitors
                if (!oldVisitors.Contains(visitor))
                {
                    var result = await New(visitor);
                    list.Add(result);
                }
                //edit visitors
                else 
                {
                    var result = await Edit(visitor);
                    list.Add(result);
                }
            }

            //delete visitors do not contain in new visitors
            foreach (var visitor in oldVisitors)
            {
                if (!newVisitors.Contains(visitor))
                {
                    var result = await Remove(visitor);
                    list.Add(result);
                }
            }

            return list;
        }

        public async Task<OperationDetails> Remove(Visitor visitor)
        {
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(visitor.Id);
            if (dispatch != null)
            {
                try
                {
                    if (dispatch.Status != Status.New)
                    {
                        dispatch.Operation = Operation.Remove;
                        dispatch.DateEdit = DateTime.Now;
                        _database.XMLDispatchManager.Update(dispatch);
                    }
                    else
                    {
                        _database.XMLDispatchManager.Delete(dispatch.Id);
                    }
                    return new OperationDetails(true, "Запись удалена");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, ex.Message);
                }
            }
            else
            {
                return new OperationDetails(false, "Отчет xml не найден");
            }
        }

        public async Task<List<OperationDetails>> Remove(ICollection<Visitor> visitors)
        {
            List<OperationDetails> list = new List<OperationDetails>();
            foreach (var visitor in visitors)
            {
                var result = await Remove(visitor);
                list.Add(result);
            }

            return list;
        }     
    }
}

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

        public Task<OperationDetails> New(int IdVisitor)
        {
            var task = Task<OperationDetails>.Factory.StartNew(() =>
            {
                try
                {
                    var dispatch = new XMLDispatch
                    {
                        Id = IdVisitor,
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

        public async Task<OperationDetails> Send(int IdVisitor)
        {
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(IdVisitor);
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

        public async Task<OperationDetails> Recd(int IdVisitor)
        {
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(IdVisitor);
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

        public async Task<OperationDetails> Edit(int IdVisitor)
        {
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(IdVisitor);
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

        public async Task<OperationDetails> Remove(int IdVisitor)
        {
            var dispatch = await _database.XMLDispatchManager.GetByIdAsync(IdVisitor);
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
                    return new OperationDetails(true, "Запись готова для удаления");
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
    }
}

using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Tests
{
    public class TestRepoXmlDispatch : IRepositoryXMLDispatch<XMLDispatch, int>
    {
        List<XMLDispatch> db;

        public TestRepoXmlDispatch()
        {
            db = new List<XMLDispatch>
            {
                //new XMLDispatch { Id = 1, IdVisitor = 11, Status = Status.New, Operation = Operation.Add},
                //new XMLDispatch { Id = 13, IdVisitor = 11, Status = Status.New, Operation = Operation.Done},


                //new XMLDispatch { Id = 14, IdVisitor = 12, Status = Status.Send, Operation = Operation.Done},
                //new XMLDispatch { Id = 15, IdVisitor = 12, Status = Status.Recd, Operation = Operation.Done},
                //new XMLDispatch { Id = 16, IdVisitor = 12, Status = Status.Send, Operation = Operation.Edit},


                //new XMLDispatch { Id = 17, IdVisitor = 13, Status = Status.New, Operation = Operation.Done},
                //new XMLDispatch { Id = 18, IdVisitor = 13, Status = Status.Send, Operation = Operation.Done},
                //new XMLDispatch { Id = 19, IdVisitor = 13, Status = Status.Recd, Operation = Operation.Remove},


                //new XMLDispatch { Id = 4, IdVisitor = 11, Status = Status.New, Operation = Operation.Remove},
                //new XMLDispatch { Id = 5, IdVisitor = 11, Status = Status.Send, Operation = Operation.Done},
                //new XMLDispatch { Id = 6, IdVisitor = 11, Status = Status.Send, Operation = Operation.Add},
                //new XMLDispatch { Id = 7, IdVisitor = 11, Status = Status.Send, Operation = Operation.Edit},
                //new XMLDispatch { Id = 8, IdVisitor = 11, Status = Status.Send, Operation = Operation.Remove},
                //new XMLDispatch { Id = 9, IdVisitor = 11, Status = Status.Recd, Operation = Operation.Done},
                //new XMLDispatch { Id = 10, IdVisitor = 11, Status = Status.Recd, Operation = Operation.Add},
                //new XMLDispatch { Id = 11, IdVisitor = 11, Status = Status.Recd, Operation = Operation.Edit},
                //new XMLDispatch { Id = 12, IdVisitor = 11, Status = Status.Recd, Operation = Operation.Remove}
            };
        }


        public XMLDispatch Create(XMLDispatch item)
        {
            throw new NotImplementedException();
        }

        public XMLDispatch Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<XMLDispatch> GetAll()
        {
            throw new NotImplementedException();
        }

        public XMLDispatch GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<XMLDispatch> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Status GetStatusByIdRecord(int id)
        {
            //var allItems = db.Where(i => i.IdVisitor == id);

            //// all items with status recd and all operations done
            //var itemsRecd = allItems.Where(i => i.Status == Status.Recd && i.Operation == Operation.Done);
            //// all items with status send and all operations done
            //var itemsSend = allItems.Where(i => i.Status == Status.Send && i.Operation == Operation.Done);

            //if (itemsRecd.Count() == allItems.Count())
            //    return Status.Recd;
            //else if (itemsSend.Count() == allItems.Count())
            //    return Status.Send;
            //else return Status.New;

            return Status.New;
        }

        public XMLDispatch Update(XMLDispatch item)
        {
            throw new NotImplementedException();
        }
    }
}

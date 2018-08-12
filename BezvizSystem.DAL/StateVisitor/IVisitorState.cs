using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.StateVisitor
{
    public interface IVisitorState
    {

        Status Status { get; set; }
        Operation Operation { get; set; }

        void Remove(Visitor visitor);
        void Edit(Visitor visitor);
        void Send(Visitor visitor);
        void Recd(Visitor visitor);
    }

    public class NewVisitorState : IVisitorState
    {
        public Status Status { get; set; }
        public Operation Operation { get; set; }

        public NewVisitorState(Status status, Operation operation)
        {
            Status = status;
            Operation = operation;
        }

        public void Edit(Visitor visitor)
        {

        }

        public void Remove(Visitor visitor)
        {

        }

        public void Send(Visitor visitor)
        {
            Status = Status.Send;
            Operation = Operation.Done;
            visitor.State = new SendVisitorState(Status, Operation);
        }

        public void Recd(Visitor visitor)
        {

        }
    }

    public class SendVisitorState : IVisitorState
    {
        public Status Status { get; set; }
        public Operation Operation { get; set; }

        public SendVisitorState(Status status, Operation operation)
        {
            Status = status;
            Operation = operation;
        }

        public void Edit(Visitor visitor)
        {
            Status = Status.Send;
            Operation = Operation.Edit;
        }

        public void Remove(Visitor visitor)
        {
            Status = Status.Send;
            Operation = Operation.Remove;
        }

        public void Send(Visitor visitor)
        {

        }

        public void Recd(Visitor visitor)
        {
            Status = Status.Recd;
            Operation = Operation.Done;
            visitor.State = new RecdVisitorState(Status, Operation);
        }      
    }

    public class RecdVisitorState : IVisitorState
    {

        public Status Status { get; set; }
        public Operation Operation { get; set; }

        public RecdVisitorState(Status status, Operation operation)
        {
            Status = status;
            Operation = operation;
        }

        public void Edit(Visitor visitor)
        {
            Status = Status.Recd;
            Operation = Operation.Edit;
        }

        public void Remove(Visitor visitor)
        {
            Status = Status.Recd;
            Operation = Operation.Remove;
        }

        public void Send(Visitor visitor)
        {
            
        }

        public void Recd(Visitor visitor)
        {
          
        }           
    }

}

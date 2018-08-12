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
        void Remove(Visitor visitor);
        void Edit(Visitor visitor);
        void Send(Visitor visitor);
        void Recd(Visitor visitor);
    }

    public class NewVisitorState : IVisitorState
    {
        public void Edit(Visitor visitor)
        {
            visitor.XML.Status = Status.New;
            visitor.XML.Operation = Operation.Add;
        }

        public void Remove(Visitor visitor)
        {
            visitor.XML.Status = Status.New;
            visitor.XML.Operation = Operation.Add;
        }

        public void Send(Visitor visitor)
        {
            visitor.XML.Status = Status.Send;
            visitor.XML.Operation = Operation.Done;
            visitor.State = new SendVisitorState();
        }

        public void Recd(Visitor visitor)
        {

        }
    }

    public class SendVisitorState : IVisitorState
    {
        public void Edit(Visitor visitor)
        {
            visitor.XML.Status = Status.Send;
            visitor.XML.Operation = Operation.Edit;
        }

        public void Remove(Visitor visitor)
        {
            visitor.XML.Status = Status.Send;
            visitor.XML.Operation = Operation.Remove;
        }

        public void Send(Visitor visitor)
        {

        }

        public void Recd(Visitor visitor)
        {
            visitor.XML.Status = Status.Recd;
            visitor.XML.Operation = Operation.Done;
            visitor.State = new RecdVisitorState();
        }      
    }

    public class RecdVisitorState : IVisitorState
    {
        public void Edit(Visitor visitor)
        {
            visitor.XML.Status = Status.Recd;
            visitor.XML.Operation = Operation.Edit;
        }

        public void Remove(Visitor visitor)
        {
            visitor.XML.Status = Status.Recd;
            visitor.XML.Operation = Operation.Remove;
        }

        public void Send(Visitor visitor)
        {
            visitor.XML.Status = Status.Recd;
            visitor.XML.Operation = Operation.Done;
        }

        public void Recd(Visitor visitor)
        {
            visitor.XML.Status = Status.Recd;
            visitor.XML.Operation = Operation.Done;
        }           
    }

}

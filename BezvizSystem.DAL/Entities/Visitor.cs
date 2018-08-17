using BezvizSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities
{
    public class Visitor
    {

        //public Visitor()
        //{
        //    State = new NewVisitorState(Status.New, Operation.Add);
        //}

        public int Id { get; set; }

        public virtual GroupVisitor Group { get; set; }

        //public IVisitorState State { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string SerialAndNumber { get; set; }
        public DateTime? DocValid { get; set; }
        public virtual Gender Gender { get; set; }
        public DateTime? BithDate { get; set; }
        public virtual Nationality Nationality { get; set; }

        public bool Arrived { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Visitor visitor)) return false;

            if (visitor.Id != this.Id) return false;

            //if (visitor.Surname == null && Surname != null) return false;
            //if (visitor.Surname != null && Surname == null) return false;
            //if (visitor.Surname != null && Surname != null)
            //    if (visitor.Surname != Surname) return false;

            //if (visitor.Name == null && Name != null) return false;
            //if (visitor.Name != null && Name == null) return false;
            //if (visitor.Name != null && Name != null)
            //    if (visitor.Name != Name) return false;

            //if (visitor.SerialAndNumber == null && SerialAndNumber != null) return false;
            //if (visitor.SerialAndNumber != null && SerialAndNumber == null) return false;
            //if (visitor.SerialAndNumber != null && SerialAndNumber != null)
            //    if (visitor.SerialAndNumber != SerialAndNumber) return false;


            //if (visitor.Gender == null && Gender != null) return false;
            //if (visitor.Gender != null && Gender == null) return false;
            //if (visitor.Gender != null && Gender != null)
            //    if (visitor.Gender.Name != Gender.Name) return false;

            //if (!visitor.BithDate.HasValue && BithDate.HasValue) return false;
            //if (visitor.BithDate.HasValue && !BithDate.HasValue) return false;
            //if (visitor.BithDate.HasValue && visitor.BithDate.HasValue)
            //    if (visitor.BithDate.Value != BithDate.Value) return false;

            //if (visitor.Nationality == null && Nationality != null) return false;
            //if (visitor.Nationality != null && Nationality == null) return false;
            //if (visitor.Nationality != null && Nationality != null)
            //    if (visitor.Nationality.Name != Nationality.Name) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        //public void Edit()
        //{
        //    State.Edit(this);
        //}

        //public void Remove()
        //{
        //    State.Remove(this);
        //}

        //public void Send()
        //{
        //    State.Send(this);
        //}

        //public void Recd()
        //{
        //    State.Recd(this);
        //}
    }
}

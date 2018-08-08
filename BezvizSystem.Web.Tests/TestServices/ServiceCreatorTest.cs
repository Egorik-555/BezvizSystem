using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.DAL.EF;
using Moq;
using BezvizSystem.BLL.Infrastructure;

namespace BezvizSystem.Web.Tests.TestServices
{
    public class ServiceCreatorTest : IServiceCreator
    {
        public IService<AnketaDTO> CreateAnketaService(string connection)
        {
            throw new NotImplementedException();
        }

        IEnumerable<CheckPointDTO> listCheckPoints;
        CheckPointDTO check1 = new CheckPointDTO { Id = 1, Name = "Check1", Active = true };
        CheckPointDTO check2 = new CheckPointDTO { Id = 2, Name = "Check2", Active = true };
        CheckPointDTO check3 = new CheckPointDTO { Id = 3, Name = "Check3", Active = true };

        IEnumerable<GenderDTO> listGenders;
        GenderDTO genter1 = new GenderDTO { Id = 1, Code = 1, Name = "Мужчина", Active = true };
        GenderDTO genter2 = new GenderDTO { Id = 2, Code = 2, Name = "Женщина", Active = true };


        IEnumerable<NationalityDTO> listNationalities;
        NationalityDTO nat1 = new NationalityDTO { Id = 1, Code = 1, Name = "nat1", ShortName = "n1", Active = true };
        NationalityDTO nat2 = new NationalityDTO { Id = 2, Code = 2, Name = "nat2", ShortName = "n2", Active = true };
        NationalityDTO nat3 = new NationalityDTO { Id = 3, Code = 3, Name = "nat3", ShortName = "n3", Active = true };
        NationalityDTO nat4 = new NationalityDTO { Id = 4, Code = 4, Name = "nat4", ShortName = "n4", Active = true };

        IList<VisitorDTO> listVisitors;
        IList<GroupVisitorDTO> listGroups;

        public IDictionaryService<CheckPointDTO> CreateCheckPointService(string connection)
        {
            listCheckPoints = new List<CheckPointDTO> { check1, check2, check3 };
            Mock<IDictionaryService<CheckPointDTO>> checkPoint = new Mock<IDictionaryService<CheckPointDTO>>();
            checkPoint.Setup(m => m.Get()).Returns(listCheckPoints);

            return checkPoint.Object;
        }

        public IDictionaryService<GenderDTO> CreateGenderService(string connection)
        {
            listGenders = new List<GenderDTO> { genter1, genter2 };
            Mock<IDictionaryService<GenderDTO>> gender = new Mock<IDictionaryService<GenderDTO>>();
            gender.Setup(m => m.Get()).Returns(listGenders);

            return gender.Object;
        }

        public IDictionaryService<NationalityDTO> CreateNationalityService(string connection)
        {
            listNationalities = new List<NationalityDTO> { nat1, nat2, nat3, nat4 };
            Mock<IDictionaryService<NationalityDTO>> gender = new Mock<IDictionaryService<NationalityDTO>>();
            gender.Setup(m => m.Get()).Returns(listNationalities);

            return gender.Object;
        }

        public IService<VisitorDTO> CreateVisitorService(string connection)
        {
            listVisitors = new List<VisitorDTO>();
            Mock<IService<VisitorDTO>> visitor = new Mock<IService<VisitorDTO>>();
            visitor.Setup(m => m.GetAll()).Returns(listVisitors);

            visitor.Setup(m => m.Create(It.IsAny<VisitorDTO>())).Returns<VisitorDTO>(v => Task.FromResult(new Func<OperationDetails>(() =>
            {
                listVisitors.Add(v);
                return new OperationDetails(true, "test create visitor", "");
            }).Invoke()));

            visitor.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id => Task.FromResult(new Func<OperationDetails>(() =>
            {
                var removeVisitor = listVisitors.Where(vis => vis.Id == id).FirstOrDefault();
                if (removeVisitor == null) return new OperationDetails(false, "test visitor not found", "");
                listVisitors.Remove(removeVisitor);
                return new OperationDetails(true, "test delete visito", "");
            }).Invoke()));

            visitor.Setup(m => m.Update(It.IsAny<VisitorDTO>())).Returns<VisitorDTO>(v => Task.FromResult(new Func<OperationDetails>(() =>
            {
                var removeVisitor = listVisitors.Where(vis => vis.Id == v.Id).FirstOrDefault();
                if (removeVisitor == null) return new OperationDetails(false, "test visitor not found", "");
                listVisitors.Remove(removeVisitor);
                listVisitors.Add(v);
                return new OperationDetails(true, "test update visitor", "");

            }).Invoke()));

            return visitor.Object;
        }

        public IService<GroupVisitorDTO> CreateGroupService(string connection)
        {
            listGroups = new List<GroupVisitorDTO>();
            Mock<IService<GroupVisitorDTO>> visitor = new Mock<IService<GroupVisitorDTO>>();
            visitor.Setup(m => m.GetAll()).Returns(listGroups);

            visitor.Setup(m => m.Create(It.IsAny<GroupVisitorDTO>())).Returns<GroupVisitorDTO>(g => Task.FromResult(new Func<OperationDetails>(() =>
            {
                listGroups.Add(g);
                foreach (VisitorDTO v in g.Visitors) listVisitors.Add(v);
                return new OperationDetails(true, "test create group", "");
            }).Invoke()));

            visitor.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id => Task.FromResult(new Func<OperationDetails>(() =>
            {
                var removeGroup = listGroups.Where(g => g.Id == id).FirstOrDefault();
                if (removeGroup == null) return new OperationDetails(false, "test group not found", "");
                foreach (VisitorDTO v in removeGroup.Visitors) listVisitors.Remove(v);
                listGroups.Remove(removeGroup);
                return new OperationDetails(true, "test delete group", "");
            }).Invoke()));

            visitor.Setup(m => m.Update(It.IsAny<GroupVisitorDTO>())).Returns<GroupVisitorDTO>(g => Task.FromResult(new Func<OperationDetails>(() =>
            {
                var removeGroup = listGroups.Where(gr => gr.Id == g.Id).FirstOrDefault();
                if (removeGroup == null) return new OperationDetails(false, "test group not found", "");
                foreach (VisitorDTO v in g.Visitors) listVisitors.Remove(v);
                listGroups.Remove(removeGroup);
                foreach (VisitorDTO v in g.Visitors) listVisitors.Add(v);
                listGroups.Add(g);
                return new OperationDetails(true, "test update group", "");

            }).Invoke()));

            return visitor.Object;
        }

    
        public IReport CreateReport(string connection)
        {
            throw new NotImplementedException();
        }
       
        public IUserService CreateUserService(string connection)
        {
            throw new NotImplementedException();
        }     

        public BezvizContext CreateContext(string connection)
        {
            throw new NotImplementedException();
        }
    }
}

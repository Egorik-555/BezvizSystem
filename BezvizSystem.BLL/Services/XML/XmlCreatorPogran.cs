using AutoMapper;
using BezvizSystem.BLL.DTO.XML;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.BLL.Mapper.XML;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BezvizSystem.BLL.Services.XML
{
    public class XmlCreatorPogran : IXmlCreator
    {
        IUnitOfWork _database;
        IMapper _mapper;

        public XmlCreatorPogran(IUnitOfWork database)
        {
            _database = database;
            _mapper =  new MapperConfiguration(cfg => cfg.AddProfile(new MapperXMLProfile(_database))).CreateMapper();
        }

        private IEnumerable<ModelForXmlToPogran> GetNewItems()
        {
            var visitors = _database.VisitorManager.GetAll().ToList().Where(v => v.Status.Code == 1);
            return _mapper.Map<IEnumerable<Visitor>, IEnumerable<ModelForXmlToPogran>>(visitors);
        }

        private IEnumerable<ModelForXmlToPogran> GetExtraNewItems()
        {
            var visitors = _database.VisitorManager.GetAll().ToList().Where(v => v.Status.Code == 1).Where(v => v.Group.ExtraSend);
            return _mapper.Map<IEnumerable<Visitor>, IEnumerable<ModelForXmlToPogran>>(visitors);
        }

        private void EditStatus(int codeOld, int codeNew)
        {
            var visitors = _database.VisitorManager.GetAll().Where(v => v.Status.Code == codeOld).ToList();

            foreach(var item in visitors)
            {
                var status = _database.StatusManager.GetAll().Where(s => s.Code == codeNew).FirstOrDefault();
                item.Status = status;
                _database.VisitorManager.Update(item);
            }
        }

        private XDocument CreadeDoc(IEnumerable<ModelForXmlToPogran> list)
        {
            XElement form = new XElement("EXPORT",
                                    list.Select(v =>
                                        new XElement("FORM_BORDER_INFORM",
                                            new XElement("SECTION1",
                                                new XElement("ORGANIZATION", v.Organization),
                                                new XElement("TYPE_OPERATION", v.TypeOperation),
                                                new XElement("UNIQUE_ID", v.Id)),

                                            new XElement("SECTION2",
                                                new XElement("LAT_SURNAME", v.Surname),
                                                new XElement("LAT_NAME", v.Name),
                                                new XElement("BITH_DATE", new XElement("DAY", v.DayBith), new XElement("MONTH", v.MonthBith), new XElement("YEAR", v.YearBith)),
                                                new XElement("SEX", new XElement("TEXT_SEX", v.TextSex), new XElement("CODE_SEX", v.CodeSex)),
                                                new XElement("DOC",
                                                    new XElement("DOC_NUM", v.SerialAndNumber),
                                                    new XElement("DOC_VALID", new XElement("DAY", v.DayValid), new XElement("MONTH", v.MonthValid), new XElement("YEAR", v.YearValid))),
                                                new XElement("AUTHORIZED_DAY", v.DayOfStay),
                                                new XElement("DATE_ENTRY", new XElement("DAY", v.DayArrival), new XElement("MONTH", v.MonthArrival), new XElement("YEAR", v.YearArrival))
                                            )
                                        )));

            return new XDocument(form);
        }

        public OperationDetails SaveNew(string name, SaveOptions options)
        {                   
            try
            {
                var list = GetNewItems();
                if (list.Count() == 0)
                    return new OperationDetails(false, "Нет записей для выгрузки", "");

                XDocument xDoc = CreadeDoc(list);
                xDoc.Save(name, options);
                //mark item unloaded
                EditStatus(codeOld: 1, codeNew: 2);
           
                return new OperationDetails(true, "XML файл создан", "");
            }
            catch(Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public OperationDetails SaveExtra(string name, SaveOptions options)
        {
            try
            {
                var list = GetExtraNewItems();
                if (list.Count() == 0)
                    return new OperationDetails(false, "Нет записей для выгрузки", "");

                XDocument xDoc = CreadeDoc(list);
                xDoc.Save(name, options);
                //mark item unloaded
                EditStatus(codeOld: 1, codeNew: 2);

                return new OperationDetails(true, "XML файл создан", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public OperationDetails SaveNew(string name)
        {
            return SaveNew(name, SaveOptions.None);
        }
        public OperationDetails SaveExtra(string name)
        {
            return SaveExtra(name, SaveOptions.None);
        }
    }
}

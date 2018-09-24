using AutoMapper;
using BezvizSystem.BLL.DTO.XML;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.BLL.Mapper.XML;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
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
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MapperXMLProfile(_database))).CreateMapper();
        }

        private void AddToListRemovedItems(List<ModelForXmlToPogran> list)
        {
            var removed = _database.XMLDispatchManager.GetAll().Where(x => x.Operation == Operation.Remove);
            foreach (var item in removed)
            {
                ModelForXmlToPogran xml = new ModelForXmlToPogran
                {
                    Organization = 1,
                    TypeOperation = (int)Operation.Remove - 1,
                    Id = item.Id
                };
                list.Add(xml);
            }
        }

        private IEnumerable<ModelForXmlToPogran> GetItems()
        {
            List<ModelForXmlToPogran> visitors;
            try
            {
                 visitors = _database.VisitorManager.GetAll().ToList().Join(_database.XMLDispatchManager.GetAll().ToList(),
                    v => v.Id,
                    x => x.Id,
                    (v, x) => new ModelForXmlToPogran
                    {
                        Organization = 1,
                        StatusOperation = (int)x.Status,
                        TypeOperation = (int)x.Operation - 1,
                        ExtraSend = v.Group.ExtraSend,

                        Id = v.Id,
                        Surname = v.Surname,
                        Name = v.Name,
                        DayBith = v.BithDate.HasValue ? v.BithDate.Value.Day.ToString() : null,
                        MonthBith = v.BithDate.HasValue ? v.BithDate.Value.Month.ToString() : null,
                        YearBith = v.BithDate.HasValue ? v.BithDate.Value.Year.ToString() : null,
                        TextSex = v.Gender?.Name,
                        CodeSex = v.Gender?.Code,

                        SerialAndNumber = v.SerialAndNumber,
                        DayValid = v.DocValid.HasValue ? v.DocValid.Value.Day.ToString() : null,
                        MonthValid = v.DocValid.HasValue ? v.DocValid.Value.Month.ToString() : null,
                        YearValid = v.DocValid.HasValue ? v.DocValid.Value.Year.ToString() : null,

                        DayOfStay = v.Group.DaysOfStay,
                        DayArrival = v.Group.DateArrival.HasValue ? v.Group.DateArrival.Value.Day.ToString() : null,
                        MonthArrival = v.Group.DateArrival.HasValue ? v.Group.DateArrival.Value.Month.ToString() : null,
                        YearArrival = v.Group.DateArrival.HasValue ? v.Group.DateArrival.Value.Year.ToString() : null
                    }).ToList();

                AddToListRemovedItems(visitors);
            }
            catch (Exception ex)
            {
                visitors = new List<ModelForXmlToPogran>();
            }

            return visitors;
        }   

        private async Task SendItems(IEnumerable<ModelForXmlToPogran> list)
        {        
            foreach (var item in list)
            {
                var xml = await _database.XMLDispatchManager.GetByIdAsync(item.Id);

                if(xml != null)
                {
                    xml.Status = Status.Send;
                    xml.Operation = Operation.Done;
                    _database.XMLDispatchManager.Update(xml);
                }             
            }
        }

        private XDocument CreateDoc(IEnumerable<ModelForXmlToPogran> list)
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

        public async Task<OperationDetails> SaveNew(string name, SaveOptions options)
        {
            try
            {
                var list = GetItems().Where(x => x.TypeOperation == 1 || x.TypeOperation == 2 || x.TypeOperation == 3);
                if (list.Count() == 0)
                    return new OperationDetails(false, "Нет анкет для выгрузки", "");

                XDocument xDoc = CreateDoc(list);
                xDoc.Save(name, options);
                //mark item unloaded
                await SendItems(list);

                return new OperationDetails(true, "XML файл создан", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, "Не удалось выгрузить анкеты. " + ex.Message, "");
            }
        }

        public async Task<OperationDetails> SaveExtra(string name, SaveOptions options)
        {
            try
            {
                var list = GetItems().Where(x => (x.TypeOperation == 1 || x.TypeOperation == 2 || x.TypeOperation == 3) && x.ExtraSend);
                if (list.Count() == 0)
                    return new OperationDetails(false, "Нет анкет для выгрузки", "");

                XDocument xDoc = CreateDoc(list);
                xDoc.Save(name, options);
                //mark item unloaded
                await SendItems(list);

                return new OperationDetails(true, "XML файл создан", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, "Не удалось выгрузить анкеты. " + ex.Message, "");
            }
        }

        public async Task<OperationDetails> SaveNew(string name)
        {
            return await SaveNew(name, SaveOptions.None);
        }
        public async Task<OperationDetails> SaveExtra(string name)
        {
            return await SaveExtra(name, SaveOptions.None);
        }
    }
}

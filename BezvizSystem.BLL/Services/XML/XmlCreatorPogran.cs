using AutoMapper;
using BezvizSystem.BLL.DTO.XML;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.BLL.Mapper.XML;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
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

        private IEnumerable<ModelForXmlToPogran> GetRemovedItems()
        {
            var removed = _database.XMLDispatchManager.GetAll().Where(x => x.Operation == Operation.Remove);

            return removed.Select(x => new ModelForXmlToPogran { Organization = 1, TypeOperation = (int)Operation.Remove - 1, Id = x.Id });
        }

        private string GetNumberWithZero(int a)
        {
            if (a <= 9) return "0" + a.ToString();

            return a.ToString();
        }

        private IEnumerable<ModelForXmlToPogran> GetItems()
        {
            IEnumerable<ModelForXmlToPogran> visitors;
            try
            {
                visitors = _database.VisitorManager.GetAll().ToList().Join(_database.XMLDispatchManager.GetAll(),
                   v => v.Id,
                   x => x.Id,
                   (v, x) => new ModelForXmlToPogran
                   {
                       Organization = 1,
                       StatusOperation = (int)x.Status,
                       TypeOperation = (int)x.Operation - 1,
                       ExtraSend = v.Group.ExtraSend,

                       Id = v.Id,
                       IdGroup = v.Group.Id,

                       Surname = v.Surname,
                       Name = v.Name,
                       DayBith = v.BithDate.HasValue ? GetNumberWithZero(v.BithDate.Value.Day) : null,
                       MonthBith = v.BithDate.HasValue ? GetNumberWithZero(v.BithDate.Value.Month) : null,
                       YearBith = v.BithDate.HasValue ? v.BithDate.Value.Year.ToString() : null,
                       TextSex = v.Gender?.Name,
                       CodeSex = v.Gender?.Code,

                       SerialAndNumber = v.SerialAndNumber,
                       DayValid = v.DocValid.HasValue ? GetNumberWithZero(v.DocValid.Value.Day) : null,
                       MonthValid = v.DocValid.HasValue ? GetNumberWithZero(v.DocValid.Value.Month) : null,
                       YearValid = v.DocValid.HasValue ? v.DocValid.Value.Year.ToString() : null,

                       DayOfStay = v.Group.DaysOfStay,
                       DayArrival = v.Group.DateArrival.HasValue ? GetNumberWithZero(v.Group.DateArrival.Value.Day) : null,
                       MonthArrival = v.Group.DateArrival.HasValue ? GetNumberWithZero(v.Group.DateArrival.Value.Month) : null,
                       YearArrival = v.Group.DateArrival.HasValue ? v.Group.DateArrival.Value.Year.ToString() : null
                   }).Where(x => x.TypeOperation == 1 || x.TypeOperation == 2 || x.TypeOperation == 3).ToList();
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

                if (xml != null)
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
                                                new XElement("LAT_SURNAME", v.Surname?.ToUpper()),
                                                new XElement("LAT_NAME", v.Name?.ToUpper()),
                                                new XElement("BIRTH_DATE", new XElement("DAY", v.DayBith), new XElement("MONTH", v.MonthBith), new XElement("YEAR", v.YearBith)),
                                                new XElement("SEX", new XElement("TEXT_SEX", v.TextSex), new XElement("CODE_SEX", v.CodeSex)),
                                                new XElement("DOC",
                                                    new XElement("DOC_NUM", v.SerialAndNumber?.ToUpper()),
                                                    new XElement("DOC_VALID", new XElement("DAY", v.DayValid), new XElement("MONTH", v.MonthValid), new XElement("YEAR", v.YearValid))),
                                                new XElement("AUTHORIZED_DAY", v.DayOfStay),
                                                new XElement("DATE_ENTRY", new XElement("DAY", v.DayArrival), new XElement("MONTH", v.MonthArrival), new XElement("YEAR", v.YearArrival))
                                            )
                                        )));

            return new XDocument(form);
        }

        private void SaveFile(string name, IEnumerable<ModelForXmlToPogran> list, SaveOptions options)
        {
            XDocument xDoc = CreateDoc(list);
            xDoc.Save(name, options);
        }

        public async Task<OperationDetails> SaveNew(string name, SaveOptions options)
        {
            try
            {
                //записи для отправки
                var list = GetItems().ToList();
                //записи для удаления
                var list1 = GetRemovedItems();

                list.AddRange(list1);

                if (list.Count() == 0)
                    return new OperationDetails(false, "Нет анкет для выгрузки", "");

                SaveFile(name, list, options);

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
                var list = GetItems().Where(x => x.ExtraSend);

                if (list.Count() == 0)
                    return new OperationDetails(false, "Нет анкет для выгрузки", "");

                SaveFile(name, list, options);
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

        public int Count()
        {

            return _database.XMLDispatchManager.GetAll().Count(x => x.Operation == Operation.Add || x.Operation == Operation.Edit || x.Operation == Operation.Remove);
                //GetItems().Count();
        }

        public int ExtraCount()
        {
            return 0;//GetItems().Count(x => x.ExtraSend);
        }
    }
}

using AutoMapper;
using BezvizSystem.BLL.DTO.XML;
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
            MapperConfiguration config = new MapperConfiguration(cfg => 
                {
                    //cfg.AddProfile<MapperXMLProfile>();
                    cfg.AddProfile(new MapperXMLProfile(_database));
                });

            _mapper = config.CreateMapper();
        }


        private IEnumerable<ModelForXmlToPogran> GetNewItems()
        {
            var visitors = _database.VisitorManager.GetAll().Where(v => v.Group == null ? false : v.Group.Status.Code == 1).ToList();
            return _mapper.Map<IEnumerable<Visitor>,IEnumerable<ModelForXmlToPogran>>(visitors);
        }

        public void Save(string name, SaveOptions options)
        {
            var list = GetNewItems();

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
                                            new XElement("BITH_DATE", new XElement("DAY", v.DayBith), new XElement("MONTH", v.MonthBith), new XElement("YEAR",  v.YearBith)),
                                            new XElement("SEX", new XElement("TEXT_SEX", v.TextSex), new XElement("CODE_SEX", v.CodeSex)),
                                            new XElement("DOC", 
                                                new XElement("DOC_NUM", v.SerialAndNumber),
                                                new XElement("DOC_VALID", new XElement("DAY", v.DayValid), new XElement("MONTH", v.MonthValid), new XElement("YEAR", v.YearValid))),
                                            new XElement("AUTHORIZED_DAY", v.DayOfStay),
                                            new XElement("DATE_ENTRY", new XElement("DAY", v.DayArrival), new XElement("MONTH", v.MonthArrival), new XElement("YEAR", v.YearArrival))
                                        )
                                    )));

            XDocument xDoc = new XDocument(form);
            xDoc.Save(name, options);
        }

        public void Save(string name)
        {
            Save(name, SaveOptions.None);
        }
    }
}

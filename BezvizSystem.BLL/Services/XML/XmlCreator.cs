using BezvizSystem.BLL.Interfaces.XML;
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

        public XmlCreatorPogran(IUnitOfWork database)
        {
            _database = database;
        }


        private IEnumerable<>

        public void Save(string name, SaveOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

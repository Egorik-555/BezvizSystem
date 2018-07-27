using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces.Docs
{
    public interface IDocCreator
    {
        OperationDetails Save(string name);
    }
}

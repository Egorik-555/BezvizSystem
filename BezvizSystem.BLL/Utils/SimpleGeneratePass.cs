using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Utils
{
    public class SimpleGeneratePass : IGeneratePass
    {
        const int length = 6;

        public string Generate()
        {
            string pass = string.Empty;
            var r = new Random();
            while (pass.Length < 6)
            {
                char c = (Char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }

            return pass;
        }
    }
}

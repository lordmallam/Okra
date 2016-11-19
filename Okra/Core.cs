using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Core
{
    public class Core
    {
        public Core()
        {

        }

        public long extractInteger(string input)
        {
            long result = 0;
            string c = "";
            foreach (var item in input)
            {
                if (isInteger(item.ToString()))
                {
                    c += item.ToString();
                }
            }
            if (c != "")
            {
                result = Convert.ToInt64(c);
            }            

            return result;
        }

        public bool isInteger(string x)
        {
            bool result = false;
            try
            {
                int.Parse(x);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}

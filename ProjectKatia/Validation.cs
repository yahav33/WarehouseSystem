using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKatia
{
    class Validation
    {
        public static bool checkIfValidDouble(string temp)
        {
            double x;
            if (double.TryParse(temp, out x)&& x > 0) return true;
            return false;        
        }
        public static bool checkIfValidInt(string temp)
        {
            int x;
            if (int.TryParse(temp, out x) && x > 0) return true;
            return false;
        }

     
    }
}

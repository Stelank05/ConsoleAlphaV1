using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Alpha_V1
{
    public class Manufacturer
    {
        string manufacturerName, bestResult, bestResultString;
        int points, rawBestResult;

        List<string> pastResults;
        List<int> rawResults;

        public Manufacturer(string mN)
        {
            manufacturerName = mN;

            bestResult = "";
            
            pastResults = new List<string>();
            rawResults = new List<int>();
        }
    }
}

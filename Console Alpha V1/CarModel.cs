using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Alpha_V1
{
    public class CarModel
    {
        string carModelName, manufacturerName, platformName;
        int mainOVR, backupOVR, bopValue, reliability;

        Class memberClass;

        public CarModel(string cMN, string mN, string pN, int ovr, int bop, int r, Class mC)
        {
            carModelName = cMN;
            manufacturerName = mN;
            platformName = pN;

            mainOVR = ovr + bop;
            backupOVR = ovr + bop;
            bopValue = bop;

            reliability = r;

            memberClass = mC;
        }

        public CarModel() { }

        public string GetModelName()
        {
            return carModelName;
        }

        public string GetManufacturer()
        {
            return manufacturerName;
        }

        public string GetPlatform()
        {
            return platformName;
        }

        public int GetMainOVR()
        {
            return mainOVR;
        }

        public int GetBackupOVR()
        {
            return backupOVR;
        }

        public int GetBOPValue()
        {
            return bopValue;
        }

        public int GetReliability()
        {
            return reliability;
        }
    }
}

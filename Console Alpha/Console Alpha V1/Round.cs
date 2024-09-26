using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Alpha_V1
{
    public class Round
    {
        string roundName, lengthType, pointsSystem, folderName;
        int raceLength, incidentRange, dnfRate;

        List<string> classesLong, classesNamed;

        Series memberSeries;

        public Round(string rN, int rL, string lT, int iR, int dR, string pS, List<string> racingClasses, Series mS)
        {
            roundName = rN;
            raceLength = rL;
            lengthType = lT;
            incidentRange = iR;
            dnfRate = dR;
            pointsSystem = pS;

            memberSeries = mS;

            LoadClasses(racingClasses);
        }

        public void LoadClasses(List<string> racingClasses)
        {
            classesLong = new List<string>();
            classesNamed = new List<string>();

            for (int i = 0; i < racingClasses.Count; i++)
            {
                if (racingClasses[i] != "")
                {
                    classesLong.Add(racingClasses[i].Replace("C", "Class "));

                    classesNamed.Add(memberSeries.GetClassList()[Convert.ToInt32(racingClasses[i].Replace("C", "")) - 1].GetClassName());
                }
            }
        }

        public string GetRoundName()
        {
            return roundName;
        }

        public string GetLengthType()
        {
            return lengthType;
        }

        public void SetFolder(string newFolder)
        {
            folderName = newFolder;
        }

        public string GetFolder()
        {
            return folderName;
        }

        public List<string> GetLongRacingClasses()
        {
            return classesLong;
        }

        public List<string> GetNamedClasses()
        {
            return classesNamed;
        }

        public int GetRaceLength()
        {
            if (lengthType == "WEC")
            {
                return raceLength * 2;
            }

            else
            {
                return raceLength;
            }
        }

        public int GetDefaultIncidentRate()
        {
            return incidentRange;
        }

        public int GetDefaultDNFRate()
        {
            return dnfRate;
        }

        public string GetPointsSystem()
        {
            return pointsSystem;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Alpha_V1
{
    public class Round
    {
        string roundName, lengthType, pointsSystem;
        int raceLength, incidentRange, dnfRate;

        List<string> classesLong = new List<string>();

        public Round(string RN, int RL, string LT, int IR, int DR, string PS, List<string> RacingClasses)
        {
            roundName = RN;
            raceLength = RL;
            lengthType = LT;
            incidentRange = IR;
            dnfRate = DR;
            pointsSystem = PS;

            LoadClasses(RacingClasses);
        }

        public void LoadClasses(List<string> RacingClasses)
        {
            for (int i = 0; i < RacingClasses.Count; i++)
            {
                classesLong.Add(RacingClasses[i].Replace("C", "Class "));
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

        public List<string> GetLongRacingClasses()
        {
            return classesLong;
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

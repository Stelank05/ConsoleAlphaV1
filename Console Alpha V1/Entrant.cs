using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Alpha_V1
{
    public class Entrant
    {
        string carNo, teamName, manufacturer, currentPositionOverall, currentPositionClass, standingsPosition;
        int mainOVR, baseOVR, stintRangeModifier, isRacingIndex,
            reliability, dnfScore, baseReliability, baseDNFScore,
            lastStint, stintsInGarage, totalStintsInGarage, totalStaysInGarage,
            points, index, stintsSincePit = 0, totalStops;
        bool isRacing, inGarage = false, extendedGarageStay = false;

        CarModel carModel;
        Class memberClass;

        public Entrant(string cN, string tN, int ovr, int srm, int r, int i, CarModel cM, Class mC)
        {
            carNo = cN;
            teamName = tN;
            manufacturer = cM.GetManufacturer();

            mainOVR = ovr + cM.GetMainOVR();
            baseOVR = ovr + cM.GetMainOVR();
            
            baseReliability = r + cM.GetReliability() + mC.GetIRM();
            baseDNFScore = mC.GetDNFRM();

            stintRangeModifier = srm;
            index = i;

            carModel = cM;
            memberClass = mC;
        }

        public Entrant(string cN, string tN, int ovr, int srm, int r, CarModel cM, Class mC)
        {
            carNo = cN;
            teamName = tN;
            manufacturer = cM.GetManufacturer();

            mainOVR = ovr + cM.GetMainOVR();
            baseOVR = ovr + cM.GetMainOVR();

            baseReliability = r + cM.GetReliability() + mC.GetIRM();
            baseDNFScore = mC.GetDNFRM();

            stintRangeModifier = srm;
            
            carModel = cM;
            memberClass = mC;
        }

        public Entrant() { }

        public void SetRound(Round currentRound)
        {
            mainOVR = baseOVR;
            reliability = baseReliability + currentRound.GetDefaultIncidentRate();
            dnfScore = baseDNFScore + currentRound.GetDefaultDNFRate();

            lastStint = 0;

            stintsInGarage = 0;
            totalStintsInGarage = 0;
            totalStaysInGarage = 0;

            stintsSincePit = 0;
            totalStops = 0;

            inGarage = false;
            extendedGarageStay = false;

            //Console.WriteLine("{0} - {1}", carNo, mainOVR);
        }

        public void SetRacing(bool newRacing)
        {
            mainOVR = baseOVR;

            isRacing = newRacing;
            isRacingIndex = 1;

            if (!newRacing)
            {
                isRacingIndex = 2;
            }
        }

        public bool GetRacing()
        {
            return isRacing;
        }

        public int GetRacingIndex()
        {
            return isRacingIndex;
        }

        public void SetIndex(int newIndex)
        {
            index = newIndex;
        }

        public Class GetClass()
        {
            return memberClass;
        }

        public CarModel GetCarModel()
        {
            return carModel;
        }

        public string GetCarNo()
        {
            return carNo;
        }

        public string GetTeamName()
        {
            return teamName;
        }

        public string GetManufacturer()
        {
            return manufacturer;
        }

        public void SetCurrentPositions(string overallPosition, string classPosition)
        {
            currentPositionOverall = overallPosition;
            currentPositionClass = classPosition;
        }

        public (string, string) GetCurrentPosition()
        {
            return (currentPositionOverall, currentPositionClass);
        }

        public void SetStandingsPosition(string newPosition)
        {
            standingsPosition = newPosition;
        }

        public string GetStandingsPosition()
        {
            return standingsPosition;
        }

        public string GetCarAsWriteString()
        {
            return carNo + " " + teamName + "," + carModel.GetModelName() + ",," + mainOVR;
        }

        public void AddToOVR(int AddValue)
        {
            mainOVR += AddValue;
        }

        public void SetGridOVR(int GridSpacing)
        {
            mainOVR = baseOVR + GridSpacing;
        }

        public void UpdateOVR(int NewOVR)
        {
            mainOVR = NewOVR;
        }

        public int GetOVR()
        {
            return mainOVR;
        }

        public int GetBaseOVR()
        {
            return baseOVR;
        }

        public int GetSRM()
        {
            return stintRangeModifier;
        }

        public int GetReliability()
        {
            return reliability;
        }

        public int GetDNF()
        {
            return dnfScore;
        }

        public int GetClassIndex()
        {
            return memberClass.GetClassIndex();
        }

        public void SetLastStint(int Stint)
        {
            lastStint = Stint;
        }

        public int GetLastStint()
        {
            return lastStint;
        }

        public bool GetInGarage()
        {
            return inGarage;
        }

        public void EnterGarage(Random Rand)
        {
            inGarage = true;

            stintsInGarage++;
            totalStintsInGarage++;
            stintsSincePit = 0;
            totalStaysInGarage++;

            Pit();

            if (Rand.Next(1, 11) == 1)
            {
                extendedGarageStay = true;
            }
        }

        public void LeaveGarage()
        {
            inGarage = false;
            stintsInGarage = 0;
        }

        public void StintInGarage()
        {
            stintsInGarage++;
            totalStintsInGarage++;
        }

        public int GetStintsInGarage()
        {
            return stintsInGarage;
        }

        public int GetTotalStintsInGarage()
        {
            return totalStintsInGarage;
        }

        public void SetPoints(int PTS)
        {
            points = PTS;
        }

        public int GetPoints()
        {
            return points;
        }

        public int GetIndex()
        {
            return index;
        }

        public void Pit()
        {
            stintsSincePit = 0;
            totalStops++;
        }

        public void NotPit()
        {
            stintsSincePit++;
        }

        public int GetStintsSincePit()
        {
            return stintsSincePit;
        }

        public int GetTotalStops()
        {
            return totalStops;
        }

        public bool GetExtendedGarageStay()
        {
            return extendedGarageStay;
        }

        public int GetTotalStaysInGarage()
        {
            return totalStaysInGarage;
        }
    }
}

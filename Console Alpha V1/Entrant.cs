using System;
using System.Collections.Generic;

namespace Console_Alpha_V1
{
    public class Entrant
    {
        string carNo, teamName, currentPositionOverall, currentPositionClass, standingsPosition, bestResult, bestResultString;
        int mainOVR, baseOVR, teamOVR, crewOVR, stintRangeModifier, isRacingIndex,
            reliability, dnfScore, baseCrewReliability, baseReliability, baseDNFScore,
            lastStint, stintsInGarage, totalStintsInGarage, totalStaysInGarage,
            points, currentRawPosition, timesFinished, index, seriesIndex, stintsSincePit = 0, totalStops;
        bool isRacing, inGarage = false, extendedGarageStay = false;

        List<string> pastResults;
        List<int> rawResults;

        CarModel carModel;
        Class memberClass;

        public Entrant(string cN, string tN, int tOVR, int cOVR, int srm, int r, int i, int sI, CarModel cM, Class mC)
        {
            carNo = cN;
            teamName = tN;

            mainOVR = tOVR + cOVR + cM.GetMainOVR();
            baseOVR = tOVR + cOVR + cM.GetMainOVR();

            teamOVR = tOVR;
            crewOVR = cOVR;

            baseCrewReliability = r;
            baseReliability = r + cM.GetReliability() + mC.GetIRM();
            baseDNFScore = mC.GetDNFRM();

            stintRangeModifier = srm;
            index = i;
            seriesIndex = sI;

            bestResult = "";
            timesFinished = 0;
            pastResults = new List<string>();
            rawResults = new List<int>();

            carModel = cM;
            memberClass = mC;
        }

        public Entrant(string cN, string tN, int tOVR, int cOVR, int srm, int r, int sI, CarModel cM, Class mC)
        {
            carNo = cN;
            teamName = tN;
            
            mainOVR = tOVR + cOVR + cM.GetMainOVR();
            baseOVR = tOVR + cOVR + cM.GetMainOVR();

            teamOVR = tOVR;
            crewOVR = cOVR;

            baseCrewReliability = r;
            baseReliability = r + cM.GetReliability() + mC.GetIRM();
            baseDNFScore = mC.GetDNFRM();

            stintRangeModifier = srm;

            bestResult = "";
            timesFinished = 0;
            pastResults = new List<string>();
            rawResults = new List<int>();

            carModel = cM;
            memberClass = mC;
        }

        public Entrant() { }


        // Round Details

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


        // Entrant Details

        public void SetIndex(int newIndex)
        {
            index = newIndex;
        }

        public void SetClass(Class newClass)
        {
            memberClass = newClass;

            baseReliability = baseCrewReliability + carModel.GetReliability() + memberClass.GetIRM();
            baseDNFScore = memberClass.GetDNFRM();
        }

        public Class GetClass()
        {
            return memberClass;
        }

        public int GetSeriesIndex()
        {
            return seriesIndex;
        }

        public void SetCarModel(CarModel newCarModel)
        {
            carModel = newCarModel;
            baseReliability = baseCrewReliability + carModel.GetReliability() + memberClass.GetIRM();
        }

        public CarModel GetCarModel()
        {
            return carModel;
        }

        public void SetCarNumber(string newCarNumber)
        {
            carNo = newCarNumber;
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
            return carModel.GetManufacturer();
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

        public int GetTeamOVR()
        {
            return teamOVR;
        }

        public void SetCrewOVR(int newOVR)
        {
            crewOVR = newOVR;
        }

        public int GetCrewOVR()
        {
            return crewOVR;
        }

        public void SetSRM(int newSRM)
        {
            stintRangeModifier = newSRM;
        }

        public int GetSRM()
        {
            return stintRangeModifier;
        }

        public int GetReliability()
        {
            return reliability;
        }

        public void SetBaseReliability(int newBaseReliability)
        {
            baseCrewReliability = newBaseReliability;
            baseReliability = newBaseReliability + carModel.GetReliability() + memberClass.GetIRM();
        }

        public int GetBaseReliability()
        {
            return baseCrewReliability;
        }

        public int GetDNF()
        {
            return dnfScore;
        }

        public int GetIndex()
        {
            return index;
        }

        public int GetClassIndex()
        {
            return memberClass.GetClassIndex();
        }


        // Positions + Standings

        public void SetPoints(int PTS)
        {
            points = PTS;
        }

        public int GetPoints()
        {
            return points;
        }

        public void SetCurrentPositions(string overallPosition, string classPosition, int rawPosition)
        {
            currentPositionOverall = overallPosition;
            currentPositionClass = classPosition;
            currentRawPosition = rawPosition;
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

        public void ResetPastResults()
        {
            pastResults.Clear();
            rawResults.Clear();
            bestResult = "";
            bestResultString = "";
        }

        public void AddResult()
        {
            pastResults.Add(currentPositionClass);

            if (currentPositionClass == "NC")
            {
                rawResults.Add(currentRawPosition + 100);
            }

            else if (currentPositionClass == "DNF")
            {
                rawResults.Add(currentRawPosition + 200);
            }

            else
            {
                rawResults.Add(currentRawPosition);
            }

            SetBestResult();
        }

        public List<string> GetPastResults()
        {
            return pastResults;
        }

        public List<int> GetRawResults()
        {
            return rawResults;
        }

        private void SetBestResult()
        {
            bool bestFinish = bestResult == "DNF" || bestResult == "NC",
                currentFinish = currentPositionClass.StartsWith("P");

            if (bestResult == "")
            {
                bestResult = currentPositionClass;
                timesFinished = 1;
            }

            else if (bestResult == currentPositionClass)
            {
                timesFinished++;
            }

            else if (bestFinish && currentFinish)
            {
                bestResult = currentPositionClass;
                timesFinished = 1;
            }

            else if (bestFinish && !currentFinish)
            {
                if (bestResult == "NC" && currentPositionClass == "NC")
                {
                    timesFinished++;
                }

                else if (bestResult == "DNF" && currentPositionClass == "NC")
                {
                    bestResult = currentPositionClass;
                    timesFinished = 1;
                }
            }

            else if (currentFinish)
            {
                int iBestResult = Convert.ToInt32(bestResult.Replace("P", "")),
                    iNewResult = Convert.ToInt32(currentPositionClass.Replace("P", ""));

                if (iNewResult < iBestResult)
                {
                    bestResult = currentPositionClass;
                    timesFinished = 1;
                }
            }

            // Set Standings Best Result String

            if (bestResult == "P1")
            {
                bestResultString = string.Format("{0} Win", timesFinished);

                if (timesFinished > 1)
                {
                    bestResultString += "s";
                }
            }

            else
            {
                bestResultString = string.Format("Best Result of {0} {1} Time", string.Format("{0},", bestResult).PadRight(4, ' '), timesFinished);

                if (timesFinished > 1)
                {
                    bestResultString += "s";
                }
            }
        }

        public string GetBestResult()
        {
            return bestResultString;
        }


        // Race Stuffs

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

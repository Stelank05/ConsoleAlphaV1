using System;
using System.Collections.Generic;

namespace Console_Alpha_V1
{
    public class Manufacturer
    {
        string manufacturerName, bestResult, bestResultString;
        int points, timesFinished;

        List<string> pastResults;
        List<int> rawResults;

        public Manufacturer(string mN)
        {
            manufacturerName = mN;

            points = 0;

            bestResult = "";
            timesFinished = 0;
            
            pastResults = new List<string>();
            rawResults = new List<int>();
        }

        public string GetManufacturerName()
        {
            return manufacturerName;
        }

        public void AddResult(string newResult, int rawNewResult)
        {
            pastResults.Add(newResult);

            if (newResult == "NC")
            {
                rawResults.Add(rawNewResult + 100);
            }

            else if (newResult == "DNF")
            {
                rawResults.Add(rawNewResult + 200);
            }

            else
            {
                rawResults.Add(rawNewResult);
            }

            SetBestResult(newResult);
        }

        private void SetBestResult(string currentPositionClass)
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

        public List<int> GetResults()
        {
            return rawResults;
        }

        public void AddPoints(int scoredPoints)
        {
            points += scoredPoints;
        }

        public int GetPoints()
        {
            return points;
        }
    }
}

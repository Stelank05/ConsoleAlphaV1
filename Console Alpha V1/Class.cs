using System.Collections.Generic;
using System.Linq;

namespace Console_Alpha_V1
{
    public class Class
    {
        string className;
        int incidentRangeModifier, dnfRateModifier,
            stintRangeHigh, stintRangeLow, stintRangeIncident,
            classIndex,
            minOVR, maxOVR,
            wecDistanceToPit, imsaDistanceToPit, lapDistanceToPit,
            wecMinStintScore, wecMaxStintScore, wecGarageStintScore,
            imsaMinStintScore, imsaMaxStintScore, imsaGarageStintScore,
            lapMinStintScore, lapMaxStintScore, lapGarageStintScore;

        List<string> eligiblePlatforms;

        List<Manufacturer> manufacturerList;

        public Class(string cN, int iRM, int dnfScore, int sRH, int sRL, int sRI, int minO, int maxO, int wecDTP, int imsaDTP, int lapDTP, int cI, List<string> eP)
        {
            className = cN;

            incidentRangeModifier = iRM;
            dnfRateModifier = dnfScore;

            stintRangeHigh = sRH;
            stintRangeLow = sRL;
            stintRangeIncident = sRI;

            classIndex = cI;

            minOVR = minO;
            maxOVR = maxO;

            wecDistanceToPit = wecDTP;
            imsaDistanceToPit = imsaDTP;
            lapDistanceToPit = lapDTP;

            eligiblePlatforms = eP;

            manufacturerList = new List<Manufacturer>();
        }

        public Class() { }

        public void SetManufacturerList(List<string> manufacturers)
        {
            manufacturerList.Clear();

            foreach (string manufacturer in manufacturers)
            {
                manufacturerList.Add(new Manufacturer(manufacturer));
            }
        }

        public List<Manufacturer> GetManufacturerList()
        {
            return manufacturerList;
        }

        public int GetManufacturerIndex(string targetManufacturer)
        {
            for (int i = 0; i < manufacturerList.Count(); i++)
            {
                if (manufacturerList[i].GetManufacturerName() == targetManufacturer)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SortStandings()
        {
            bool swap;

            int roundIndex = 0;
            List<int> driver1Results, driver2Results;

            for (int i = 0; i < manufacturerList.Count() - 1; i++)
            {
                swap = false;

                for (int j = 0; j < manufacturerList.Count() - i - 1; j++)
                {
                    if (manufacturerList[j].GetPoints() < manufacturerList[j + 1].GetPoints())
                    {
                        swap = true;

                        (manufacturerList[j], manufacturerList[j + 1]) = (manufacturerList[j + 1], manufacturerList[j]);
                    }

                    else if (manufacturerList[j].GetPoints() == manufacturerList[j + 1].GetPoints())
                    {
                        driver1Results = OrderResults(manufacturerList[j].GetResults());
                        driver2Results = OrderResults(manufacturerList[j + 1].GetResults());

                        while (roundIndex < driver1Results.Count())
                        {
                            if (driver1Results[roundIndex] > driver2Results[roundIndex])
                            {
                                swap = true;

                                (manufacturerList[j], manufacturerList[j + 1]) = (manufacturerList[j + 1], manufacturerList[j]);
                                break;
                            }

                            else if (driver1Results[roundIndex] == driver2Results[roundIndex])
                            {
                                roundIndex++;
                            }

                            else
                            {
                                break;
                            }
                        }

                        roundIndex = 0;
                    }
                }

                if (!swap)
                {
                    break;
                }
            }
        }

        private List<int> OrderResults(List<int> rawResults)
        {
            if (rawResults.Count() > 1)
            {
                bool swap;

                for (int i = 0; i < rawResults.Count() - 1; i++)
                {
                    swap = false;

                    for (int j = 0; j < rawResults.Count() - i - 1; j++)
                    {
                        if (rawResults[j] > rawResults[j + 1])
                        {
                            swap = true;

                            (rawResults[j], rawResults[j + 1]) = (rawResults[j + 1], rawResults[j]);
                        }
                    }

                    if (!swap)
                    {
                        break;
                    }
                }
            }

            return rawResults;
        }


        public void SetClassName(string newClassName)
        {
            className = newClassName;
        }

        public string GetClassName()
        {
            return className;
        }

        public List<string> GetEligiblePlatforms()
        {
            return eligiblePlatforms;
        }

        public void SetIRM(int newIRM)
        {
            incidentRangeModifier = newIRM;
        }

        public int GetIRM()
        {
            return incidentRangeModifier;
        }

        public void SetDNFRM(int newDNFRM)
        {
            dnfRateModifier = newDNFRM;
        }

        public int GetDNFRM()
        {
            return dnfRateModifier;
        }

        public void SetClassIndex(int CI)
        {
            classIndex = CI;
        }

        public int GetClassIndex()
        {
            return classIndex;
        }

        public void SetSRLow(int newSRL)
        {
            stintRangeLow = newSRL;
        }

        public int GetSRLow()
        {
            return stintRangeLow;
        }

        public void SetSRHigh(int newSRH)
        {
            stintRangeHigh = newSRH;
        }

        public int GetSRHigh()
        {
            return stintRangeHigh;
        }

        public void SetSRInc(int SRI)
        {
            stintRangeIncident = SRI;
        }

        public int GetSRInc()
        {
            return stintRangeIncident;
        }

        public void SetMinOVR(int OVR)
        {
            minOVR = OVR;
        }

        public int GetMinOVR()
        {
            return minOVR;
        }

        public void SetMaxOVR(int OVR)
        {
            maxOVR = OVR;
        }

        public int GetMaxOVR()
        {
            return maxOVR;
        }

        public void SetWECDTP(int newDTP)
        {
            wecDistanceToPit = newDTP;
        }

        public int GetWECDTP()
        {
            return wecDistanceToPit;
        }

        public void SetIMSADTP(int newDTP)
        {
            imsaDistanceToPit = newDTP;
        }

        public int GetIMSADTP()
        {
            return imsaDistanceToPit;
        }

        public void SetLapDTP(int newDTP)
        {
            lapDistanceToPit = newDTP;
        }

        public int GetLapDTP()
        {
            return lapDistanceToPit;
        }

        public void SetWECGarageValues(int V1, int V2, int V3)
        {
            wecMinStintScore = V1;
            wecMaxStintScore = V2;
            wecGarageStintScore = V3;
        }

        public (int, int, int) GetWECGarageValues()
        {
            return (wecMinStintScore, wecMaxStintScore, wecGarageStintScore);
        }

        public void SetIMSAGarageValues(int V1, int V2, int V3)
        {
            imsaMinStintScore = V1;
            imsaMaxStintScore = V2;
            imsaGarageStintScore = V3;
        }

        public (int, int, int) GetIMSAGarageValues()
        {
            return (imsaMinStintScore, imsaMaxStintScore, imsaGarageStintScore);
        }

        public void SetLapGarageValues(int V1, int V2, int V3)
        {
            lapMinStintScore = V1;
            lapMaxStintScore = V2;
            lapGarageStintScore = V3;
        }

        public (int, int, int) GetLapGarageValues()
        {
            return (lapMinStintScore, lapMaxStintScore, lapGarageStintScore);
        }
    }
}

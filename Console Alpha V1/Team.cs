using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Alpha_V1
{
    public class Team
    {
        string teamName;
        int teamOVR;

        List<int> spacerList;

        Series enteredSeries;
        List<Entrant> crewList, oldCrews;
        List<Series> enteredSeriesList;

        public Team(string tN, int tO, Series eS, List<Entrant> cL)
        {
            teamName = tN;
            teamOVR = tO;

            enteredSeries = eS;

            crewList = cL;
            oldCrews = new List<Entrant>();

            enteredSeriesList = new List<Series>() { eS };

            SetSpacerList();
        }

        public void SetSpacerList()
        {
            spacerList = new List<int>();

            foreach (Entrant newEntrant in crewList)
            {
                if (spacerList.Count() == 0)
                {
                    spacerList.Add(newEntrant.GetCarNo().Length);
                    spacerList.Add(newEntrant.GetManufacturer().Length);
                    spacerList.Add(newEntrant.GetClass().GetClassName().Length);
                }

                else
                {
                    if (newEntrant.GetCarNo().Length > spacerList[0])
                    {
                        spacerList[0] = newEntrant.GetCarNo().Length;
                    }

                    if (newEntrant.GetManufacturer().Length > spacerList[1])
                    {
                        spacerList[1] = newEntrant.GetManufacturer().Length;
                    }

                    if (newEntrant.GetClass().GetClassName().Length > spacerList[2])
                    {
                        spacerList[2] = newEntrant.GetClass().GetClassName().Length;
                    }
                }
            }
        }

        public void SetTeamName(string newTeamName)
        {
            teamName = newTeamName;
        }

        public string GetTeamName()
        {
            return teamName;
        }

        public void SetTeamOVR(int newOVR)
        {
            teamOVR = newOVR;
        }

        public int GetTeamOVR()
        {
            return teamOVR;
        }

        public Series GetEnteredSeries()
        {
            return enteredSeries;
        }

        public List<Series> GetEnteredSeriesList()
        {
            return enteredSeriesList;
        }

        public void AddCrew(Entrant newCrew)
        {
            crewList.Add(newCrew);
        }

        public List<Entrant> GetTeamEntries()
        {
            return crewList;
        }

        public void DeleteCrew(Entrant oldCrew)
        {
            crewList.Remove(oldCrew);
            oldCrews.Add(oldCrew);
        }

        public List<Entrant> GetOldCrews()
        {
            return oldCrews;
        }

        public List<int> GetSpacerList()
        {
            return spacerList;
        }

        public void OrderCrews()
        {
            // Order by Number

            int carNumber1, carNumber2;
            bool swap;

            for (int i = 0; i < crewList.Count() - 1; i++)
            {
                swap = false;

                for (int j = 0; j < crewList.Count() - i - 1; j++)
                {
                    carNumber1 = Convert.ToInt32(crewList[j].GetCarNo().Replace("#", ""));
                    carNumber2 = Convert.ToInt32(crewList[j + 1].GetCarNo().Replace("#", ""));

                    if (carNumber1 > carNumber2)
                    {
                        swap = true;

                        (crewList[j], crewList[j + 1]) = (crewList[j + 1], crewList[j]);
                    }
                }

                if (!swap)
                {
                    break;
                }
            }

            // Order by Class

            for (int i = 0; i < crewList.Count() - 1; i++)
            {
                swap = false;

                for (int j = 0; j < crewList.Count() - i - 1; j++)
                {
                    if (crewList[j].GetClassIndex() > crewList[j + 1].GetClassIndex())
                    {
                        swap = true;

                        (crewList[j], crewList[j + 1]) = (crewList[j + 1], crewList[j]);
                    }
                }

                if (!swap)
                {
                    break;
                }
            }

            // Order by Series

            for (int i = 0; i < crewList.Count() - 1; i++)
            {
                swap = false;

                for (int j = 0; j < crewList.Count() - i - 1; j++)
                {
                    if (crewList[j].GetSeriesIndex() > crewList[j + 1].GetSeriesIndex())
                    {
                        swap = true;

                        (crewList[j], crewList[j + 1]) = (crewList[j + 1], crewList[j]);
                    }
                }

                if (!swap)
                {
                    break;
                }
            }
        }

        public void UpdateCrewStats(Random randomiser)
        {
            Entrant currentEntrant;

            for (int i = 0; i < crewList.Count(); i++)
            {
                currentEntrant = crewList[i];

                int newOVR, newReliability;

                (newOVR, newReliability) = UpdateCrewStat(currentEntrant, randomiser);

                currentEntrant.SetCrewOVR(newOVR);
                currentEntrant.SetBaseReliability(newReliability);
                currentEntrant.SetPoints(0);
                currentEntrant.ResetPastResults();
            }
        }

        private (int, int) UpdateCrewStat(Entrant currentEntrant, Random randomiser)
        {
            int currentOVR = currentEntrant.GetOVR(),
                    currentReliability = currentEntrant.GetBaseReliability();

            int ovrUpperRange = currentEntrant.GetClass().GetMaxOVR() - currentOVR,
                reliabilityUpperRange = 30 - currentReliability;

            if (ovrUpperRange > 3)
            {
                ovrUpperRange = 3;
            }

            if (reliabilityUpperRange > 3)
            {
                reliabilityUpperRange = 3;
            }

            int ovrLowerRange = ovrUpperRange - randomiser.Next(2, 5),
                reliabilityLowerRange = reliabilityUpperRange - randomiser.Next(2, 5);

            int newOVR = currentOVR + randomiser.Next(ovrLowerRange, ovrUpperRange + 1),
                newReliability = currentReliability + randomiser.Next(reliabilityLowerRange, reliabilityUpperRange + 1);

            if (newOVR > currentEntrant.GetClass().GetMaxOVR())
            {
                newOVR = currentEntrant.GetClass().GetMaxOVR();
            }

            if (newReliability > 30)
            {
                newReliability = 30;
            }

            return (newOVR, newReliability);
        }

    }
}

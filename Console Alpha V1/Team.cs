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

        List<int> spacerList;

        Series enteredSeries;
        List<Entrant> crewList, oldCrews;

        public Team(string tN, Series eS, List<Entrant> cL)
        {
            teamName = tN;

            enteredSeries = eS;

            crewList = cL;
            oldCrews = new List<Entrant>();

            SetSpacerList();
        }

        private void SetSpacerList()
        {
            spacerList = new List<int>();

            foreach (Entrant newEntrant in crewList)
            {
                if (spacerList.Count() == 0)
                {
                    spacerList.Add(newEntrant.GetCarNo().Length);
                    spacerList.Add(newEntrant.GetManufacturer().Length);
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
                }
            }
        }

        public string GetTeamName()
        {
            return teamName;
        }

        public Series GetEnteredSeries()
        {
            return enteredSeries;
        }

        public List<Entrant> GetTeamEntries()
        {
            return crewList;
        }

        public void AddOldCrew(Entrant oldCrew)
        {
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
    }
}

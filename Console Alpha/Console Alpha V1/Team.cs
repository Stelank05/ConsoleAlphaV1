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

        Series enteredSeries;
        List<Entrant> crewList;

        public Team(string tN, Series eS, List<Entrant> cL)
        {
            teamName = tN;

            enteredSeries = eS;
            crewList = cL;
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
    }
}

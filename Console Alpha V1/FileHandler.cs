using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_Alpha_V1
{
    public static class FileHandler
    {
        public static void SetGameSaveFolder(Team playerTeam)
        {
            string saveFolder = Path.Combine(CommonData.GetMainFolder(), "Results"),
                teamName = playerTeam.GetTeamName();

            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            string playerSaveFolder = Path.Combine(saveFolder, teamName);
            int saveIndex = 1;

            while (Directory.Exists(playerSaveFolder))
            {
                playerSaveFolder = Path.Combine(saveFolder, teamName + String.Format(" ({0})", saveIndex));
                saveIndex++;
            }

            CommonData.SetSaveFolder(playerSaveFolder);
            Directory.CreateDirectory(playerSaveFolder);

            //WriteTeamData(playerTeam);
        }

        public static void SetSeasonFolder(int seasonNumber)
        {
            string seasonFolder = Path.Combine(CommonData.GetSaveFolder(), string.Format("Season {0}", seasonNumber));

            CommonData.SetSeasonFolder(seasonFolder);
            Directory.CreateDirectory(seasonFolder);
        }

        public static void WriteTeamData(Team playerTeam, int seasonNumber)
        {
            string filePath = Path.Combine(CommonData.GetSeasonFolder(), string.Format("Team Data - Season {0}.csv", seasonNumber)),
                writeString = playerTeam.GetTeamName() + "\nCrew No,Series,Class,Car No,Car Model,,Team OVR,Crew OVR,SRM,Reliability";

            Entrant currentEntrant;
            List<Entrant> crewList = playerTeam.GetTeamEntries();

            for (int i = 0; i < crewList.Count(); i++)
            {
                currentEntrant = crewList[i];

                writeString += string.Format("\nCrew {0},{1},{2},{3},{4},,{5},{6},{7},{8}",
                    i + 1, playerTeam.GetEnteredSeries().GetFolderName(), currentEntrant.GetClass().GetClassName(),
                    currentEntrant.GetCarNo(), currentEntrant.GetCarModel().GetModelName(),
                    currentEntrant.GetTeamOVR(), currentEntrant.GetCrewOVR(), currentEntrant.GetSRM(), currentEntrant.GetBaseReliability());
            }

            WriteFile(writeString, filePath);
        }

        public static void CreateRoundFolder(int roundNumber, Round currentRound)
        {
            string folderPath = Path.Combine(CommonData.GetSeasonFolder(), string.Format("Round {0} - {1}", roundNumber, currentRound.GetRoundName()));

            currentRound.SetFolder(folderPath);
            Directory.CreateDirectory(folderPath);
        }

        public static string[] ReadFile(string filePath)
        {
            bool fileRead = false;
            string[] returnData = new string[1];

            while (!fileRead)
            {
                try
                {
                    returnData = File.ReadAllLines(filePath);
                    fileRead = true;
                }

                catch
                {
                    Console.WriteLine("Please close File '{0}'", filePath);
                    fileRead = false;
                    Console.ReadLine();
                }
            }

            return returnData;
        }

        public static void WriteFile(string writeString, string filePath)
        {
            bool fileWritten = false;

            while (!fileWritten)
            {
                try
                {
                    File.WriteAllText(filePath, writeString);
                    fileWritten = true;
                }

                catch
                {
                    Console.WriteLine("Error with File '{0}'", filePath);
                    fileWritten = false;
                    Console.ReadLine();
                }
            }
        }
    }
}

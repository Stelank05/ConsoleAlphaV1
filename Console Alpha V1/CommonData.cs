using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_Alpha_V1
{
    public static class CommonData
    {
        static string mainFolder, setupFolder, seriesFile, saveFolder, seasonFolder,
            entrantsFolder, distancesFolder, garageValuesFolder, pointsSystemFolder;

        static List<string> wecDistances = new List<string>(),
            imsaDistances = new List<string>(),
            lapDistances = new List<string>();

        static List<int> wec_LeaveGarageValues = new List<int>(), wec_EnterGarageValues = new List<int>(),
            imsa_LeaveGarageValues = new List<int>(), imsa_EnterGarageValues = new List<int>(),
            lap_LeaveGarageValues = new List<int>(), lap_EnterGarageValues = new List<int>();

        public static void Setup()
        {
            string seriesFile = @"Series.csv";

            mainFolder = Path.GetFullPath(seriesFile).Replace("\\Console Alpha V1\\bin\\Debug\\Series.csv", "");
            setupFolder = "Setup";
            CommonData.seriesFile = "Series.csv";
            entrantsFolder = "Entrants";
            distancesFolder = "Race Distances";
            garageValuesFolder = "Garage Values";
            pointsSystemFolder = "Points Systems";

            LoadDistances();
            LoadGarageValues();
        }

        private static void LoadDistances()
        {
            try
            {
                string[] sWECDistances = FileHandler.ReadFile(Path.Combine(GetSetupPath(), distancesFolder, "WEC Distances.csv"));

                foreach (string sWEC in sWECDistances)
                {
                    wecDistances.Add(sWEC);
                }

                string[] sIMSADistances = FileHandler.ReadFile(Path.Combine(GetSetupPath(), distancesFolder, "IMSA Distances.csv"));

                foreach (string sIMSA in sIMSADistances)
                {
                    imsaDistances.Add(sIMSA);
                }

                string[] sLapDistances = FileHandler.ReadFile(Path.Combine(GetSetupPath(), distancesFolder, "Lap Distances.csv"));

                foreach (string sLap in sLapDistances)
                {
                    lapDistances.Add(sLap);
                }
            }

            catch
            {
                Console.WriteLine("Please Close any Open Distance Files (WEC, IMSA, Lap)");
                Console.ReadLine();
                LoadDistances();
            }
        }

        private static void LoadGarageValues()
        {
            List<string> fileNames = new List<string> { "Enter Garage Values.csv", "Exit Garage Values.csv" };
            List<string> lengths = new List<string> { "WEC", "IMSA", "Laps" };

            string length, fileName;

            for (int i = 0; i < lengths.Count(); i++)
            {
                length = lengths[i];

                for (int j = 0; j < fileNames.Count(); j++)
                {
                    fileName = fileNames[j];

                    string[] data = FileHandler.ReadFile(Path.Combine(GetSetupPath(), garageValuesFolder, length, fileName));

                    List<int> mungedData = new List<int>();

                    foreach (string dataPiece in data)
                    {
                        mungedData.Add(Convert.ToInt32(dataPiece.Split(',')[1]));
                    }

                    if (fileName.StartsWith("Enter"))
                    {
                        switch (length)
                        {
                            case "WEC":
                                wec_EnterGarageValues = mungedData;
                                break;
                            case "IMSA":
                                imsa_EnterGarageValues = mungedData;
                                break;
                            case "Laps":
                                lap_EnterGarageValues = mungedData;
                                break;
                        }
                    }

                    else
                    {
                        switch (length)
                        {
                            case "WEC":
                                wec_LeaveGarageValues = mungedData;
                                break;
                            case "IMSA":
                                imsa_LeaveGarageValues = mungedData;
                                break;
                            case "Laps":
                                lap_LeaveGarageValues = mungedData;
                                break;
                        }
                    }
                    
                }
            }
        }

        public static string GetMainFolder()
        {
            return mainFolder;
        }

        public static string GetSetupFolder()
        {
            return setupFolder;
        }

        public static string GetSetupPath()
        {
            return Path.Combine(mainFolder, setupFolder);
        }

        public static void SetSaveFolder(string newFolder)
        {
            saveFolder = newFolder;
        }

        public static string GetSaveFolder()
        {
            return saveFolder;
        }

        public static void SetSeasonFolder(string newFolder)
        {
            seasonFolder = newFolder;
        }

        public static string GetSeasonFolder()
        {
            return seasonFolder;
        }

        public static string GetSeriesFile()
        {
            return Path.Combine(GetSetupPath(), seriesFile);
        }

        public static string GetEntrantsFolder()
        {
            return entrantsFolder;
        }

        public static string GetPointsSystemFolder()
        {
            return pointsSystemFolder;
        }

        public static List<string> GetDistancesList(string lengthType)
        {
            switch (lengthType)
            {
                case "WEC":
                    return wecDistances;
                case "IMSA":
                    return imsaDistances;
                case "Laps":
                    return lapDistances;
                default:
                    return new List<string>();
            }
        }

        public static List<int> GetEnterGarageValues(string lengthType)
        {
            switch (lengthType)
            {
                case "WEC":
                    return wec_EnterGarageValues;
                case "IMSA":
                    return imsa_EnterGarageValues;
                case "Laps":
                    return lap_EnterGarageValues;
                default:
                    return new List<int>();
            }
        }

        public static List<int> GetLeaveGarageValues(string lengthType)
        {
            switch (lengthType)
            {
                case "WEC":
                    return wec_LeaveGarageValues;
                case "IMSA":
                    return imsa_LeaveGarageValues;
                case "Laps":
                    return lap_LeaveGarageValues;
                default:
                    return new List<int>();
            }
        }
    }
}

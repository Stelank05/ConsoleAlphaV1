using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_Alpha_V1
{
    public class Game
    {
        Random randomiser;

        Round currentRound;
        Series chosenSeries;
        Simulator gameSimulator;
        Team playerTeam;

        List<int> pointsSystem;

        List<Entrant> entryList;
        List<Series> seriesList;

        public Game()
        {
            CommonData.Setup();
            LoadSeries();

            randomiser = new Random();
            gameSimulator = new Simulator(randomiser);
        }

        private void LoadSeries()
        {
            seriesList = new List<Series>();

            string[] seriesData = File.ReadAllLines(CommonData.GetSeriesFile()), lineData;

            Series newSeries;

            for (int i = 1; i < seriesData.Length; i++)
            {
                lineData = seriesData[i].Split(',');

                newSeries = new Series(lineData[0], lineData[1], Convert.ToInt32(lineData[2]));
                seriesList.Add(newSeries);
            }
        }


        // Game Functions

        public void PlayGame()
        {
            SetupTeam();
            //DisplaySeriesCalendar();

            string roundLength = "";

            currentRound = chosenSeries.GetCalendar()[0];
            LoadEntryList();

            for (int roundNumber = 0; roundNumber < chosenSeries.GetCalendar().Count(); roundNumber++)
            {
                if (roundNumber > 0)
                {
                    currentRound = chosenSeries.GetCalendar()[roundNumber];
                }

                gameSimulator.SetRound(currentRound);

                switch (currentRound.GetLengthType())
                {
                    case "WEC":
                        roundLength = CommonData.GetDistancesList("WEC")[currentRound.GetRaceLength() - 1];
                        break;
                    case "IMSA":
                        roundLength = CommonData.GetDistancesList("IMSA")[currentRound.GetRaceLength() - 1];
                        break;
                    case "Laps":
                        roundLength = currentRound.GetRaceLength() + " Laps";
                        break;
                }

                Console.WriteLine("Round Details:\nRound {0} - {1}\nLength: {2}", roundNumber + 1, currentRound.GetRoundName(), roundLength);
                Console.ReadLine();

                SetEntryList();

                for (int i = 0; i < 3; i++)
                {
                    gameSimulator.Qualifying(entryList, entryList.Count());
                }

                SetPositions();

                Console.WriteLine("Qualifying Results for {0}:", playerTeam.GetTeamName());
                DisplayTeamEntrants();
                Console.ReadLine();

                Console.WriteLine("Full {0} Qualifying Results:", currentRound.GetRoundName());
                DisplayEntrants();
                Console.ReadLine();

                gameSimulator.SetGrid(entryList, 10);

                int raceLength = currentRound.GetRaceLength();

                for (int stintNumber = 1; stintNumber <= raceLength; stintNumber++)
                {
                    gameSimulator.Race(entryList, stintNumber);

                    if (stintNumber == raceLength / 2)
                    {
                        SetPositions();

                        Console.WriteLine("{0} Running Positions at Half Distance:", playerTeam.GetTeamName());
                        DisplayTeamEntrants();
                        Console.ReadLine();

                        Console.WriteLine("{0} Running Order at Half Distance:", currentRound.GetRoundName());
                        DisplayEntrants();
                        Console.ReadLine();
                    }
                }

                for (int i = 0; i < entryList.Count(); i++)
                {
                    if (entryList[i].GetInGarage() && entryList[i].GetOVR() != 1)
                    {
                        entryList[i].UpdateOVR(100);
                    }
                }

                gameSimulator.Sort(entryList, 0, entryList.Count());

                SetPositions();

                Console.WriteLine("{0} Finising Positions:", playerTeam.GetTeamName());
                DisplayTeamEntrants();
                Console.ReadLine();

                Console.WriteLine("Full {0} Race Results:", currentRound.GetRoundName());
                DisplayEntrants();
                Console.ReadLine();

                AwardPoints();
                SortStandings();
                SetStandingsPositions();

                Console.WriteLine("Points Scored by {0}:", playerTeam.GetTeamName());
                DisplayTeamPoints();
                Console.ReadLine();

                Console.WriteLine("\nStandings after {0}:\n", currentRound.GetRoundName());
                DisplayPoints();
                Console.ReadLine();
            }

            Console.WriteLine("{0} Class Champions:", chosenSeries.GetSeriesName());

            Entrant championshipWinner;

            for (int classIndex = 0; classIndex < chosenSeries.GetClassList().Count(); classIndex++)
            {
                championshipWinner = GetChampionshipLeader(chosenSeries.GetClassList()[classIndex].GetClassName());

                Console.WriteLine("{0}: {1} {2} - {3} - {4} Points", chosenSeries.GetClassList()[classIndex].GetClassName().PadRight(7, ' '), championshipWinner.GetCarNo().PadRight(4, ' '), championshipWinner.GetTeamName().PadRight(43, ' '), championshipWinner.GetManufacturer().PadRight(16, ' '), championshipWinner.GetPoints());
            }

            Console.ReadLine();

            Console.WriteLine("Thank you for playing the First Console Alpha of the (Hopefully Happening) Global Endurance Masters Game!");
            Console.WriteLine("That's the end of the Game, but you can always play again with different Cars");
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }

        private void SetupTeam()
        {
            Console.Write("Please Enter Team Name: ");
            string teamName = Console.ReadLine();

            chosenSeries = seriesList[SelectSeries()];

            List<Entrant> crewList = CreateCrews(teamName, chosenSeries.GetMaxEnterableCrews());

            playerTeam = new Team(teamName, chosenSeries, crewList);

            Console.WriteLine();
            Console.WriteLine("Team Information:");
            Console.WriteLine("Team Name: {0}", playerTeam.GetTeamName());
            Console.WriteLine("Entered Series: {0}", playerTeam.GetEnteredSeries().GetSeriesName());
            
            for (int i = 0; i < playerTeam.GetTeamEntries().Count(); i++)
            {
                Console.WriteLine("Crew {0}: {1} {2} - {3}", i + 1, playerTeam.GetTeamEntries()[i].GetCarNo().PadRight(4, ' '), playerTeam.GetTeamEntries()[i].GetCarModel().GetManufacturer(), playerTeam.GetTeamEntries()[i].GetClass().GetClassName());
            }

            Console.ReadLine();
        }


        // Display Functions

        private void DisplaySeriesCalendar()
        {
            Console.WriteLine("{0} Calendar:", chosenSeries.GetSeriesName());

            Round outputRound;
            string roundLength = "";

            for (int i = 0; i < chosenSeries.GetCalendar().Count(); i++)
            {
                outputRound = chosenSeries.GetCalendar()[i];

                switch (outputRound.GetLengthType())
                {
                    case "WEC":
                        roundLength = CommonData.GetDistancesList("WEC")[outputRound.GetRaceLength() - 1];
                        break;
                    case "IMSA":
                        roundLength = CommonData.GetDistancesList("IMSA")[outputRound.GetRaceLength() - 1];
                        break;
                    case "Laps":
                        roundLength = outputRound.GetRaceLength() + " Laps";
                        break;
                }

                Console.WriteLine("R{0}: - {1} - {2}", Convert.ToString(i + 1).PadRight(2, ' '), outputRound.GetRoundName().PadRight(22, ' '), roundLength);
            }

            Console.ReadLine();
        }

        private void DisplayEntrants()
        {
            string posString, classPosString;

            Entrant currentEntrant;

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];

                (posString, classPosString) = currentEntrant.GetCurrentPosition();

                Console.WriteLine("{0} Overall / {1} In {6} - {2} {3} - {4} - {5}", posString.PadRight(3, ' '), classPosString.PadRight(3, ' '), currentEntrant.GetCarNo().PadRight(4, ' '), currentEntrant.GetTeamName().PadRight(43, ' '), currentEntrant.GetManufacturer().PadRight(16, ' '), currentEntrant.GetOVR(), currentEntrant.GetClass().GetClassName().PadRight(7, ' '));
            }
        }

        private void DisplayTeamEntrants()
        {
            string posString, classPosString;

            Entrant currentEntrant;
            List<Entrant> teamEntrants = new List<Entrant>();

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];
                
                if (currentEntrant.GetTeamName() == playerTeam.GetTeamName())
                {
                    teamEntrants.Add(currentEntrant);
                }
            }

            IndexSort(teamEntrants);

            for (int i = 0; i < teamEntrants.Count(); i++)
            {
                currentEntrant = teamEntrants[i];
                
                (posString, classPosString) = currentEntrant.GetCurrentPosition();

                Console.WriteLine("Crew {0}: {1} {2} - {3} Overall / {4} In {5}", i + 1, currentEntrant.GetCarNo().PadRight(4, ' '), currentEntrant.GetManufacturer(), posString.PadRight(3, ' '), classPosString.PadRight(3, ' '), currentEntrant.GetClass().GetClassName());
            }
        }

        private void DisplayTeamPoints()
        {
            Entrant currentEntrant;
            List<Entrant> teamEntrants = new List<Entrant>();

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];
                if (currentEntrant.GetTeamName() == playerTeam.GetTeamName())
                {
                    teamEntrants.Add(currentEntrant);
                }
            }

            IndexSort(teamEntrants);

            for (int i = 0; i < teamEntrants.Count(); i++)
            {
                currentEntrant = teamEntrants[i];

                Console.WriteLine("Crew {0}: {1} {2} - {3} Points - {4} in {5}", i + 1, currentEntrant.GetCarNo().PadRight(4, ' '), currentEntrant.GetManufacturer().PadRight(16, ' '), Convert.ToString(currentEntrant.GetPoints()).PadRight(3, ' '), currentEntrant.GetStandingsPosition().PadRight(3, ' '), currentEntrant.GetClass().GetClassName());
            }
        }

        private void DisplayPoints()
        {
            string currentClass = chosenSeries.GetClassList()[0].GetClassName();
            int classPosition = 1, classIndex = 1;

            Entrant currentEntrant;

            Console.WriteLine("{0}:", currentClass);

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];

                if (currentEntrant.GetClass().GetClassName() != currentClass)
                {
                    currentClass = chosenSeries.GetClassList()[classIndex].GetClassName();
                    Console.WriteLine("\n{0}:", currentClass);
                    classIndex++;
                }

                Console.WriteLine("{0}: {1} {2} - {3} - {4} Points", currentEntrant.GetStandingsPosition().PadRight(3, ' '), currentEntrant.GetCarNo().PadRight(4, ' '), currentEntrant.GetTeamName().PadRight(43, ' '), currentEntrant.GetManufacturer().PadRight(16, ' '), currentEntrant.GetPoints());
                classPosition++;
            }
        }


        // Crew Creation

        private int SelectSeries()
        {
            int selectedSeries;

            Console.WriteLine("\nAvailable Series:");

            for (int i = 0; i < seriesList.Count(); i++)
            {
                Console.WriteLine("{0} - {1}", (i + 1), seriesList[i].GetSeriesName());
            }

            Console.Write("Please Select a Series: ");
            bool validSeries = int.TryParse(Console.ReadLine(), out selectedSeries);

            if (validSeries && selectedSeries > 0 && selectedSeries <= seriesList.Count())
            {
                return selectedSeries - 1;
            }

            Console.WriteLine("Invalid Series Selected\n");
            return SelectSeries();
        }

        private List<Entrant> CreateCrews(string teamName, int maxCrews)
        {
            List<Class> classList = chosenSeries.GetClassList();
            List<Entrant> crewList = new List<Entrant>();

            CarModel selectedModel;
            Class chosenClass;

            Entrant newCrew;

            int teamOVR = randomiser.Next(97, 101), crewOVR, stintModifier, reliability;

            int[] classEntrants = new int[classList.Count()];

            for (int i = 0; i < classList.Count(); i++)
            {
                classEntrants[i] = 0;
            }

            while (crewList.Count() < maxCrews)
            {
                int selectedClass = SelectClass(classList);

                if (selectedClass == -1)
                {
                    return crewList;
                }

                else if (classEntrants[selectedClass] + 1 > 2)
                {
                    Console.WriteLine("Too Many Entrants in this Class\n");
                }

                else
                {
                    selectedModel = SelectCarModel(selectedClass);
                    chosenClass = classList[selectedClass];
                    
                    string carNumber = GetCarNumber();

                    crewOVR = randomiser.Next(chosenClass.GetMinOVR(), chosenClass.GetMaxOVR() + 1);
                    stintModifier = randomiser.Next(2, 6);
                    reliability = randomiser.Next(24, 31);

                    newCrew = new Entrant(carNumber, teamName, teamOVR + crewOVR, stintModifier, reliability, selectedModel, chosenClass);
                    crewList.Add(newCrew);

                    classEntrants[selectedClass]++;
                }
            }

            return crewList;
        }

        private int SelectClass(List<Class> classList)
        {
            string selectedClass;
            int intSelectedClass;

            Console.WriteLine("\nAvailable Classes:");

            for (int i = 0; i < classList.Count(); i++)
            {
                Console.WriteLine("{0} - {1}", i + 1, classList[i].GetClassName());
            }

            Console.WriteLine("E - End Selection");
            Console.Write("Class Choice: ");
            selectedClass = Console.ReadLine().ToUpper();

            if (int.TryParse(selectedClass, out intSelectedClass))
            {
                if (intSelectedClass > 0 && intSelectedClass <= classList.Count())
                {
                    return intSelectedClass - 1;
                }
            }

            else
            {
                for (int i = 0; i < classList.Count(); i++)
                {
                    if (selectedClass == classList[i].GetClassName())
                    {
                        return i;
                    }
                }

                if (selectedClass == "E")
                {
                    return -1;
                }
            }

            Console.WriteLine("Invalid Option");

            return SelectClass(classList);
        }

        private CarModel SelectCarModel(int chosenClass)
        {
            Class selectedClass = chosenSeries.GetClassList()[chosenClass];

            List<CarModel> modelList = chosenSeries.GetCarModelList(),
                availableModels = new List<CarModel>();
            int modelNumber = 1;

            Console.WriteLine("\nEligible Car Models");

            for (int i = 0; i < modelList.Count(); i++)
            {
                if (selectedClass.GetEligiblePlatforms().Contains(modelList[i].GetPlatform()))
                {
                    availableModels.Add(modelList[i]);
                    Console.WriteLine("{0} - {1} - {2}", Convert.ToString(modelNumber).PadRight(2, ' '), modelList[i].GetPlatform().PadRight(4, ' '), modelList[i].GetModelName());
                    modelNumber++;
                }
            }

            Console.Write("Car Model Choice: ");
            string chosenModel = Console.ReadLine().ToLower();
            int intChosenModel;

            if (int.TryParse(chosenModel, out intChosenModel))
            {
                if (intChosenModel > 0 && intChosenModel <= availableModels.Count())
                {
                    return availableModels[intChosenModel - 1];
                }
            }

            else
            {
                for (int i = 0; i < availableModels.Count(); i++)
                {
                    if (chosenModel == availableModels[i].GetModelName().ToLower())
                    {
                        return availableModels[i];
                    }
                }
            }

            Console.WriteLine("Invalid Car Model\n");
            return SelectCarModel(chosenClass);
        }

        private string GetCarNumber()
        {
            Console.Write("Please Enter the Desired Car Number: #");
            string desiredNumber = Console.ReadLine();

            desiredNumber = desiredNumber.Replace("#", "");

            if (UniqueNumber(desiredNumber))
            {
                return "#" + desiredNumber;
            }

            Console.WriteLine("Invalid / Non-Unique Car Number\n");
            return GetCarNumber();
        }

        private bool UniqueNumber(string desiredNumber)
        {
            string filePath;
            string[] usedNumbers = new string[1],
                carNumber;
            
            int totalUsedNumbers;

            bool uniqueCarNumber = true,
                bN = int.TryParse(desiredNumber, out int iN),
                fileRead;

            if (bN)
            {
                string checkNumber = "#" + desiredNumber;

                for (int j = 0; j < 8; j++)
                {
                    filePath = Path.Combine(CommonData.GetSetupPath(), chosenSeries.GetFolderName(), "Entrants", "Class " + (j + 1) + ".csv");

                    fileRead = false;

                    while (!fileRead)
                    {
                        try
                        {
                            usedNumbers = File.ReadAllLines(filePath);
                            fileRead = true;
                        }
                        catch
                        {
                            Console.WriteLine("Please Close '{0}'.", filePath);
                            fileRead = false;
                            Console.ReadLine();
                        }
                    }

                    totalUsedNumbers = File.ReadAllLines(filePath).Length;

                    if (totalUsedNumbers > 0)
                    {
                        for (int i = 0; i < totalUsedNumbers; i++)
                        {
                            if (usedNumbers[i] != "")
                            {
                                carNumber = usedNumbers[i].Split(',');

                                if (carNumber[1] == checkNumber)
                                {
                                    uniqueCarNumber = false;
                                    break;
                                }
                            }
                        }
                    }

                    else
                    {
                        break;
                    }
                }
            }

            else
            {
                uniqueCarNumber = false;
            }

            return uniqueCarNumber;
        }


        // Entry List Stuffs

        private void LoadEntryList()
        {
            List<string> classes = currentRound.GetLongRacingClasses();

            entryList = new List<Entrant>();

            CarModel carModel;
            Class enteredClass;

            Entrant newEntrant;

            string basePath = Path.Combine(CommonData.GetSetupPath(), chosenSeries.GetFolderName(), "Entrants"), filePath;
            string[] classEntrants, entrantData;
            int index = 0;

            for (int classIndex = 0; classIndex < classes.Count(); classIndex++)
            {
                filePath = Path.Combine(basePath, classes[classIndex] + ".csv");

                classEntrants = File.ReadAllLines(filePath);

                for (int i = 0; i < classEntrants.Length; i++)
                {
                    if (classEntrants[i] != "")
                    {
                        entrantData = classEntrants[i].Split(',');

                        carModel = chosenSeries.GetCarModel(entrantData[3]);
                        enteredClass = chosenSeries.GetClass(entrantData[0]);

                        newEntrant = new Entrant(entrantData[1], entrantData[2], Convert.ToInt32(entrantData[5]), Convert.ToInt32(entrantData[7]), Convert.ToInt32(entrantData[9]), index, carModel, enteredClass);
                        index++;

                        entryList.Add(newEntrant);
                    }
                }
            }

            for (int i = 0; i < playerTeam.GetTeamEntries().Count(); i++)
            {
                newEntrant = playerTeam.GetTeamEntries()[i];

                newEntrant.SetIndex(index);
                newEntrant.SetRound(currentRound);

                entryList.Add(newEntrant);

                index++;
            }
        }
    
        private void SetEntryList()
        {
            IndexSort(entryList);

            for (int i = 0; i < entryList.Count(); i++)
            {
                entryList[i].SetRound(currentRound);
            }
        }


        // Points System Stuffs

        private void AwardPoints()
        {
            List<Class> classList = chosenSeries.GetClassList();

            string currentClass = classList[0].GetClassName();
            int classPosition = 0;

            ClassSort();
            LoadPointsSystem();

            for (int i = 0; i < entryList.Count(); i++)
            {
                if (entryList[i].GetClass().GetClassName() != currentClass)
                {
                    currentClass = entryList[i].GetClass().GetClassName();
                    classPosition = 0;
                }

                if (classPosition < pointsSystem.Count() && entryList[i].GetOVR() > 100)
                {
                    entryList[i].SetPoints(entryList[i].GetPoints() + pointsSystem[classPosition]);
                    classPosition++;
                }
            }
        }

        private void LoadPointsSystem()
        {
            pointsSystem = new List<int>();

            string pointsSystemFile = Path.Combine(CommonData.GetSetupPath(), "Points Systems", "Race Systems", "System " + currentRound.GetPointsSystem() + ".csv");
            string[] pointsSystemData = new string[1];

            bool fileRead = false;

            while (!fileRead)
            {
                try
                {
                    pointsSystemData = File.ReadAllLines(pointsSystemFile);
                    fileRead = true;
                }
                catch
                {
                    Console.WriteLine("Please Close '{0}'.", pointsSystemFile);
                    Console.ReadLine();
                    fileRead = false;
                }
            }

            for (int i = 0; i < pointsSystemData.Length; i++)
            {
                pointsSystem.Add(Convert.ToInt32(pointsSystemData[i]));
            }
        }

        private Entrant GetChampionshipLeader(string targetClassName)
        {
            for (int i = 0; i < entryList.Count(); i++)
            {
                if (entryList[i].GetClass().GetClassName() == targetClassName)
                {
                    return entryList[i];
                }
            }

            return new Entrant();
        }


        // Set Positions

        private void SetPositions()
        {
            string posString, classPosString;
            int classIndex;
            List<int> classPositions = new List<int>();

            Entrant currentEntrant;

            for (int i = 0; i < chosenSeries.GetClassList().Count(); i++)
            {
                classPositions.Add(1);
            }

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];
                classIndex = currentEntrant.GetClassIndex() - 1;

                if (currentEntrant.GetOVR() == 1)
                {
                    posString = "DNF";
                    classPosString = "DNF";
                }

                else if (currentEntrant.GetOVR() == 100 && currentEntrant.GetInGarage())
                {
                    posString = "NC";
                    classPosString = "NC";
                }

                else if (currentEntrant.GetInGarage())
                {
                    posString = "GAR";
                    classPosString = "GAR";

                    classPositions[classIndex]++;
                }

                else
                {
                    posString = "P" + (i + 1);
                    classPosString = "P" + classPositions[classIndex];

                    classPositions[classIndex]++;
                }

                currentEntrant.SetCurrentPositions(posString, classPosString);
            }
        }

        private void SetStandingsPositions()
        {
            string posString;
            int classIndex;
            List<int> classPositions = new List<int>();

            Entrant currentEntrant;

            for (int i = 0; i < chosenSeries.GetClassList().Count(); i++)
            {
                classPositions.Add(1);
            }

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];
                classIndex = currentEntrant.GetClassIndex() - 1;

                posString = "P" + classPositions[classIndex];
                classPositions[classIndex]++;

                currentEntrant.SetStandingsPosition(posString);
            }
        }


        // Sorts

        private void IndexSort(List<Entrant> entryList)
        {
            bool swap;

            for (int i = 0; i < entryList.Count() - 1; i++)
            {
                swap = false;

                for (int j = 0; j < entryList.Count() - i - 1; j++)
                {
                    if (entryList[j].GetIndex() > entryList[j + 1].GetIndex())
                    {
                        swap = true;

                        (entryList[j], entryList[j + 1]) = (entryList[j + 1], entryList[j]);
                    }
                }

                if (!swap)
                {
                    break;
                }
            }
        }

        private void ClassSort()
        {
            for (int i = 0; i < entryList.Count() - 1; i++)
            {
                bool Swap = false;

                for (int j = 0; j < entryList.Count() - i - 1; j++)
                {
                    if (entryList[j].GetClassIndex() > entryList[j + 1].GetClassIndex())
                    {
                        Swap = true;

                        (entryList[j], entryList[j + 1]) = (entryList[j + 1], entryList[j]);
                    }
                }

                if (!Swap)
                {
                    break;
                }
            }
        }

        private void SortStandingsOLD()
        {
            List<(int, int)> classLeaders = new List<(int, int)>();

            for (int i = 0; i < chosenSeries.GetClassList().Count(); i++)
            {
                classLeaders.Add((-1, 0));
            }

            for (int i = 0; i < entryList.Count(); i++)
            {
                int newItem1 = classLeaders[entryList[i].GetClassIndex() - 1].Item1,
                    newItem2 = classLeaders[entryList[i].GetClassIndex() - 1].Item2;

                if (classLeaders[entryList[i].GetClassIndex() - 1].Item1 == -1)
                {
                    newItem1 = i;
                }

                if (classLeaders[entryList[i].GetClassIndex() - 1].Item2 < i)
                {
                    newItem2 = i;
                }

                classLeaders[entryList[i].GetClassIndex() - 1] = (newItem1, newItem2);
            }

            for (int i = 0; i < classLeaders.Count(); i++)
            {
                SortPoints(classLeaders[i].Item1, classLeaders[i].Item2);
            }
        }

        private void SortStandings()
        {
            string currentClass = chosenSeries.GetClassList()[0].GetClassName();
            int startIndex = 0;

            for (int i = 0; i < entryList.Count(); i++)
            {
                if (entryList[i].GetClass().GetClassName() != currentClass)
                {
                    SortPoints(startIndex, i);
                    currentClass = entryList[i].GetClass().GetClassName();
                    startIndex = i;
                }
            }

            SortPoints(startIndex, entryList.Count());
        }

        private void SortPoints(int start, int end)
        {
            bool swap;

            for (int i = 0; i < end - 1; i++)
            {
                swap = false;

                for (int j = start; j < end - i - 1; j++)
                {
                    if (entryList[j].GetPoints() < entryList[j + 1].GetPoints())
                    {
                        swap = true;

                        (entryList[j], entryList[j + 1]) = (entryList[j + 1], entryList[j]);
                    }
                }

                if (!swap)
                {
                    break;
                }
            }
        }
    }
}

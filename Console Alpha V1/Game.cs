using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Console_Alpha_V1
{
    public class Game
    {
        int fileNumber, racingCount, seasonNumber, minimumTeamNameLength;
        bool playGame;

        Random randomiser;

        Round currentRound;
        Series chosenSeries;
        Simulator gameSimulator;
        Team playerTeam;

        List<int> pointsSystem, entrantSpacers, spacerList;

        List<Entrant> entryList;
        List<Series> seriesList;

        public Game()
        {
            CommonData.Setup();
            LoadSeries();

            fileNumber = 1;
            seasonNumber = 1;
            minimumTeamNameLength = 5;

            entryList = null;

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

                newSeries = new Series(lineData[0], lineData[1], Convert.ToInt32(lineData[2]), i);
                seriesList.Add(newSeries);
            }
        }


        // Game Functions

        public void PlayGame()
        {
            SetupPlayerTeam();
            FileHandler.SetGameSaveFolder(playerTeam);

            string roundLength = "";
            playGame = true;

            while(playGame)
            {
                Console.Clear();

                if (seasonNumber == 1)
                {
                    FileHandler.SetSeasonFolder(seasonNumber);
                }

                FileHandler.WriteTeamData(playerTeam, seasonNumber);

                currentRound = chosenSeries.GetCalendar()[0];
                LoadEntryList();

                for (int roundNumber = 0; roundNumber < chosenSeries.GetCalendar().Count(); roundNumber++)
                {
                    if (roundNumber > 0)
                    {
                        currentRound = chosenSeries.GetCalendar()[roundNumber];
                    }

                    gameSimulator.SetRound(currentRound);

                    FileHandler.CreateRoundFolder(roundNumber + 1, currentRound);
                    fileNumber = 1;

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
                        gameSimulator.Qualifying(entryList, racingCount);
                    }

                    SetPositions();

                    DisplayTeamEntrants(string.Format("Qualifying Results for {0}:", playerTeam.GetTeamName()));
                    DisplayEntrants(string.Format("Full {0} Qualifying Results:", currentRound.GetRoundName()), "Qualifying Results");
                    
                    gameSimulator.SetGrid(entryList, 10);

                    int raceLength = currentRound.GetRaceLength();

                    string halfDistanceString;

                    if (currentRound.GetLengthType() == "Laps")
                    {
                        halfDistanceString = string.Format("{0} Laps", raceLength / 2);
                    }

                    else
                    {
                        halfDistanceString = CommonData.GetDistancesList(currentRound.GetLengthType())[(raceLength / 2) - 1];
                    }

                    for (int stintNumber = 1; stintNumber <= raceLength; stintNumber++)
                    {
                        gameSimulator.Race(entryList, stintNumber, racingCount);

                        if (stintNumber == raceLength / 2)
                        {
                            SetPositions();

                            DisplayTeamEntrants(string.Format("{0} Running Positions at Half Distance:", playerTeam.GetTeamName()));
                            DisplayEntrants(string.Format("{0} Running Order at Half Distance:", currentRound.GetRoundName()), string.Format("Half Distance - {0}", halfDistanceString));
                        }
                    }

                    for (int i = 0; i < entryList.Count(); i++)
                    {
                        if (entryList[i].GetRacing() && entryList[i].GetInGarage() && entryList[i].GetOVR() != 1)
                        {
                            entryList[i].UpdateOVR(100);
                        }
                    }

                    gameSimulator.Sort(entryList, 0, racingCount);

                    SetPositions();

                    DisplayTeamEntrants(string.Format("{0} Finishing Positions", playerTeam.GetTeamName()));
                    DisplayEntrants(string.Format("Full {0} Race Results", currentRound.GetRoundName()), "Race Results");

                    AwardPoints();
                    SortStandings();
                    SetStandingsPositions();

                    DisplayTeamPoints(string.Format("Points Scored by {0}:", playerTeam.GetTeamName()));
                    DisplayPoints(string.Format("\nStandings after {0}:\n", currentRound.GetRoundName()));
                }

                Console.WriteLine("{0} Class Champions:", chosenSeries.GetSeriesName());

                Entrant championshipWinner;

                List<Class> classList = chosenSeries.GetClassList();
                List<Entrant> championshipWinners = new List<Entrant>();

                List<int> championshipSpacers = new List<int>();

                foreach (Class currentClass in classList)
                {
                    championshipWinner = GetChampionshipLeader(currentClass.GetClassName());

                    if (championshipSpacers.Count() == 0)
                    {
                        championshipSpacers.Add(championshipWinner.GetCarNo().Length);
                        championshipSpacers.Add(championshipWinner.GetTeamName().Length);
                        championshipSpacers.Add(championshipWinner.GetManufacturer().Length);
                    }

                    else
                    {
                        if (championshipWinner.GetCarNo().Length > championshipSpacers[0])
                        {
                            championshipSpacers[0] = championshipWinner.GetCarNo().Length;
                        }

                        if (championshipWinner.GetTeamName().Length > championshipSpacers[1])
                        {
                            championshipSpacers[1] = championshipWinner.GetTeamName().Length;
                        }

                        if (championshipWinner.GetManufacturer().Length > championshipSpacers[2])
                        {
                            championshipSpacers[2] = championshipWinner.GetManufacturer().Length;
                        }
                    }

                    championshipWinners.Add(championshipWinner);
                }

                for (int classIndex = 0; classIndex < classList.Count(); classIndex++)
                {
                    championshipWinner = championshipWinners[classIndex];

                    Console.WriteLine("{0}: {1} {2} - {3} - {4} Points", classList[classIndex].GetClassName().PadRight(chosenSeries.GetClassSpacer(), ' '), championshipWinner.GetCarNo().PadRight(championshipSpacers[0], ' '), championshipWinner.GetTeamName().PadRight(championshipSpacers[1], ' '), championshipWinner.GetManufacturer().PadRight(championshipSpacers[2], ' '), championshipWinner.GetPoints());
                }

                SaveFinalStandings();
                Console.ReadLine();

                playGame = GetBoolean("Play Another Season?");

                if (playGame)
                {
                    seasonNumber++;
                    FileHandler.SetSeasonFolder(seasonNumber);

                    UpdateCrewStats();
                    UpdatePlayerTeam();

                    Console.WriteLine("Press Enter to Start Next Season");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Thank you for playing the First Console Alpha of the (Hopefully Happening) Global Endurance Masters Game!");
            Console.WriteLine("That's the end of the Game.");//, but you can always play again with different Cars");
            Console.WriteLine("Press Enter to Exit...");
            Console.ReadLine();
        }

        private void SetupPlayerTeam()
        {
            string teamName = "";
            
            while (teamName.Length < minimumTeamNameLength)
            {
                Console.Write("Please Enter Team Name: ");
                teamName = Console.ReadLine();

                if (teamName.Length < minimumTeamNameLength)
                {
                    Console.WriteLine("Team Name must be at least {0} Characters Long)", minimumTeamNameLength);
                }
            }

            chosenSeries = seriesList[SelectSeries(true)];

            (int teamOVR, List<Entrant> crewList) = CreateCrews(teamName, chosenSeries.GetMaxEnterableCrews());

            playerTeam = new Team(teamName, teamOVR, chosenSeries, crewList);

            playerTeam.OrderCrews();

            spacerList = playerTeam.GetSpacerList();
            
            Console.WriteLine();
            Console.WriteLine("Team Information:");
            Console.WriteLine("Team Name: {0}", playerTeam.GetTeamName());
            Console.WriteLine("Entered Series: {0}", playerTeam.GetEnteredSeries().GetSeriesName());
            
            for (int i = 0; i < playerTeam.GetTeamEntries().Count(); i++)
            {
                Console.WriteLine("Crew {0}: {1} {2} - {3}", i + 1, playerTeam.GetTeamEntries()[i].GetCarNo().PadRight(spacerList[0], ' '), playerTeam.GetTeamEntries()[i].GetCarModel().GetManufacturer().PadRight(spacerList[1], ' '), playerTeam.GetTeamEntries()[i].GetClass().GetClassName());
            }

            Console.ReadLine();
        }

        private void UpdatePlayerTeam()
        {
            List<Entrant> playerCrews = playerTeam.GetTeamEntries();

            IndexSort(playerCrews);

            if (MakeTeamChanges())
            {
                if (GetBoolean("Change Team Name?"))
                {
                    ChangeTeamName();
                }

                UpdateTeam();

                playerTeam.OrderCrews();
            }

            else
            {
                playerTeam.UpdateCrewStats(randomiser);
            }
        }

        private bool MakeTeamChanges()
        {
            //return false;

            Console.WriteLine("Make Changes to your Team?\nY - Yes\nN - No");
            Console.Write("Choice: ");

            string continueChoice = Console.ReadLine().ToUpper();

            if (continueChoice == "Y" || continueChoice == "YES")
            {
                return true;
            }

            else if (continueChoice == "N" || continueChoice == "NO")
            {
                return false;
            }

            Console.WriteLine("Invalid Option");
            return MakeTeamChanges();
        }

        private void ChangeTeamName()
        {
            string teamName = "";

            while (teamName.Length < minimumTeamNameLength)
            {
                Console.Write("Please Enter Team Name: ");
                teamName = Console.ReadLine();

                if (teamName.Length < minimumTeamNameLength)
                {
                    Console.WriteLine("Team Name must be at least {0} Characters Long)", minimumTeamNameLength);
                }
            }

            playerTeam.SetTeamName(teamName);
        }

        private void UpdateTeam()
        {
            string newCarNumber;
            int newClass, newOVR, newReliability;
            bool deleteCrew;

            Entrant currentEntrant;

            List<Entrant> playerCrews = playerTeam.GetTeamEntries();
            List<Class> classList = chosenSeries.GetClassList();

            for (int i = 0; i < playerCrews.Count(); i++)
            {
                currentEntrant = playerCrews[i];

                Console.WriteLine("Current Crew: {0} {1}", currentEntrant.GetCarNo(), currentEntrant.GetManufacturer());

                deleteCrew = false;

                if (playerCrews.Count() > 1)
                {
                    deleteCrew = GetBoolean("Delete Crew?");
                }

                if (!deleteCrew)
                {
                    newClass = currentEntrant.GetClassIndex() - 1;
                    CarModel newCarModel;

                    if (GetBoolean("Change Car Number?"))
                    {
                        newCarNumber = GetCarNumber(playerCrews);
                        currentEntrant.SetCarNumber(newCarNumber);
                    }

                    if (GetBoolean("Change Class?"))
                    {
                        newClass = SelectClass(classList);
                        currentEntrant.SetClass(classList[newClass]);

                        newCarModel = SelectCarModel(newClass);
                        currentEntrant.SetCarModel(newCarModel);
                    }

                    else if (GetBoolean("Change Car Model?"))
                    {
                        newCarModel = SelectCarModel(newClass);
                        currentEntrant.SetCarModel(newCarModel);
                    }

                    (newOVR, newReliability) = UpdateCrewStat(currentEntrant);

                    currentEntrant.SetCrewOVR(newOVR);
                    currentEntrant.SetBaseReliability(newReliability);
                    currentEntrant.SetPoints(0);
                }

                else
                {
                    playerTeam.DeleteCrew(currentEntrant);
                }
            }

            int maxCrews = 0;

            foreach (Series enteredSeries in playerTeam.GetEnteredSeriesList())
            {
                maxCrews += enteredSeries.GetMaxEnterableCrews();
            }

            string carNumber;
            int selectedClass, crewOVR, stintModifier, reliability;

            CarModel selectedModel;
            Class chosenClass;
            Entrant newCrew;

            int[] classEntrants = new int[classList.Count()];

            for (int i = 0; i < classEntrants.Length; i++)
            {
                classEntrants[i] = 0;
            }

            for (int i = 0; i < playerTeam.GetTeamEntries().Count(); i++)
            {
                classEntrants[playerTeam.GetTeamEntries()[i].GetClassIndex() - 1]++;
            }

            while (playerTeam.GetTeamEntries().Count() < maxCrews)
            {
                if (GetBoolean("Create New Crew?"))
                {
                    selectedClass = SelectClass(classList);

                    if (selectedClass == -1)
                    {
                        break;
                    }

                    else if (classEntrants[selectedClass] + 1 > 2)
                    {
                        Console.WriteLine("Too Many Entrants in this Class\n");
                    }

                    else
                    {
                        selectedModel = SelectCarModel(selectedClass);
                        chosenClass = classList[selectedClass];

                        carNumber = GetCarNumber(playerTeam.GetTeamEntries());

                        crewOVR = randomiser.Next(chosenClass.GetMinOVR(), chosenClass.GetMaxOVR() + 1);
                        stintModifier = randomiser.Next(1, 6);
                        reliability = randomiser.Next(24, 31);

                        newCrew = new Entrant(carNumber, playerTeam.GetTeamName(), playerTeam.GetTeamOVR(), crewOVR, stintModifier, reliability, chosenSeries.GetSeriesIndex(), selectedModel, chosenClass);
                        playerTeam.AddCrew(newCrew);

                        classEntrants[selectedClass]++;
                    }
                }

                else
                {
                    break;
                }
            }

            playerTeam.SetSpacerList();
        }

        private bool GetBoolean(string outputString)
        {
            Console.Write("{0}\nY - Yes\nN - No\nChoice: ", outputString);
            string choice = Console.ReadLine().ToUpper();
            Console.WriteLine();

            if (choice == "Y" || choice == "YES")
            {
                return true;
            }

            if (choice == "N" || choice == "NO")
            {
                return false;
            }

            Console.WriteLine("Invalid Option");
            return GetBoolean(outputString);
        }

        // Season End Functions

        private void UpdateCrewStats()
        {
            // Update Stats
            Entrant currentEntrant;

            IndexSort(entryList);

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];

                int newOVR, newReliability;

                (newOVR, newReliability) = UpdateCrewStat(currentEntrant);

                currentEntrant.SetCrewOVR(newOVR);
                currentEntrant.SetBaseReliability(newReliability);
            }

            // Rewrite Stats to Appropriate Season Folder

            List<Class> classList = chosenSeries.GetClassList();

            string folderPath = Path.Combine(CommonData.GetSeasonFolder(), "Entrants"),
                currentClassName = classList[0].GetClassName(),
                fileName = Path.Combine(folderPath, "Class 1.csv"),
                writeString = "";

            int classIndex = 1;

            Directory.CreateDirectory(folderPath);

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];

                if (currentEntrant.GetTeamName() == playerTeam.GetTeamName())
                {
                    break;
                }

                if (currentEntrant.GetClass().GetClassName() != currentClassName)
                {
                    FileHandler.WriteFile(writeString, fileName);

                    currentClassName = classList[classIndex].GetClassName();
                    classIndex++;

                    writeString = "";
                    fileName = Path.Combine(folderPath, string.Format("Class {0}.csv", classIndex));
                }

                writeString += string.Format("{0},{1},{2},{3},,{4},{5},,{6},,{7}\n", currentClassName, currentEntrant.GetCarNo(),
                    currentEntrant.GetTeamName(), currentEntrant.GetCarModel().GetModelName(), currentEntrant.GetTeamOVR(),
                    currentEntrant.GetCrewOVR(), currentEntrant.GetSRM(), currentEntrant.GetBaseReliability());
            }

            FileHandler.WriteFile(writeString, fileName);
        }

        private (int, int) UpdateCrewStat(Entrant currentEntrant)
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


        // Display Functions

        private void DisplaySeriesCalendar()
        {
            Series displaySeries = seriesList[SelectSeries(false)];

            Console.WriteLine("\n{0} Calendar:", displaySeries.GetSeriesName());

            Round outputRound;
            string roundLength = "";

            for (int i = 0; i < displaySeries.GetCalendar().Count(); i++)
            {
                outputRound = displaySeries.GetCalendar()[i];

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

                Console.WriteLine("R{0}: {1} - {2}", Convert.ToString(i + 1).PadRight(2, ' '), outputRound.GetRoundName().PadRight(displaySeries.GetCalendarSpacer(), ' '), roundLength);
            }

            Console.ReadLine();
        }

        private void DisplayTeamEntrants(string outputString)
        {
            Console.WriteLine(outputString);

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

            spacerList = playerTeam.GetSpacerList();

            for (int i = 0; i < teamEntrants.Count(); i++)
            {
                currentEntrant = teamEntrants[i];

                if (currentEntrant.GetRacing())
                {
                    (posString, classPosString) = currentEntrant.GetCurrentPosition();

                    Console.WriteLine("Crew {0}: {1} {2} - {3} Overall / {4} In {5}", i + 1, currentEntrant.GetCarNo().PadRight(spacerList[0], ' '), currentEntrant.GetManufacturer().PadRight(spacerList[1], ' '), posString.PadRight(3, ' '), classPosString.PadRight(3, ' '), currentEntrant.GetClass().GetClassName());
                }
            }

            Console.ReadLine();
        }

        private void DisplayEntrants(string outputString, string saveFile)
        {
            Console.WriteLine(outputString);

            string posString, classPosString;

            Entrant currentEntrant;

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];

                if (currentEntrant.GetRacing())
                {
                    (posString, classPosString) = currentEntrant.GetCurrentPosition();

                    Console.WriteLine("{0} Overall / {1} In {6} - {2} {3} - {4} - {5}", posString.PadRight(3, ' '), classPosString.PadRight(3, ' '), currentEntrant.GetCarNo().PadRight(entrantSpacers[0], ' '), currentEntrant.GetTeamName().PadRight(entrantSpacers[1], ' '), currentEntrant.GetManufacturer().PadRight(entrantSpacers[2], ' '), currentEntrant.GetOVR(), currentEntrant.GetClass().GetClassName().PadRight(chosenSeries.GetClassSpacer(), ' '));
                }
            }

            SaveResults(saveFile);
            Console.ReadLine();
        }

        private void DisplayTeamPoints(string outputString)
        {
            Console.WriteLine(outputString);

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

            spacerList = playerTeam.GetSpacerList();

            for (int i = 0; i < teamEntrants.Count(); i++)
            {
                currentEntrant = teamEntrants[i];

                Console.WriteLine("Crew {0}: {1} {2} - {3} Points - {4} in {5}", i + 1, currentEntrant.GetCarNo().PadRight(spacerList[0], ' '), currentEntrant.GetManufacturer().PadRight(spacerList[1], ' '), Convert.ToString(currentEntrant.GetPoints()).PadRight(3, ' '), currentEntrant.GetStandingsPosition().PadRight(3, ' '), currentEntrant.GetClass().GetClassName());
            }

            Console.ReadLine();
        }

        private void DisplayPoints(string outputString)
        {
            Console.WriteLine(outputString);

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

                Console.WriteLine("{0}: {1} {2} - {3} - {4} Points", currentEntrant.GetStandingsPosition().PadRight(3, ' '), currentEntrant.GetCarNo().PadRight(entrantSpacers[0], ' '), currentEntrant.GetTeamName().PadRight(entrantSpacers[1], ' '), currentEntrant.GetManufacturer().PadRight(entrantSpacers[2], ' '), currentEntrant.GetPoints());
                classPosition++;
            }

            SaveStandings();
            Console.ReadLine();
        }


        // Crew Creation Functions

        private int SelectSeries(bool selectCalendar)
        {
            int selectedSeries;

            Console.WriteLine("\nAvailable Series:");

            for (int i = 0; i < seriesList.Count(); i++)
            {
                Console.WriteLine("{0} - {1}", i + 1, seriesList[i].GetSeriesName());
            }

            if (selectCalendar)
            {
                Console.WriteLine("C - View Calendar");
            }

            Console.Write("Please Select a Series: ");
            string desiredSeries = Console.ReadLine().ToUpper();
            bool validSeries = int.TryParse(desiredSeries, out selectedSeries);

            if (validSeries && selectedSeries > 0 && selectedSeries <= seriesList.Count())
            {
                return selectedSeries - 1;
            }

            else if (selectCalendar && desiredSeries == "C")
            {
                DisplaySeriesCalendar();
                return SelectSeries(selectCalendar);
            }

            Console.WriteLine("Invalid Series Selected\n");
            return SelectSeries(selectCalendar);
        }

        private (int, List<Entrant>) CreateCrews(string teamName, int maxCrews)
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
                    return (teamOVR, crewList);
                }

                else if (classEntrants[selectedClass] + 1 > 2)
                {
                    Console.WriteLine("Too Many Entrants in this Class\n");
                }

                else
                {
                    selectedModel = SelectCarModel(selectedClass);
                    chosenClass = classList[selectedClass];
                    
                    string carNumber = GetCarNumber(crewList);

                    crewOVR = randomiser.Next(chosenClass.GetMinOVR(), chosenClass.GetMaxOVR() + 1);
                    stintModifier = randomiser.Next(1, 6);
                    reliability = randomiser.Next(24, 31);

                    newCrew = new Entrant(carNumber, teamName, teamOVR, crewOVR, stintModifier, reliability, chosenSeries.GetSeriesIndex(), selectedModel, chosenClass);
                    crewList.Add(newCrew);

                    classEntrants[selectedClass]++;
                }
            }

            return (teamOVR, crewList);
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

            int modelNumber = 1, platformSpacer = 0;

            List<string> eligiblePlatforms = selectedClass.GetEligiblePlatforms();
            List<CarModel> modelList = chosenSeries.GetCarModelList(),
                availableModels = new List<CarModel>();

            spacerList = chosenSeries.GetCarModelSpacers();

            if (eligiblePlatforms.Count() > 1)
            {
                foreach (string eligiblePlatform in eligiblePlatforms)
                {
                    if (eligiblePlatform.Length > platformSpacer)
                    {
                        platformSpacer = eligiblePlatform.Length;
                    }
                }
            }

            else
            {
                platformSpacer = eligiblePlatforms[0].Length;
            }

            Console.WriteLine("\nEligible Car Models"); 

            for (int i = 0; i < modelList.Count(); i++)
            {
                if (eligiblePlatforms.Contains(modelList[i].GetPlatform()))
                {
                    availableModels.Add(modelList[i]);

                    Console.WriteLine("{0} - {1} - {2} - {3}", Convert.ToString(modelNumber).PadRight(2, ' '), modelList[i].GetPlatform().PadRight(platformSpacer, ' '), modelList[i].GetManufacturer().PadRight(spacerList[0], ' '), modelList[i].GetModelName().PadRight(spacerList[1], ' '));
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

        private string GetCarNumber(List<Entrant> crewList)
        {
            Console.Write("Please Enter the Desired Car Number: #");
            string desiredNumber = Console.ReadLine();

            desiredNumber = desiredNumber.Replace("#", "");

            if (UniqueNumber(desiredNumber, crewList))
            {
                return "#" + desiredNumber;
            }

            Console.WriteLine("Invalid / Non-Unique Car Number\n");
            return GetCarNumber(crewList);
        }

        private bool UniqueNumber(string desiredNumber, List<Entrant> crewList)
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

                for (int i = 0; i < 8; i++)
                {
                    filePath = Path.Combine(CommonData.GetSetupPath(), chosenSeries.GetFolderName(), "Entrants", "Class " + (i + 1) + ".csv");

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
                        for (int j = 0; j < totalUsedNumbers; j++)
                        {
                            if (usedNumbers[j] != "")
                            {
                                carNumber = usedNumbers[j].Split(',');

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

                if (uniqueCarNumber)
                {
                    for (int i = 0; i < crewList.Count(); i++)
                    {
                        if (crewList[i].GetCarNo() == checkNumber)
                        {
                            uniqueCarNumber = false;
                            break;
                        }
                    }
                }
            }

            else
            {
                uniqueCarNumber = false;
            }

            return uniqueCarNumber;
        }


        // Entry List Functions

        private void LoadEntryList()
        {
            List<Class> classes = chosenSeries.GetClassList();

            if (entryList == null)
            {
                entryList = new List<Entrant>();
            }

            else
            {
                entryList.Clear();
            }

            CarModel carModel;
            Class enteredClass;

            Entrant newEntrant;

            string basePath, filePath;
            string[] classEntrants, entrantData;
            int index = 0;

            if (seasonNumber == 1)
            {
                basePath = Path.Combine(CommonData.GetSetupPath(), chosenSeries.GetFolderName(), "Entrants");
            }

            else
            {
                basePath = Path.Combine(CommonData.GetSeasonFolder(), "Entrants");
            }

            entrantSpacers = new List<int>();

            for (int classIndex = 0; classIndex < classes.Count(); classIndex++)
            {
                filePath = Path.Combine(basePath, "Class " + (classIndex + 1) + ".csv");

                classEntrants = FileHandler.ReadFile(filePath);

                for (int i = 0; i < classEntrants.Length; i++)
                {
                    if (classEntrants[i] != "")
                    {
                        entrantData = classEntrants[i].Split(',');

                        carModel = chosenSeries.GetCarModel(entrantData[3]);
                        enteredClass = chosenSeries.GetClass(entrantData[0]);

                        newEntrant = new Entrant(entrantData[1], entrantData[2], Convert.ToInt32(entrantData[5]), Convert.ToInt32(entrantData[6]), Convert.ToInt32(entrantData[8]), Convert.ToInt32(entrantData[10]), index, chosenSeries.GetSeriesIndex(), carModel, enteredClass);
                        index++;

                        UpdateEntrantSpacers(newEntrant);

                        entryList.Add(newEntrant);
                    }
                }
            }

            for (int i = 0; i < playerTeam.GetTeamEntries().Count(); i++)
            {
                newEntrant = playerTeam.GetTeamEntries()[i];

                newEntrant.SetIndex(index);
                newEntrant.SetRound(currentRound);

                UpdateEntrantSpacers(newEntrant);

                entryList.Add(newEntrant);

                index++;
            }
        }

        private void UpdateEntrantSpacers(Entrant newEntrant)
        {
            if (entrantSpacers.Count() == 0)
            {
                entrantSpacers.Add(newEntrant.GetCarNo().Length);
                entrantSpacers.Add(newEntrant.GetTeamName().Length);
                entrantSpacers.Add(newEntrant.GetManufacturer().Length);
            }

            else
            {
                if (newEntrant.GetCarNo().Length > entrantSpacers[0])
                {
                    entrantSpacers[0] = newEntrant.GetCarNo().Length;
                }

                if (newEntrant.GetTeamName().Length > entrantSpacers[1])
                {
                    entrantSpacers[1] = newEntrant.GetTeamName().Length;
                }

                if (newEntrant.GetManufacturer().Length > entrantSpacers[2])
                {
                    entrantSpacers[2] = newEntrant.GetManufacturer().Length;
                }
            }
        }
    
        private void SetEntryList()
        {
            IndexSort(entryList);
            racingCount = 0;

            for (int i = 0; i < entryList.Count(); i++)
            {
                entryList[i].SetRound(currentRound);

                if (currentRound.GetNamedClasses().Contains(entryList[i].GetClass().GetClassName()))
                {
                    entryList[i].SetRacing(true);
                    racingCount++;
                }

                else
                {
                    entryList[i].SetRacing(false);
                }
            }

            IsRacingSort(entryList);
        }


        // Points System Functions

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

                if (classPosition < pointsSystem.Count() && entryList[i].GetOVR() > 100 && entryList[i].GetRacing())
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


        // Set Position Functions

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


        // Results + Standings Saving

        private void SaveResults(string stintName)
        {
            string filePath = Path.Combine(currentRound.GetFolder(), string.Format("{0} - {1}.csv", fileNumber, stintName)), writeString = "",
                overallPosition, classPosition;

            fileNumber++;

            Entrant currentEntrant;

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];

                if (currentEntrant.GetRacing())
                {
                    (overallPosition, classPosition) = currentEntrant.GetCurrentPosition();

                    writeString += string.Format("{0},{1},{2},{3} {4},{5},,{6}", overallPosition, currentEntrant.GetClass().GetClassName(), classPosition, currentEntrant.GetCarNo(), currentEntrant.GetTeamName(), currentEntrant.GetCarModel().GetModelName(), currentEntrant.GetOVR()); ;

                    if (i < entryList.Count() - 1)
                    {
                        writeString += "\n";
                    }
                }
            }

            FileHandler.WriteFile(writeString, filePath);
        }

        private void SaveStandings()
        {
            List<Class> classList = chosenSeries.GetClassList();

            string folderPath = Path.Combine(currentRound.GetFolder(), "Post Race Standings"),
                currentClassName = classList[0].GetClassName(),
                fileName = Path.Combine(folderPath, string.Format("Class 1 - {0}.csv", currentClassName)),
                writeString = "";

            int classIndex = 1;

            Entrant currentEntrant;

            Directory.CreateDirectory(folderPath);

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];
                
                if (currentEntrant.GetClass().GetClassName() != currentClassName)
                {
                    FileHandler.WriteFile(writeString, fileName);
                    
                    currentClassName = classList[classIndex].GetClassName();
                    classIndex++;
                    
                    writeString = "";
                    
                    fileName = Path.Combine(folderPath, string.Format("Class {0} - {1}.csv", classIndex, currentClassName));
                }

                writeString += string.Format("{0},{1} {2},{3},{4}\n", currentEntrant.GetStandingsPosition(), currentEntrant.GetCarNo(),
                    currentEntrant.GetTeamName(), currentEntrant.GetCarModel().GetModelName(), currentEntrant.GetPoints());
            }

            FileHandler.WriteFile(writeString, fileName);
        }

        private void SaveFinalStandings()
        {
            List<Class> classList = chosenSeries.GetClassList();

            string folderPath = Path.Combine(CommonData.GetSeasonFolder(), "Final Standings"),
                currentClassName = classList[0].GetClassName(), writeString = "",
                fileName = Path.Combine(folderPath, string.Format("Class 1 - {0}.csv", currentClassName));

            int classIndex = 1;

            Entrant currentEntrant;

            Directory.CreateDirectory(folderPath);

            for (int i = 0; i < entryList.Count(); i++)
            {
                currentEntrant = entryList[i];

                if (currentEntrant.GetClass().GetClassName() != currentClassName)
                {
                    FileHandler.WriteFile(writeString, fileName);

                    currentClassName = classList[classIndex].GetClassName();
                    classIndex++;

                    writeString = "";

                    fileName = Path.Combine(folderPath, string.Format("Class {0} - {1}.csv", classIndex, currentClassName));
                }

                writeString += string.Format("{0},{1} {2},{3},{4}\n", currentEntrant.GetStandingsPosition(), currentEntrant.GetCarNo(),
                    currentEntrant.GetTeamName(), currentEntrant.GetCarModel().GetModelName(), currentEntrant.GetPoints());
            }

            FileHandler.WriteFile(writeString, fileName);
        }


        // Sort Functions

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

        private void IsRacingSort(List<Entrant> entryList)
        {
            bool swap;

            for (int i = 0; i < entryList.Count() - 1; i++)
            {
                swap = false;

                for (int j = 0; j < entryList.Count() - i - 1; j++)
                {
                    if (entryList[j].GetRacingIndex() > entryList[j + 1].GetRacingIndex())
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

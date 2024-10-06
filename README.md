# ConsoleAlphaV1
Console Alpha V1 of a Game I desire to make in the future

Written In C# in order to use parts of another program that does this but with a UI and other editor features that weren't necessary for this.

Requires Contents of Setup Folder to run, with 'Series.csv' located where it is and with the same name, else the Program won't find the Root Folder.

It is possible to change the Included Entrants, via the Entrants Folder, and the Class X Files in there (Up to 8 Classes Possible)

Ideally, its Download, Unzip, Plonk Somewhere, open Visual Studio, and hit F5.

**PLEASE NOTE**
* As of V1.4.1, the File Structure of the Entry Lists has Changed, necessitating the redownload of the Setup Folder, if you just download the Code Folder


**How To Play**
* Team Creation
  - At the start of the Game, you will be asked to enter a team name, this can be changed later, and must be at least 5 Characters Long.
  - You then must Select a Series to Compete in, you can view the Calendar of Each Series before selecting, by entering 'c' on the Series Select Menu, then selecting a Series on the 2nd menu
  - You will then be prompted to Create Crews for your Team, in order to play the Game, you must have at least 1 Crew
    - You will be asked to Select a Class for the Crew to compete in
    - You will then be asked to Select an appropriate Car Model
    - You will then need to Choose a Number for your Car, which must be a valid Integer Number, and also not used by another Entrants, either AI or your own.
    - You will then be returned to the Class Selector, where you can enter another Crew, or confirm your Team.
  - When you have Confirmed your Team, the Crews will be sorted by Car Number and Class, for example:
    - Created Order
      - LMGT3 - #13 Lamborghini
      - LMGTP - #37 Lamborghini
      - LMGTP - #19 Lamborghini
    - Output Order
      - LMGTP - #19 Lamborghini
      - LMGTP - #37 Lamborghini
      - LMGT3 - #13 Lamborghini
* Team Editing
  - At the end of each season, you will have the option of Playing a New Season, or exiting the game. If you choose to play another season, you will be asked if you want to make any changes to your team. If you select yes, the following options will appear (in this order)
    - Change Team Name - Asks if you would like to Change the Name of your Team
    - Each Crew will then be ran through in Order, asking if you would like to make any changes to the Crew, with the following options
      - Change Car Number
      - Change Car Class
      - Change Car Model (Will Automatically Trigger if Entered Class is Changed)
    - After that, there will then be the option to add more Crews, if you aren't already at the limit.
    - Crew Stats for both Player and Computer Crews will be updated between seasons.
* Season Gameplay
  - This isn't too fleshed out at the moment, in part due to, Race Simulation is split in 2 main parts, Qualifying, and the Race, no Practice or Warm Up or anything.
  - For those Interested in Functional Details:
    - Qualifying is done as 3 'Sessions', only the 3rd of which is Saved as the Qualifying Results
    - The Race is done as 2x 'Stints', where a stint is 30 Minutes, and x is the length of the race in Hours (ie a 24 Hour Race is 48 Stints)
    - The Simulator is taken straight from GEM V3, with the only appreciable changes being to Variable Names.
      - GEM V3 is not currently publically available for Download, however parts (like the afforementioned Simulator, and some other Classes) are taken from the program for use here.
    - Cars can 'enter' their garage for a period of time
    - Cars can either finish, be a Non Classified Finisher (they finished the race in the garage), or a Retirement. There currently is no Scrutineering Functionality.
  - Positions for Each Team Car are displayed before Full Results, to make it easier for the Player to find their Cars.
* Results Folder Structure
  - Each Save is stored in its own Folder inside the Results Folder, which is Auto Generated at the Start of the Game, and then each Season Folder within that folder ahead of each season.
  - A .csv File is Saved in it's relevant Season Folder ahead of Each Season that holds the Team Data, it serves no functional purpose (though that may change), it's mainly to you can see the stats of your Crews compared to your rivals.
  - In the Season Folder, Race Results + Post Race Standings are stored, all stored as .csv Files.
  - Final Standings are also Stored at the End of the Season, in it's own Folder in the Season Folder.
  - Ahead of Each New Season (Season 2 Onwards) an Entry List Folder is created, where all the Updated Stats for each Computer Crew is stored, again, no functional purpose, just for story telling, if you want to see how each Crew evolved throughout the save.
  - Results Folder Structure Run Down
    - Team Name
      - Season 1
        - Team Data File
        - Round 1
          - Qualifying Results
          - Half Distance
          - Race Results
          - Post Race Standings
            - Class 1 (ex LMGTP)
            - Class 2 (ex LMGT3)

**Changelog**
* V1
  - Original Upload
* V1.1
  - Added Season Calendar Viewing
  - Added Spacers for Each Series, for neatened Display
* V1.2
  - Fixed/Re-added Select Classes at each Event / Ability for Classes to miss rounds
* V1.3
  - Updated File Read + Write System with new FileHandler Class
  - Added Results + Standings Saving
    - Results Saved after Qualifying, Half Distance + Race Results
    - Standings Saved after Each Round, and Season End
* V1.4
  - Added Ability to Complete Multiple Seasons
  - Console Will now Clear before the Start of Each Season
  - Results Folder Structure tweaked to include Season Folders within Team Folders
  - Updated Repository Folder Structure
    - Allows to Update only the Code or Setup Files, not having to Update Both
* V1.4.1
  - Made Code Changes to how Results are Displayed
  - Crew Stat Updates + Resaving for both Game Crews, and Player Crews
    - Game Crews have Entry List Written to relevant Season Folder for Season 2 Onwards
  - Changed File Structure of Entry List Files to split Team + Crew OVR Scores
  - FIXED Player Crews not having Points Totals reset between Seasons
* V1.4.2
  - Added Player Team/Crew Editing:
    - Players can now change their Team Name
    - Players can now Delete Crews
    - Players can now change the Number of a Crew Car 
    - Players can now change the Class of a Crew
    - Players can now change the Car Model of a Crew
  - FIXED Multiple Cars in Player Team able to use the Same Car Number
* V1.5
  - Improved Standings
    - Added Best Results
    - Added Countback
  - FIXED Player being able to play without any Crews
* V1.5.1
  - Further Standings Improvements
    - Added Manufacturer Standings
    - Changed Standings Saving
      - Now Stored in Folders by Class
  - FIXED Entrant Best Results being Updated even if Entrant wasn't Racing
  - FIXED Entrant Best Results not being Reset if Player makes no Team Changes
  - FIXED Class Names not being properly Spaced in Player Team Standings Output

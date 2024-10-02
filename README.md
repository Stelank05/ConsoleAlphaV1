# ConsoleAlphaV1
Console Alpha V1 of a Game I desire to make in the future

Written In C# in order to use parts of another program that does this but with a UI and other editor features that weren't necessary for this.

Requires Contents of Setup Folder to run, with 'Series.csv' located where it is and with the same name, else the Program won't find the Root Folder.

It is possible to change the Included Entrants, via the Entrants Folder, and the Class X Files in there (Up to 8 Classes Possible)

Ideally, its Download, Unzip, Plonk Somewhere, open Visual Studio, and hit F5.

**PLEASE NOTE**:
* As of V1.4.1, the File Structure of the Entry Lists has Changed, necessitating the redownload of the Setup Folder, if you just download the Code Folder


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

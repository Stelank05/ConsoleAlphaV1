using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_Alpha_V1
{
    public class Series
    {
        string seriesName, folderName;
        int maxCrews;

        List<CarModel> modelList;
        List<Class> classList;
        List<Round> calendar;

        public Series(string sN, string fN, int mC)
        {
            seriesName = sN;
            folderName = fN;

            maxCrews = mC;

            LoadClasses();
            LoadCarModels();
            LoadCalendar();
        }

        public Series() { }

        private void LoadClasses()
        {
            classList = new List<Class>();
            Class newClass;

            List<string> platformList;
            string[] platforms;

            string classFile = Path.Combine(CommonData.GetSetupPath(), folderName, "Class Setup.csv");

            bool fileRead = false;
            string[] classData = new string[1];// = File.ReadAllLines(classFile);
            string[] splitLine;

            while (!fileRead)
            {
                try
                {
                    classData = File.ReadAllLines(classFile);
                    fileRead = true;
                }

                catch
                {
                    Console.WriteLine("Please close File '{0}'", classFile);
                    fileRead = false;
                    Console.ReadLine();
                }
            }

            for (int i = 1; i < classData.Length; i++)
            {
                splitLine = classData[i].Split(',');

                if (splitLine[1] != "")
                {
                    platformList = new List<string>();
                    platforms = splitLine[3].Split('/');

                    foreach (string platform in platforms)
                    {
                        platformList.Add(platform);
                    }

                    newClass = new Class(splitLine[1], Convert.ToInt32(splitLine[5]), Convert.ToInt32(splitLine[6]), Convert.ToInt32(splitLine[8]), Convert.ToInt32(splitLine[9]), Convert.ToInt32(splitLine[10]),
                        Convert.ToInt32(splitLine[12]), Convert.ToInt32(splitLine[13]), Convert.ToInt32(splitLine[15]), Convert.ToInt32(splitLine[16]), Convert.ToInt32(splitLine[17]), i, platformList);

                    classList.Add(newClass);
                }
            }
        }

        private void LoadCarModels()
        {
            modelList = new List<CarModel>();
            CarModel newCarModel;

            Class enteredClass;

            string carModelFile = Path.Combine(CommonData.GetSetupPath(), folderName, "Available Cars.csv");

            bool fileRead = false;
            string[] modelData = new string[1];// = File.ReadAllLines(classFile);
            string[] splitLine;

            while (!fileRead)
            {
                try
                {
                    modelData = File.ReadAllLines(carModelFile);
                    fileRead = true;
                }

                catch
                {
                    Console.WriteLine("Please close File '{0}'", carModelFile);
                    fileRead = false;
                    Console.ReadLine();
                }
            }

            for (int i = 1; i < modelData.Length; i++)
            {
                splitLine = modelData[i].Split(',');

                enteredClass = GetEnteredClass(splitLine[2]);

                newCarModel = new CarModel(splitLine[0], splitLine[1], splitLine[2], Convert.ToInt32(splitLine[3]), Convert.ToInt32(splitLine[4]), Convert.ToInt32(splitLine[5]), enteredClass);
                modelList.Add(newCarModel);
            }
        }

        private void LoadCalendar()
        {
            calendar = new List<Round>();

            string calendarFile = Path.Combine(CommonData.GetSetupPath(), folderName, "Calendar.csv");

            try
            {
                string[] calendarData = File.ReadAllLines(calendarFile), roundData;

                Round newRound;

                List<string> racingClasses;

                for (int i = 1; i < calendarData.Length; i++)
                {
                    roundData = calendarData[i].Split(',');

                    racingClasses = new List<string>();

                    for (int j = 9; j < roundData.Length; j++)
                    {
                        racingClasses.Add(roundData[j]);
                    }

                    newRound = new Round(roundData[0], Convert.ToInt32(roundData[1]), roundData[5], Convert.ToInt32(roundData[2]), Convert.ToInt32(roundData[3]), roundData[7], racingClasses);
                    calendar.Add(newRound);
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("Please Close '{0}'", calendarFile);
                Console.ReadLine();
                LoadCalendar();
            }
        }

        public string GetSeriesName()
        {
            return seriesName;
        }

        public string GetFolderName()
        {
            return folderName;
        }

        public List<Round> GetCalendar()
        {
            return calendar;
        }

        public int GetMaxEnterableCrews()
        {
            return maxCrews;
        }

        public List<Class> GetClassList()
        {
            return classList;
        }

        public Class GetClass(string enteredClass)
        {
            foreach (Class checkClass in classList)
            {
                if (checkClass.GetClassName() == enteredClass)
                {
                    return checkClass;
                }
            }

            return new Class();
        }

        private Class GetEnteredClass(string modelPlatform)
        {
            for (int i = 0; i < classList.Count(); i++)
            {
                if (classList[i].GetEligiblePlatforms().Contains(modelPlatform))
                {
                    return classList[i];
                }
            }

            return new Class();
        }

        public List<CarModel> GetCarModelList()
        {
            return modelList;
        }

        public CarModel GetCarModel(string enteredModel)
        {
            foreach (CarModel checkModel in modelList)
            {
                if (checkModel.GetModelName() == enteredModel)
                {
                    return checkModel;
                }
            }

            return new CarModel();
        }
    }
}

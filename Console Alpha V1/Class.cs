using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Alpha_V1
{
    public class Class
    {
        string className;
        int incidentRangeModifier, dnfRateModifier,
            stintRangeHigh, stintRangeLow, stintRangeIncident,
            classIndex,
            minOVR, maxOVR,
            wecDistanceToPit, imsaDistanceToPit, lapDistanceToPit,
            wecMinStintScore, wecMaxStintScore, wecGarageStintScore,
            imsaMinStintScore, imsaMaxStintScore, imsaGarageStintScore,
            lapMinStintScore, lapMaxStintScore, lapGarageStintScore;

        List<string> eligiblePlatforms;

        public Class(string cN, int iRM, int dnfScore, int sRH, int sRL, int sRI, int minO, int maxO, int wecDTP, int imsaDTP, int lapDTP, int cI, List<string> eP)
        {
            className = cN;

            incidentRangeModifier = iRM;
            dnfRateModifier = dnfScore;

            stintRangeHigh = sRH;
            stintRangeLow = sRL;
            stintRangeIncident = sRI;

            classIndex = cI;

            minOVR = minO;
            maxOVR = maxO;

            wecDistanceToPit = wecDTP;
            imsaDistanceToPit = imsaDTP;
            lapDistanceToPit = lapDTP;

            eligiblePlatforms = eP;
        }

        public Class() { }

        public void SetClassName(string newClassName)
        {
            className = newClassName;
        }

        public string GetClassName()
        {
            return className;
        }

        public List<string> GetEligiblePlatforms()
        {
            return eligiblePlatforms;
        }

        public void SetIRM(int newIRM)
        {
            incidentRangeModifier = newIRM;
        }

        public int GetIRM()
        {
            return incidentRangeModifier;
        }

        public void SetDNFRM(int newDNFRM)
        {
            dnfRateModifier = newDNFRM;
        }

        public int GetDNFRM()
        {
            return dnfRateModifier;
        }

        public void SetClassIndex(int CI)
        {
            classIndex = CI;
        }

        public int GetClassIndex()
        {
            return classIndex;
        }

        public void SetSRLow(int newSRL)
        {
            stintRangeLow = newSRL;
        }

        public int GetSRLow()
        {
            return stintRangeLow;
        }

        public void SetSRHigh(int newSRH)
        {
            stintRangeHigh = newSRH;
        }

        public int GetSRHigh()
        {
            return stintRangeHigh;
        }

        public void SetSRInc(int SRI)
        {
            stintRangeIncident = SRI;
        }

        public int GetSRInc()
        {
            return stintRangeIncident;
        }

        public void SetMinOVR(int OVR)
        {
            minOVR = OVR;
        }

        public int GetMinOVR()
        {
            return minOVR;
        }

        public void SetMaxOVR(int OVR)
        {
            maxOVR = OVR;
        }

        public int GetMaxOVR()
        {
            return maxOVR;
        }

        public void SetWECDTP(int newDTP)
        {
            wecDistanceToPit = newDTP;
        }

        public int GetWECDTP()
        {
            return wecDistanceToPit;
        }

        public void SetIMSADTP(int newDTP)
        {
            imsaDistanceToPit = newDTP;
        }

        public int GetIMSADTP()
        {
            return imsaDistanceToPit;
        }

        public void SetLapDTP(int newDTP)
        {
            lapDistanceToPit = newDTP;
        }

        public int GetLapDTP()
        {
            return lapDistanceToPit;
        }

        public void SetWECGarageValues(int V1, int V2, int V3)
        {
            wecMinStintScore = V1;
            wecMaxStintScore = V2;
            wecGarageStintScore = V3;
        }

        public (int, int, int) GetWECGarageValues()
        {
            return (wecMinStintScore, wecMaxStintScore, wecGarageStintScore);
        }

        public void SetIMSAGarageValues(int V1, int V2, int V3)
        {
            imsaMinStintScore = V1;
            imsaMaxStintScore = V2;
            imsaGarageStintScore = V3;
        }

        public (int, int, int) GetIMSAGarageValues()
        {
            return (imsaMinStintScore, imsaMaxStintScore, imsaGarageStintScore);
        }

        public void SetLapGarageValues(int V1, int V2, int V3)
        {
            lapMinStintScore = V1;
            lapMaxStintScore = V2;
            lapGarageStintScore = V3;
        }

        public (int, int, int) GetLapGarageValues()
        {
            return (lapMinStintScore, lapMaxStintScore, lapGarageStintScore);
        }
    }
}

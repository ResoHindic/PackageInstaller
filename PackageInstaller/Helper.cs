using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PackageInstaller.InstallOrderResult;

namespace PackageInstaller
{
    public class Helper
    {
        /*
         * Main logic on sorting the passed in list.
         * 
         * My Solution: 
         * I go though the list of combinations (package: dependency).
         * Check to see if dependency is null for a specific combo, if so, I add the package to return List.
         * If dependency is not null, I check to see if that dependency is already in the return List.
         *      If yes, I add the package to the return List.
         *      If no, I skip to the next item.
         */
        public static InstalOrderResults SeperatePackageVsDependency(List<string> list)
        {
            List<string> installedItems = new List<string>();
            StringBuilder builder = new StringBuilder();

            int listSize = list.Count();
            string installResult = "";

            for (int loopTimes = 1; loopTimes <= listSize; loopTimes++)
            {

                List<string> removeFromList = new List<string>();

                foreach (string item in list)
                {
                    string[] combo = item.Split(new[] { ":" }, StringSplitOptions.None);

                    string package = combo[0].Trim();
                    string dependency = combo[1].Trim();

                    if (dependency == "")
                    {
                        if(installResult == "")
                        {                            
                            installResult = builder.Append(package).ToString();
                        }
                        else
                        {
                            installResult = builder.AppendFormat(", {0}", package).ToString();
                        }                        
                        installedItems.Add(package);
                        removeFromList.Add(item);
                    }
                    else
                    {
                        if (installedItems.Contains(dependency))
                        {
                            installResult = builder.AppendFormat(", {0}", package).ToString();
                            installedItems.Add(package);
                            removeFromList.Add(item);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                list.RemoveAll(x => removeFromList.Contains(x));
            }

            InstalOrderResults result;
            result.isValid = CheckIsListsAreSameLenght(listSize, installedItems);
            result.installOrder = installResult;

            return result;
        }

        /*
        * Invalid Combo (infinite loop):
        * 
        * I loop through the List of combos List.Count() times
        *  (this ensures that we loop through the list enough times to get all combinations)
        *  
        *  -If _installedItems List.Count() is samaller than combo List.Count(), we know that some
        *   of the packages/ dependencies were not added to _installedItems List do to a loop.
        */
        private static bool CheckIsListsAreSameLenght(int originalListSize, List<string> generatedList)
        {
            if((originalListSize > 0) && (generatedList.Count() > 0 || generatedList != null))
            {
                if(originalListSize == generatedList.Count())
                return true;
            }
            return false;
        }        
    }
}

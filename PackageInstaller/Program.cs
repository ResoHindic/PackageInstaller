using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PackageInstaller
{
    class Program
    {
        static StringBuilder _installOrder;
        static List<string> _installedItems;

        static void Main(string[] args)
        {
            string input;
            string inputText = "";
            List<string> listOfInputCombinations = null;

            Console.WriteLine("Please provide the Install List in following format:\n [ \"Package: Dependency\", \"Package2: Dependency2\", \"Package: \" ]");
            
            // If user proved text, store it and move on
            // Else, print an error back to the user.
            if ((input = Console.ReadLine()) != "")
            {
                inputText = input;
            }
            else
            {
                Console.WriteLine("No Input Provided! ... Please try again, format EX: \n [ \"Package: Dependency\", \"Package2: Dependency2\", \"Package: \" ]");
                Environment.Exit(1);
            }
            
            try
            {
                // run the JSon serilizer to comma separate the input
                // and store it into a List of strings.
                listOfInputCombinations = JsonConvert.DeserializeObject<List<string>>(inputText);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to parse through the input, please make sure it follows the exact format \n [ \"Package: Dependency\", \"Package2: Dependency2\", \"Package: \" ]");
                Environment.Exit(1);
            }

            SeperatePackageVsDependency(listOfInputCombinations);

            Console.WriteLine("Order of Install: " + _installOrder);
             
        }

        private static void SeperatePackageVsDependency(List<string> list)
        {
            List<string> packages = new List<string>();
            List<string> dependencies = new List<string>();

            foreach (string item in list)
            {                
                string[] combo = item.Split(new[] { ":" }, StringSplitOptions.None);

                if (combo[1].Trim() == "")
                {
                    BuildFinalInstallResult(combo[0].Trim());
                    list.Remove(item);                     
                }
                else {
                    //TODO: loop though each one and decide if they can be added
                    // installed list.
                    packages.Add(combo[0].Trim());
                    dependencies.Add(combo[1].Trim());
                }
            }
        }

        private static void BuildFinalInstallResult(string item)
        {
            if (_installOrder == null)
            {
                _installOrder = new StringBuilder();
                _installedItems = new List<string>();

                _installOrder.Append(item);
                _installedItems.Add(item);
            }
            else
            {
                _installOrder.AppendFormat(", {0}", item);
                _installedItems.Add(item);
            }
        }
    }
}

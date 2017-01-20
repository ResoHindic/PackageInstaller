using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static PackageInstaller.Helper;
using static PackageInstaller.InstallOrderResult;

namespace PackageInstaller
{
    public class Program
    {
        static void Main(string[] args)
        {
            string input;
            string inputText = "";
            List<string> listOfInputCombinations = null;

            Console.WriteLine("Please provide the Install List in following format:\n [ \"Package: Dependency\", \"Package2: Dependency2\", \"Package: \" ] \n");

            // If user proved text, store it and move on
            // Else, print an error back to the user.
            while ((input = Console.ReadLine()) == "")
            {
                Console.WriteLine("No Input Provided! ... Please try again, format EX: \n [ \"Package: Dependency\", \"Package2: Dependency2\", \"Package: \" ] \n");                
            }
            inputText = input;
            
            try
            {
                // run the JSon serilizer to comma separate the input
                // and store it into a List of strings.
                listOfInputCombinations = JsonConvert.DeserializeObject<List<string>>(inputText);
            }
            catch
            {
                Console.WriteLine("/n Unable to process the input, please make sure it follows the following format: \n [ \"Package: Dependency\", \"Package2: Dependency2\", \"Package: \" ] \n");
                Environment.Exit(1);
            }

            InstalOrderResults result = SeperatePackageVsDependency(listOfInputCombinations);
                        
            if (!result.isValid)
            {
                Console.WriteLine("/n Unable to determine Install Order, list privided has a loop. \n");
                Environment.Exit(1);
            }

            Console.WriteLine("\n Order of Install: " + result.installOrder);             
        }        
    }
}

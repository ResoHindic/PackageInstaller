using System;
using PackageInstaller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PackageInstallerTests
{
    [TestClass]
    public class InstallerTests
    {
        [TestMethod]
        public void Valid_InputString()
        {
            #region This test will process a Valid input string and validate if return string is as expected.
            
            string input = "[ \"Yes: No\", \"No: Maybe\", \"Maybe: \" ]";
            List<string> inputList = DeserializeInput(input);

            var result = Helper.SeperatePackageVsDependency(inputList);

            Assert.AreEqual("Maybe, No, Yes", result.installOrder);
            Console.WriteLine("Text should be: \"Maybe, No, Yes\", \n Text generated: " + result.installOrder);
            Assert.AreEqual(true, result.isValid);

            #endregion
        }

        [TestMethod]
        public void Valid_isValidCheck()
        {
            string input = "[ \"Yes: No\", \"No: Maybe\", \"Maybe: \" ]";
            List<string> inputList = DeserializeInput(input);

            var result = Helper.SeperatePackageVsDependency(inputList);

            Assert.AreEqual(true, result.isValid);
            Console.WriteLine("Response should be: \"True\", \n Response received: " + result.isValid);
        }

        [TestMethod]
        public void Valid_InputStringWithWhitespace()
        {
            #region This test will precess correctly if input contained unnecessary whiteSpace.

            string input = "[ \"  Yes:   No  \", \" No  : Maybe     \", \" Maybe  :   \"]";
            List<string> inputList = DeserializeInput(input);

            var result = Helper.SeperatePackageVsDependency(inputList);

            Assert.AreEqual("Maybe, No, Yes", result.installOrder);
            Console.WriteLine("Text should be: \"Maybe, No, Yes\", \n Text generated: " + result.installOrder);
            Assert.AreEqual(true, result.isValid);

            #endregion
        }

        [TestMethod]
        public void Valid_EXFromPDF()
        {
            #region This tests validats that example string from provided PDF returns correct result.
            
            string input = "[ \"KittenService: \",\"Leetmeme: Cyberportal\",\"Cyberportal: Ice\",\"CamelCaser: KittenService\",\"Fraudstream: Leetmeme\",\"Ice: \"]";
            List<string> inputList = DeserializeInput(input);

            var result = Helper.SeperatePackageVsDependency(inputList);

            Assert.AreEqual("KittenService, CamelCaser, Ice, Cyberportal, Leetmeme, Fraudstream", result.installOrder);
            Console.WriteLine("Text should be: \"KittenService, CamelCaser, Ice, Cyberportal, Leetmeme, Fraudstream\", \n Text generated: " + result.installOrder);
            Assert.AreEqual(true, result.isValid);

            #endregion
        }

        [TestMethod]
        public void InValid_EXFromPDF()
        {
            #region This tests validats that example string from provided PDF returns invalid result.

            string input = "[\"KittenService: \",\"Leetmeme: Cyberportal\",\"Cyberportal: Ice\",\"CamelCaser: KittenService\",\"Fraudstream: \",\"Ice: Leetmeme\"]";
            List<string> inputList = DeserializeInput(input);

            var result = Helper.SeperatePackageVsDependency(inputList);

            Assert.AreEqual("KittenService, CamelCaser, Fraudstream", result.installOrder);
            Console.WriteLine("Text should be: \"KittenService, CamelCaser, Fraudstream\", \n Text generated: " + result.installOrder);
            Assert.AreEqual(false, result.isValid);

            #endregion
        }

        [TestMethod]
        public void Looping_InputString()
        {
            #region This test will validate that appropirate message is returned when input contains a dependency loop.

            string input = "[ \"Yes: No\", \"No: Maybe\", \"Maybe: Yes\", \"Definitely: \" ]";
            List<string> inputList = DeserializeInput(input);

            var result = Helper.SeperatePackageVsDependency(inputList);

            Assert.AreEqual("Definitely", result.installOrder);
            Console.WriteLine("Text should be: \"Definitely\", \n Text generated: " + result.installOrder);
            Assert.AreEqual(false, result.isValid);

            #endregion
        }
        
        private List<string> DeserializeInput(string input)
        {

            List<string> inputList = JsonConvert.DeserializeObject<List<string>>(input);

            return inputList;
        }        
    }
}

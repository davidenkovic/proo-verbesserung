using Core.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CreditCardNumberValidations
    {
        /// <summary>
        ///A test for IsCreditCardValid
        ///</summary>
        [TestMethod()]
        public void T01_IsCreditCard_OK()
        {
            string creditCardNumber = "2718-2818-2845-8567";
            bool expected = true;
            var actual = CreditCardNumberValidation.IsCreditCardValid(creditCardNumber);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void T02_IsCreditCard_Checksum_NotOK()
        {
            string creditCardNumber = "2718-2818-2845-8566";
            bool expected = false;
            var actual = CreditCardNumberValidation.IsCreditCardValid(creditCardNumber);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void T03_IsCreditCard_Letter()
        {
            string creditCardNumber = "2718281828X58566";
            bool expected = false;
            var actual = CreditCardNumberValidation.IsCreditCardValid(creditCardNumber);
            Assert.AreEqual(expected, actual, "Enthält Buchstaben");
        }

        [TestMethod()]
        public void T04_IsCreditCard_OK_Zero()
        {
            string creditCardNumber = "2418281828458560";
            bool expected = true;
            var actual = CreditCardNumberValidation.IsCreditCardValid(creditCardNumber);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void T05_IsCreditCard_OK_DifferentLengths()
        {

            Assert.IsTrue(CreditCardNumberValidation.IsCreditCardValid("4773346602970"));
            Assert.IsTrue(CreditCardNumberValidation.IsCreditCardValid("6771-8956-2609-8195"));
            Assert.IsTrue(CreditCardNumberValidation.IsCreditCardValid("3757-236420-67619"));
            Assert.IsTrue(CreditCardNumberValidation.IsCreditCardValid("6767-5468-4342-4033-753"));
            Assert.IsTrue(CreditCardNumberValidation.IsCreditCardValid("6759-5288-7611-6755-85"));

        }
        
    }
}
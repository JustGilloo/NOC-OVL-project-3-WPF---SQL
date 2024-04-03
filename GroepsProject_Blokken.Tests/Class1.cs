using Groepsproject_Blokken;
using NUnit.Framework;

namespace GroepsProject_Blokken.Tests
{


    [TestFixture]
    public class PrimeWordTests
    {
        [Test]
        public void PrimeWordTestEen()
        {
            //Arrange
            PrimeWord eenPrimeword = new PrimeWord();
            eenPrimeword.Primeword = "Banaan";
            eenPrimeword.Hint = "Fruit";
            //Act
            bool iets = eenPrimeword.CheckAnswerIfPrimeWord("Banaan");

            //Assert
            Assert.That(iets, Is.True);
        }

        [Test]
        public void PrimeWordTestTwee()
        {
            //Arrange
            PrimeWord eenPrimeword = new PrimeWord();
            eenPrimeword.Primeword = "Brocolli";
            eenPrimeword.Hint = "Groente";
            //Act
            PrimeWord.PrimeWordCuttingAndShowing(eenPrimeword);

            //Assert

        }
    }
}

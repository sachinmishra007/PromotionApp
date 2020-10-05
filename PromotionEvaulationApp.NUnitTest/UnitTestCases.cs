using NUnit.Framework;

namespace PromotionEvaulationApp.NUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Scenario1()
        {
            PromotionEvaulationApp._mainProgram._mainProgram _main = new PromotionEvaulationApp._mainProgram._mainProgram();
            // Setting up the Order
            _main._lstOrderItems = new System.Collections.Generic.List<char>() { 'A', 'B', 'C' };

            _main._getUnitSKUPrice(true);
            _main._getActivePromotion(true);
            _main._getBasicSetup();


            _main._getComboOrderInfo();
            _main._getSummaryOrderInfo();

            Assert.IsTrue(_main._totalAmount == 100);
        }


        [Test(Author = "Sachin Mishra", Description = " Testing the Order Scenario 1")]
        public void Scenario2()
        {
            PromotionEvaulationApp._mainProgram._mainProgram _main = new PromotionEvaulationApp._mainProgram._mainProgram();
            // Setting up the Order
            _main._lstOrderItems = new System.Collections.Generic.List<char>()
            {
                'A', 'A', 'A', 'A', 'A',
                'B', 'B', 'B', 'B', 'B',
                'C'
            };

            _main._getUnitSKUPrice(true);
            _main._getActivePromotion(true);
            _main._getBasicSetup();


            _main._getComboOrderInfo();
            _main._getSummaryOrderInfo();

            Assert.IsTrue(_main._totalAmount == 370);
        }

        [Test]
        public void Scenario3()
        {
            PromotionEvaulationApp._mainProgram._mainProgram _main = new PromotionEvaulationApp._mainProgram._mainProgram();
            // Setting up the Order
            _main._lstOrderItems = new System.Collections.Generic.List<char>()
            {
                'A','A','A',
                'B','B','B','B','B',
                'C','D'
            };

            _main._getUnitSKUPrice(true);
            _main._getActivePromotion(true);
            _main._getBasicSetup();


            _main._getComboOrderInfo();
            _main._getSummaryOrderInfo();

            Assert.IsTrue(_main._totalAmount == 280);
        }
    }
}
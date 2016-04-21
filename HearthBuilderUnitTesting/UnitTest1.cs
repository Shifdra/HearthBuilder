using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HearthBuilder;
using HearthBuilder.Models.Account;
using HearthBuilder.Models.Decks;


namespace HearthBuilderUnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AccountTests()
        {
            //Logging in
            User user = new UserLogin();
            user.Email = "test@test.com";
            user.Password = "test";
            user = UserDAO.Instance.GetAccountByEmailAndPassword(user);
            Assert.AreEqual("Test", user.FirstName);
            Assert.AreEqual("McTester", user.LastName);
            Assert.AreEqual(8, user.ID);

            //Registration
            /*User user2 = new UserRegister();
            user2.Email = "UnitTest1";
            user2.Password = "UnitTest2";
            user2.FirstName = "TestFirstName";
            user2.LastName = "TestLastName";
            user2 = UserDAO.Instance.RegisterUser(user2);*/

        }

        [TestMethod]
        public void DeckTests()
        {
            //Creating a deck, based on a class
            Deck deck = new Deck(HearthBuilder.Models.PlayerClasses.DRUID);


        }

        [TestMethod]
        public void FilterTests()
        {

        }
    }
}

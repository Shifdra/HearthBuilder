using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HearthBuilder;
using HearthBuilder.Models;
using HearthBuilder.Models.Account;
using HearthBuilder.Models.Cards;
using HearthBuilder.Models.Decks;
using HearthBuilder.Models.FilterDecks;
using System.Collections.Generic;
using HearthBuilder.Controllers;
using System.Web.Mvc;

namespace HearthBuilderUnitTesting
{
    /*
     * For our Unit Testing, we'll run the business logic though its paces.
     * This requires making two accounts, completing normal functions, then trying to access eachothers stuff.
     * We'll be performing the tests on both users.
     */

    [TestClass]
    public class UnitTest1
    {
        User user1;
        User user2;

        [TestMethod]
        public void AccountGet()
        {
            user1 = UserDAO.Instance.GetUserbyEmail("unittest1@test.com");
            Assert.AreEqual("Robot1", user1.FirstName);
            user2 = UserDAO.Instance.GetUserbyEmail("unittest2@test.com");
            Assert.AreEqual("Robot2", user2.FirstName);
        }

        [TestMethod]
        public void AccountDelete()
        {
            user1 = UserDAO.Instance.GetUserbyEmail("unittest1@test.com");
            user1 = UserDAO.Instance.DeleteUser(user1);
            Assert.AreEqual(null, user1);

            user2 = UserDAO.Instance.DeleteUser(new User { ID = 0 } ); //try to delete a user that doesnt exist!
            Assert.AreEqual(null, user2);
        }

        [TestMethod]
        [ExpectedException(typeof(HearthBuilder.Models.Account.UserException), "An invalid user was innapropriately registered!")]
        public void AccountRegister()
        {
            user1 = new User();
            user1.FirstName = "Robot1";
            user1.LastName = "Robert";
            user1.Email = "unittest1@test.com";
            user1.Password = "testing";
            user1 = UserDAO.Instance.RegisterUser(user1);
            Assert.IsTrue(user1.ID > 0, "user1 has an id of <= 0 after registering!?");

            user2 = new User();
            user2.FirstName = "Robot2";
            user2.LastName = "Robert";
            user2.Email = "unittest2@test.com";
            user2.Password = "testing";
            user2 = UserDAO.Instance.RegisterUser(user2);
            Assert.IsTrue(user2.ID > 0, "user2 has an id of <= 0 after registering!?");
            //Creating a deck, based on a class
            Deck deck = new Deck(HearthBuilder.Models.PlayerClasses.DRUID);


        }

        [TestMethod]
        public void AccountLogin()
        {
            //Creating a deck, based on a class
            Deck deck = new Deck(HearthBuilder.Models.PlayerClasses.DRUID);


        }

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

            //Assert.AreEqual(1, 2); //force a fail

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
            Deck deck = new Deck(PlayerClasses.DRUID);
        }

        [TestMethod]
        public void FilterByClass()
        {
            //Filter by class
            SearchParams searchParams = new SearchParams();
            foreach (ClassNames className in searchParams.Classes)
            {
                if (className.PlayerClass == PlayerClasses.DRUID.ToString())
                    className.Checked = true;
            }
            List<Deck> filteredDeckList = DeckDAO.Instance.GetDecksByClassAndDeckName(searchParams);
            System.Diagnostics.Debug.WriteLine(filteredDeckList.Count);

            foreach (Deck filteredDeck in filteredDeckList)
            {
                Assert.AreEqual(filteredDeck.Class, PlayerClasses.DRUID);
            }
        }

        [TestMethod]
        public void FilterByDeckName()
        {
            //Filter by deck name
            SearchParams searchParams = new SearchParams();
            searchParams.DeckName = "Dat Ramp";
            List<Deck> filteredDeckList = DeckDAO.Instance.GetDecksByClassAndDeckName(searchParams);
            System.Diagnostics.Debug.WriteLine(filteredDeckList.Count);

            foreach (Deck filteredDeck in filteredDeckList)
            {
                Assert.AreEqual(filteredDeck.Title, searchParams.DeckName);
            }
        }

        [TestMethod]
        public void FilterTests()
        {
            SearchParams searchParams = new SearchParams();
        }

        /*
         * 
         */ 

        [TestMethod]
        public void TestAccountView()
        {
            var loginController = new AccountController();
            UserLogin user = new UserLogin { Email = "test@test.com", Password = "test" };

            var result = (RedirectToRouteResult)loginController.Index(user);
            result.RouteValues["action"].Equals("Index");

            Assert.AreEqual("", result.ViewName);
        }
    }
}

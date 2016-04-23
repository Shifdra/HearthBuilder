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
        public void CreateDeck() //Creating a deck
        {
            Deck newDeck = new Deck(PlayerClasses.PALADIN);
            newDeck.UserId = 8; //test user
            newDeck.Title = "testDeck";
            DeckDAO.Instance.UpdateDeck(newDeck); //create deck in db

            List<Deck> deckList = DeckDAO.Instance.GetDecksByUser(8); //get all of test user's decks
            System.Diagnostics.Debug.WriteLine(deckList.Count);
            Boolean successFlag = false;
            foreach (Deck deck in deckList)
            {
                //if no paladin deck exists with the title 'testDeck', the test is a fail
                if (deck.Class == newDeck.Class && deck.Title == newDeck.Title)
                {
                    successFlag = true;
                }
            }
            if (!successFlag)
                Assert.Fail();
        }

        [TestMethod]
        public void DeleteDeck() //Deleting a deck
        {
            //all decks made by user 'test'
            List<Deck> deckList = DeckDAO.Instance.GetDecksByUser(8);

            Boolean successFlag = false;
            foreach (Deck deck in deckList)
            {
                if (deck.Class == PlayerClasses.PALADIN && deck.Title == "testDeck")
                {
                    DeckDAO.Instance.DeleteDeck(deck.Id);
                    successFlag = true;
                }
            }
            if (!successFlag)
                Assert.Fail();
        }

        [TestMethod]
        public void DeckTests()
        {
            Deck deck = new Deck(PlayerClasses.DRUID);
        }

        [TestMethod]
        public void FilterByOneClass() //Filter by one class only
        {
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
                //all returned results should be the Druid class
                Assert.AreEqual(filteredDeck.Class, PlayerClasses.DRUID);
            }
        }

        [TestMethod]
        public void FilterByMultipleClasses() //Filter by multiple classes
        {
            SearchParams searchParams = new SearchParams();
            foreach (ClassNames className in searchParams.Classes)
            {
                if (className.PlayerClass == PlayerClasses.DRUID.ToString())
                    className.Checked = true;
                if (className.PlayerClass == PlayerClasses.HUNTER.ToString())
                    className.Checked = true;
            }

            List<Deck> filteredDeckList = DeckDAO.Instance.GetDecksByClassAndDeckName(searchParams);
            System.Diagnostics.Debug.WriteLine(filteredDeckList.Count);

            foreach (Deck filteredDeck in filteredDeckList)
            {
                System.Diagnostics.Debug.WriteLine(filteredDeck.Class.ToString());
                //all returned results should be Druid or Hunter
                if (filteredDeck.Class != PlayerClasses.DRUID && filteredDeck.Class != PlayerClasses.HUNTER)
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void FilterByDeckName() //Filter by deck name only
        {
            SearchParams searchParams = new SearchParams();
            searchParams.DeckName = "Dat Ramp";

            List<Deck> filteredDeckList = DeckDAO.Instance.GetDecksByClassAndDeckName(searchParams);
            System.Diagnostics.Debug.WriteLine(filteredDeckList.Count);

            foreach (Deck filteredDeck in filteredDeckList)
            {
                //all returned results should have the deck name 'Dat Ramp'
                Assert.AreEqual(filteredDeck.Title, searchParams.DeckName);
            }
        }

        [TestMethod]
        public void FilterByPartialDeckName() //Filter with a partial deck name
        {
            SearchParams searchParams = new SearchParams();
            searchParams.DeckName = "st";

            List<Deck> filteredDeckList = DeckDAO.Instance.GetDecksByClassAndDeckName(searchParams);
            System.Diagnostics.Debug.WriteLine(filteredDeckList.Count);

            foreach (Deck filteredDeck in filteredDeckList)
            {
                System.Diagnostics.Debug.WriteLine(filteredDeck.Title);
                //all returned results should have a deck name that contains the string 'st'
                Assert.IsTrue(filteredDeck.Title.Contains("st") || filteredDeck.Title.Contains("ST"));
            }
        }

        [TestMethod]
        public void FilterByClassAndDeckName() //Filter by class and deck name
        {
            SearchParams searchParams = new SearchParams();
            searchParams.DeckName = "Dat Ramp";
            foreach (ClassNames className in searchParams.Classes)
            {
                if (className.PlayerClass == PlayerClasses.DRUID.ToString())
                    className.Checked = true;
            }

            List<Deck> filteredDeckList = DeckDAO.Instance.GetDecksByClassAndDeckName(searchParams);
            System.Diagnostics.Debug.WriteLine(filteredDeckList.Count);

            foreach (Deck filteredDeck in filteredDeckList)
            {
                //all returned results should be the Druid class and have the deck name 'Dat Ramp'
                Assert.AreEqual(filteredDeck.Title, searchParams.DeckName);
            }
        }

        [TestMethod]
        public void FilterTests()
        {
            SearchParams searchParams = new SearchParams();
        }
    }
}

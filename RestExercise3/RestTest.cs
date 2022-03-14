using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestExercise2.Managers;
using RestExercise2.Models;

namespace RestExercise3
{
    [TestClass]
    public class RestTest
    {
        //Having an object variable manager,so each method doesnt have to initialze one
        public ItemsManager _manager = new ItemsManager();
        [TestMethod]
        public void RestGetAll()
        {
            //test without paramters
             Assert.IsTrue(_manager.GetAll().Count == 3);
            //testing the substring
            Assert.IsTrue(_manager.GetAll("c#").Count == 2);
            //testing the substring is case-insenstive
            Assert.IsTrue(_manager.GetAll("C#").Count == 2);
            //Testing with a filter that doesnt return anything,expecting an empty list,not null
            Assert.IsTrue(_manager.GetAll("something").Count == 0);


            //Tesing the item quality filter
            Assert.IsTrue(_manager.GetAll(null, 25).Count == 2);
            Assert.IsTrue(_manager.GetAll(null, 0).Count==3);
            Assert.IsTrue(_manager.GetAll(null, 5000).Count == 0);

            //testing the quatity filter
            Assert.IsTrue(_manager.GetAll(null, 0, 2).Count == 2);
            Assert.IsTrue(_manager.GetAll(null, 0, 0).Count == 3);
            Assert.IsTrue(_manager.GetAll(null, 0, 11).Count == 0);

            //Testing the combination
            Assert.IsTrue(_manager.GetAll("c#", 0, 0).Count == 2);
            Assert.IsTrue(_manager.GetAll("C#", 100, 5).Count == 1);
            Assert.IsTrue(_manager.GetAll("fruit", 1, 6).Count == 0);


        }
        [TestMethod]
        public void TestGetAllBetweenQuality()
        {
            //expecting the single result (fruit basket)
            Assert.IsTrue(_manager.GetAllBetweenQuality(40, 60).Count == 1);
            //chechking the signle results name should be fruite basket
            Assert.IsTrue(_manager.GetAllBetweenQuality(20, 30).Count == 0);
            //testing a ranage that shouldnt have any item
            Assert.IsTrue(_manager.GetAllBetweenQuality(20, 30).Count == 0);

        }
        [TestMethod]
        public void testByid()
        {
            //checking the first item

            Assert.IsTrue(_manager.GetById(1).ItemQuality.Equals (300));
            Assert.IsTrue(_manager.GetById(1).Name.Equals("Book about C#"));
            Assert.IsNull(_manager.GetById(25));
        }
        [TestMethod]
        public void AddAndDelete()
        {
            // Reads the count before adding so we can compare it the number after adding
            int beforeAddCount = _manager.GetAll().Count;
            //creates a variable with the id we assign,should be overriden when the manager add the item
            int defaultId = 0;
            // create a test ite to be added
            Item newitem = new Item(defaultId,"testitem", 3, 3);
            //adding the item and storing the result in a variable
            Item addResult = _manager.Add(newitem);
            //stores the newely assigned Id
            int newID = addResult.Id;
            //CHECKING THE MANAGER ASSIGNS A NEW ID
            Assert.AreNotEqual(defaultId, newID);
            // checking now the count of the lis is 1 more than before
            Assert.AreEqual(beforeAddCount + 1, _manager.GetAll().Count);

            // checking that the id of the deleted item is the same that we asked to be deleted
            Item delteitem = _manager.Delete(newID);
            //checks that the count is the same as when we began before adding and deleting
            Assert.AreEqual(beforeAddCount,_manager.GetAll().Count);
            //checks that if we ask to delete an item with an id that doesnt exist,that it returns null
            Assert.IsNull(_manager.Delete(35));
          }
        [TestMethod]
        public void testUpdate()
        {
            //create an item with holds data to update another item
            Item newitem = new Item(14, "testItem", 3, 4);
            //updates the item
            _manager.Update(1, newitem);

            //checks that the item in the manager has the name from the newItem
            Assert.AreEqual(newitem.Name, _manager.GetById(1).Name);
            //checks that we receive null when trying to update something not existing in the manager
            Assert.IsNull(_manager.Update(4, newitem));

            //cleans up

            Item cleansUpItem=new Item()
            { Name="Book about c#",ItemQuality=300,Quantity=10};
            _manager.Update(1, cleansUpItem);


        }

    }
}

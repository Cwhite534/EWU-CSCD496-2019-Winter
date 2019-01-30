using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class GiftControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftController_RequiresGiftService()
        {
            new GiftController(null);
        }

        [TestMethod]
        public void GetGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftForUser(-1);

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastModifiedUserId);
        }

        [TestMethod]
        public void GetGiftsForUser_ReturnsUsersFromService()
        {
            var gift = CreateGiftWithId(1);
            var testService = new TestableGiftService {GiftsToReturn = new List<Gift> {gift}};
            var controller = new GiftController(testService);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftForUser(4);

            Assert.AreEqual(4, testService.LastModifiedUserId);
            DTO.Gift resultGift = result.Value.Single();
            Assert.AreEqual(gift.Id, resultGift.Id);
            Assert.AreEqual(gift.Title, resultGift.Title);
            Assert.AreEqual(gift.Description, resultGift.Description);
            Assert.AreEqual(gift.Url, resultGift.Url);
            Assert.AreEqual(gift.OrderOfImportance, resultGift.OrderOfImportance);
        }
        
        [TestMethod]
        public void AddGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);

            var gift = new DTO.Gift(CreateGiftWithId(1));
            ActionResult<DTO.Gift> result = controller.AddGiftForUser(-1, gift);

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastModifiedUserId);
            Assert.IsNull(testService.LastModifiedGift);

        }

        [TestMethod]
        public void AddGiftForUser_RequiresGift()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            ActionResult<DTO.Gift> result = controller.AddGiftForUser(1, null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastModifiedUserId);
            Assert.IsNull(testService.LastModifiedGift);
        }

        [TestMethod]
        public void AddGiftForUser_ReturnAddition()
        {
            var testService = new TestableGiftService {GiftsToReturn = new List<Gift>()};
            var controller = new GiftController(testService);
            var giftBeforeAdd = CreateGiftWithId(1);
            var giftAfterAdd = controller.AddGiftForUser(5, new DTO.Gift(giftBeforeAdd)).Value;
            Assert.AreEqual(5, testService.LastModifiedUserId);
            Assert.AreEqual(giftBeforeAdd.Id, giftAfterAdd.Id);
            Assert.AreEqual(giftBeforeAdd.Title, giftAfterAdd.Title);
            Assert.AreEqual(giftBeforeAdd.Description, giftAfterAdd.Description);
            Assert.AreEqual(giftBeforeAdd.Url, giftAfterAdd.Url);
            Assert.AreEqual(5, testService.LastModifiedUserId);
        }
        
        [TestMethod]
        public void UpdateGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);

            var gift = new DTO.Gift(CreateGiftWithId(1));
            ActionResult<DTO.Gift> result = controller.UpdateGiftForUser(-1, gift);

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastModifiedUserId);
            Assert.IsNull(testService.LastModifiedGift);

        }

        [TestMethod]
        public void UpdateGiftForUser_RequiresGift()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            ActionResult<DTO.Gift> result = controller.UpdateGiftForUser(1, null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastModifiedUserId);
            Assert.IsNull(testService.LastModifiedGift);
        }

        [TestMethod]
        public void UpdateGiftForUser_ReturnUpdated()
        {
            var testService = new TestableGiftService {GiftsToReturn = new List<Gift>()};
            var controller = new GiftController(testService);
            var giftBeforeUpdate = CreateGiftWithId(1);
            var giftAfterUpdate = controller.UpdateGiftForUser(5, new DTO.Gift(giftBeforeUpdate)).Value;
            Assert.AreEqual(5, testService.LastModifiedUserId);
            Assert.AreEqual(giftBeforeUpdate.Id, giftAfterUpdate.Id);
            Assert.AreEqual(giftBeforeUpdate.Title, giftAfterUpdate.Title);
            Assert.AreEqual(giftBeforeUpdate.Description, giftAfterUpdate.Description);
            Assert.AreEqual(giftBeforeUpdate.Url, giftAfterUpdate.Url);
            Assert.AreEqual(5, testService.LastModifiedUserId);
        }
        

        [TestMethod]
        public void RemoveGiftForUser_RequiresGift()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            ActionResult<DTO.Gift> result = controller.RemoveGift(null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.LastModifiedGift);
        }

        [TestMethod]
        public void RemoveGiftForUser_GiftIsRemoved()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            var giftToRemove = CreateGiftWithId(5);
            controller.RemoveGift(new DTO.Gift(giftToRemove));
            var lastGiftRemoved = testService.LastModifiedGift;
            Assert.AreEqual(giftToRemove.Id, lastGiftRemoved.Id);
            Assert.AreEqual(giftToRemove.Title, lastGiftRemoved.Title);
            Assert.AreEqual(giftToRemove.Description, lastGiftRemoved.Description);
            Assert.AreEqual(giftToRemove.Url, lastGiftRemoved.Url);
        }

        private Gift CreateGiftWithId(int id)
        {
            return new Gift
            {
                Id = id,
                Title = $"Gift Title {id}",
                Description = $"Gift Description {id}",
                Url = $"http://www.gift{id}.url",
                OrderOfImportance = id
            };
        }
    }
}
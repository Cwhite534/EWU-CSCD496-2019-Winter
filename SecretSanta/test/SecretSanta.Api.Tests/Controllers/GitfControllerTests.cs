﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests
    {
        [TestMethod]
        public void GetGiftForUser_ReturnsUsersFromService()
        {
            var gift = new Gift
            {
                Id = 3,
                Title = "Gift Tile",
                Description = "Gift Description",
                Url = "http://www.gift.url",
                OrderOfImportance = 1
            };
            var testService = new TestableGiftService
            {
                ToReturn = new List<Gift>
                {
                    gift
                }
            };
            var mapper = Mapper.Instance;

            var controller = new GiftController(testService, mapper);
            

            IActionResult result = controller.GetGiftForUser(4);
            Assert.IsTrue(result is OkObjectResult);
            var okObjectResult = (OkObjectResult) result;
            var resultList = (List<GiftViewModel>) okObjectResult.Value;
            Assert.AreEqual(4, testService.GetGiftsForUser_UserId);
            
            GiftViewModel resultGift = resultList.Single();
            Assert.AreEqual(gift.Id, resultGift.Id);
            Assert.AreEqual(gift.Title, resultGift.Title);
            Assert.AreEqual(gift.Description, resultGift.Description);
            Assert.AreEqual(gift.Url, resultGift.Url);
            Assert.AreEqual(gift.OrderOfImportance, resultGift.OrderOfImportance);
        }

        [TestMethod]
        public void GetGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var mapper = Mapper.Instance;

            var controller = new GiftController(testService, mapper);

            IActionResult result = controller.GetGiftForUser(-1);

            Assert.IsTrue(result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.GetGiftsForUser_UserId);
        }
    }
}

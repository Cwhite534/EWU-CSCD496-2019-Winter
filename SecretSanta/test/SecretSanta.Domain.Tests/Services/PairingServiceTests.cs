using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class PairingServiceTests : DatabaseServiceTests
    {

        private Group _sut { get; set; }

        [TestInitialize]
        public async Task SetupUsersAndGroupForTests()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                GroupService groupService = new GroupService(context);
                UserService userService = new UserService(context);

                var user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };
                var user2 = new User
                {
                    FirstName = "Princess",
                    LastName = "Buttercup"
                };
                var user3 = new User
                {
                    FirstName = "Don",
                    LastName = "Quixote",
                };

                user = await userService.AddUser(user);
                user2 = await userService.AddUser(user2);
                user3 = await userService.AddUser(user3);

                var group = new Group
                {
                    Name = "Test Group"
                };

                group = await groupService.AddGroup(group);

                await groupService.AddUserToGroup(group.Id, user.Id);
                await groupService.AddUserToGroup(group.Id, user2.Id);
                await groupService.AddUserToGroup(group.Id, user3.Id);
            }
        }


        [TestMethod]
        public async Task GeneratePairings_EachPairHasUniqueRecipient()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                List<Pairing> userPairings = await pairingService.GenerateUserPairings(1);
                var sortedPairings = userPairings.OrderBy(pair => pair.RecipientId).ToList();
                for (var i = 0; i < sortedPairings.Count; i++)
                {
                    var pair = sortedPairings[i];
                    Assert.AreEqual(i + 1, pair.RecipientId);
                }
            }
        }

        
        [TestMethod]
        public async Task GeneratePairings_EachPairHasUniqueSanta()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                List<Pairing> userPairings = await pairingService.GenerateUserPairings(1);
                var sortedPairings = userPairings.OrderBy(pair => pair.SantaId).ToList();
                for (var i = 0; i < sortedPairings.Count; i++)
                {
                    var pair = sortedPairings[i];
                    Assert.AreEqual(i + 1, pair.SantaId);
                }
            }
        }
    }
}
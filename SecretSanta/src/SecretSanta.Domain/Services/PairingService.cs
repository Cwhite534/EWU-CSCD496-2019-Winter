﻿using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services
{
    public class PairingService : IPairingService
    {
        private ApplicationDbContext DbContext { get; }
        private IRandom Rand { get; }

        public PairingService(ApplicationDbContext dbContext, IRandom rand = null)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Rand = rand ?? new ThreadSafeRandom();
        }

        public async Task<List<Pairing>> GenerateUserPairings(int groupId)
        {
            //remove existing pairs for the group
            var existingPairs = DbContext.Pairings.Where(p => p.GroupId == groupId).ToList();
            existingPairs.ForEach(pair => DbContext.Pairings.Remove(pair));
            
            var userIds = await DbContext.Groups
                .Where(g => g.Id == groupId)
                .SelectMany(g => g.GroupUsers, (g, gu) => gu.UserId)
                .ToListAsync();

            List<Pairing> pairings = await Task.Run(() => GenerateUserPairings(userIds, groupId));

            await DbContext.Pairings.AddRangeAsync(pairings);
            await DbContext.SaveChangesAsync();
            return pairings;
        }

        private List<Pairing> GenerateUserPairings(List<int> userIds, int groupId)
        {
            List<Pairing> generatedPairings = new List<Pairing>();
            var randUserIds = userIds.Randomize(Rand).ToList();

            for (var i = 0; i < randUserIds.Count - 1; i++)
            {
                generatedPairings.Add(new Pairing {RecipientId = randUserIds[i], SantaId = randUserIds[i + 1], GroupId = groupId});
            }

            generatedPairings.Add(new Pairing {RecipientId = randUserIds.Last(), SantaId = randUserIds.First(), GroupId = groupId});

            return generatedPairings;
        }
    }
}
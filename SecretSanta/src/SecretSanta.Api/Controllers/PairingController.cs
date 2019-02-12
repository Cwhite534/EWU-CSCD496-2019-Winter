using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PairingController : ControllerBase
    {
        private IPairingService PairingService { get; }
        private IMapper Mapper { get; }

        public PairingController(IPairingService pairingService, IMapper mapper)
        {
            PairingService = pairingService;
            Mapper = mapper;
        }

        // POST api/pairing/5
        [HttpPost("{groupId}")]
        [Produces(typeof(ICollection<PairingViewModel>))]
        public async Task<IActionResult> GenerateUserPairings(int groupId)
        {
            if (groupId <= 0)
            {
                return BadRequest("A group id must be specified");
            }
            
            var pairings = await PairingService.GenerateUserPairings(groupId);
            return CreatedAtAction(nameof(GenerateUserPairings), new {groupId = groupId}, pairings.Select(p => Mapper.Map<PairingViewModel>(p)).ToList());
        }
    }
}
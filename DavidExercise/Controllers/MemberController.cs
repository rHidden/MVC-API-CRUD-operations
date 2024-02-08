using DavidExercise.Models;
using DavidExercise.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DavidExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMember(int Id)
        {
            var member = await _memberService.GetMember(Id);
            return Ok(member);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateMember(Member member)
        {
            var createdMember = await _memberService.CreateMember(member);
            return Ok(createdMember);
        }

        [HttpPost]
        [Route("promote")]
        public async Task<IActionResult> PromoteMember(int Id)
        {
            var newLeader = await _memberService.PromoteMember(Id);
            return Ok(newLeader);
        }

        [HttpPatch]
        public IActionResult UpdateMember(Member member)
        {
            _memberService.UpdateMember(member);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteMember(int id)
        {
            _memberService.DeleteMember(id);
            return Ok();
        }
    }
}

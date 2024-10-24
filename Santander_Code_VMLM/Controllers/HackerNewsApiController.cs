using Microsoft.AspNetCore.Mvc;
using Santander_Code_VMLM.Interfaces;
using Santander_Code_VMLM.Models;

namespace Santander_Code_VMLM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsApiController  : ControllerBase
    {

        private readonly IHackerNew _hackerNew;

        public HackerNewsApiController(IHackerNew hackerNew)
        {
            this._hackerNew = hackerNew;
        }


        [HttpGet]
        [Route("GetBestHistories")]
        public async Task<ActionResult<BestHistories>> GetBestHistories(int count, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _hackerNew.GetBestStoriesAsync(count, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
        }



    }
}

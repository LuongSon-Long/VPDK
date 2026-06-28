using HeThongQuanLyVanPhong.Controllers.Api;
using HeThongQuanLyVanPhong.Filters;
using HeThongQuanLyVanPhong.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Controllers.Api.Admin
{
    [RequireSession(AllowGuest = false)]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ModuleController : ApiControllerBase
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public ModuleController(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var modules = await _context.Modules
                .Select(m => new { m.Id, m.TenModule })
                .ToListAsync();
            return Ok(modules);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PercobaanAPI2.Models;
using System;
using System.Collections.Generic;

namespace PercobaanAPI2.Controllers
{
    public class SiswaController : Controller
    {
        private readonly IConfiguration _configuration;

        public SiswaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("api/siswa")]
        public ActionResult<IEnumerable<siswa>> ListSiswa()
        {
            using (var context = new PersonContext(_configuration))
            {
                return Ok(context.ListSiswa());
            }
        }

        [HttpGet("api/siswa/{id}")]
        public ActionResult<siswa> GetSiswa(int id)
        {
            using (var context = new PersonContext(_configuration))
            {
                var siswa = context.GetSiswaById(id);
                if (siswa == null)
                {
                    return NotFound();
                }
                return Ok(siswa);
            }
        }

        [HttpPost("api/siswa")]
        public ActionResult<siswa> AddSiswa([FromBody] siswa newSiswa)
        {
            if (ModelState.IsValid)
            {
                using (var context = new PersonContext(_configuration))
                {
                    var addedSiswa = context.AddSiswa(newSiswa);
                    return CreatedAtAction(nameof(GetSiswa), new { id = addedSiswa.id_siswa }, addedSiswa);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("api/siswa/{id}")]
        public IActionResult UpdateSiswa(int id, [FromBody] siswa updatedSiswa)
        {
            if (id != updatedSiswa.id_siswa)
            {
                return BadRequest("ID mismatch");
            }

            using (var context = new PersonContext(_configuration))
            {
                var existingSiswa = context.GetSiswaById(id);
                if (existingSiswa == null)
                {
                    return NotFound();
                }

                context.UpdateSiswa(updatedSiswa);
                return NoContent();
            }
        }

        [HttpDelete("api/siswa/{id}")]
        public IActionResult DeleteSiswa(int id)
        {
            using (var context = new PersonContext(_configuration))
            {
                var existingSiswa = context.GetSiswaById(id);
                if (existingSiswa == null)
                {
                    return NotFound();
                }

                context.DeleteSiswa(id);
                return NoContent();
            }
        }
    }
}

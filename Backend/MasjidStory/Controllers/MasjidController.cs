//using Microsoft.AspNetCore.Mvc;
//using ViewModels;
//using Services;
//using Models.Entities;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;
//namespace MasjidStory.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class MasjidController : ControllerBase
//    {
//        private readonly MasjidService _service;

//        public MasjidController(MasjidService service)
//        {
//            _service = service;
//        }

//        [HttpGet ("getAll")]
//        public async Task<ActionResult<List<MasjidViewModel>>> GetAll()
//        {
//            var masjids = await _service.GetAllMasjidsAsync();
//            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(masjids));
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<MasjidViewModel>> GetById(int id)
//        {
//            var masjid = await _service.GetMasjidByIdAsync(id);
//            if (masjid == null) return NotFound();
//            return Ok(ApiResponse<MasjidViewModel>.Ok(masjid));
//        }
//        [HttpGet("search")]
//        public async Task<ActionResult<List<MasjidViewModel>>> Search([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int size = 10)
//        {
//            var result = await _service.GetMasjidsPagedAsync(query, page, size);
//            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(result));
//        }

//        [Authorize(Roles ="Admin")]
//        [HttpPost("create")]
//        public async Task<IActionResult> Create([FromBody] MasjidCreateViewModel model)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            await _service.AddMasjidAsync(model);
//            return Ok("Masjid Created Sucessfully");
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] MasjidEditViewModel model)
//        {
//            if (id != model.Id) return BadRequest("ID mismatch.");
//            var result = await _service.UpdateMasjidAsync(model);
//            if (!result) return NotFound();
//            return Ok("Masjid Updated Sucessfully");
//        }
//        [Authorize(Roles ="Admin")]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _service.DeleteMasjidAsync(id);
//            if (!result) return NotFound();
//            return NoContent();
//        }
//        [HttpGet]
//        [Route("{id}/details")]
//        public async Task<ActionResult<MasjidDetailsViewModel>> GetMasjidDetails(int id, [FromQuery] string? lang = null)
//        {
//            var result = await _service.GetMasjidDetailsAsync(id, lang);
//            if (result == null) return NotFound();
//            return Ok(ApiResponse<MasjidDetailsViewModel>.Ok(result));
//        }
//        [HttpPost("{id}/visit")]
//        [Authorize]
//        public async Task<IActionResult> RegisterVisit(int id)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (userId == null) return Unauthorized();

//            var success = await _service.RegisterVisitAsync(id, userId);
//            if (!success) return NotFound(ApiResponse<string>.Fail("Masjid not found."));

//            return Ok(ApiResponse<string>.Ok("Visit recorded."));
//        }
//        [HttpGet("featured")]
//        public async Task<ActionResult<List<MasjidViewModel>>> GetFeaturedMasjids()
//        {
//            var masjids = await _service.GetFeaturedMasjidsAsync();
//            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(masjids));
//        }



//    }

//}
//using Microsoft.AspNetCore.Mvc;
//using ViewModels;
//using Services;
//using Models.Entities;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;

//namespace MasjidStory.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class MasjidController : ControllerBase
//    {
//        private readonly MasjidService _service;
//        private readonly MediaService _mediaService;

//        public MasjidController(MasjidService service, MediaService mediaService)
//        {
//            _service = service;
//            _mediaService = mediaService;
//        }

//        [HttpGet("getAll")]
//        public async Task<ActionResult<List<MasjidViewModel>>> GetAll()
//        {
//            var masjids = await _service.GetAllMasjidsAsync();
//            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(masjids));
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<MasjidViewModel>> GetById(int id)
//        {
//            var masjid = await _service.GetMasjidByIdAsync(id);
//            if (masjid == null) return NotFound();
//            return Ok(ApiResponse<MasjidViewModel>.Ok(masjid));
//        }

//        [HttpGet("search")]
//        public async Task<ActionResult<List<MasjidViewModel>>> Search([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int size = 10)
//        {
//            var result = await _service.GetMasjidsPagedAsync(query, page, size);
//            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(result));
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpPost("create")]
//        public async Task<IActionResult> Create([FromForm] MasjidCreateViewModel model)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            try
//            {
//                var masjidId = await _service.AddMasjidAsync(model);

//                // Handle media uploads if any
//                if (model.MediaFiles != null && model.MediaFiles.Any())
//                {
//                    foreach (var file in model.MediaFiles)
//                    {
//                        var mediaModel = new MediaCreateViewModel
//                        {
//                            MasjidId = masjidId,
//                            File = file
//                        };
//                        await _mediaService.UploadMediaAsync(mediaModel);
//                    }
//                }

//                return Ok(ApiResponse<string>.Ok("Masjid Created Successfully"));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ApiResponse<string>.Fail($"Failed to create masjid: {ex.Message}"));
//            }
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromForm] MasjidEditViewModel model)
//        {
//            if (id != model.Id) return BadRequest("ID mismatch.");

//            try
//            {
//                // Handle media deletions if any
//                if (model.MediaIdsToDelete != null && model.MediaIdsToDelete.Any())
//                {
//                    foreach (var mediaId in model.MediaIdsToDelete)
//                    {
//                        await _mediaService.DeleteMediaAsync(mediaId);
//                    }
//                }

//                // Handle new media uploads if any
//                if (model.NewMediaFiles != null && model.NewMediaFiles.Any())
//                {
//                    foreach (var file in model.NewMediaFiles)
//                    {
//                        var mediaModel = new MediaCreateViewModel
//                        {
//                            MasjidId = id,
//                            File = file
//                        };
//                        await _mediaService.UploadMediaAsync(mediaModel);
//                    }
//                }

//                var result = await _service.UpdateMasjidAsync(model);
//                if (!result) return NotFound();
//                return Ok(ApiResponse<string>.Ok("Masjid Updated Successfully"));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ApiResponse<string>.Ok($"Failed to update masjid: {ex.Message}"));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                var result = await _service.DeleteMasjidAsync(id);
//                if (!result) return NotFound();
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Failed to delete masjid: {ex.Message}");
//            }
//        }

//        [HttpGet]
//        [Route("{id}/details")]
//        public async Task<ActionResult<MasjidDetailsViewModel>> GetMasjidDetails(int id, [FromQuery] string? lang = null)
//        {
//            var result = await _service.GetMasjidDetailsAsync(id, lang);
//            if (result == null) return NotFound();
//            return Ok(ApiResponse<MasjidDetailsViewModel>.Ok(result));
//        }

//        [HttpPost("{id}/visit")]
//        [Authorize]
//        public async Task<IActionResult> RegisterVisit(int id)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (userId == null) return Unauthorized();

//            var success = await _service.RegisterVisitAsync(id, userId);
//            if (!success) return NotFound(ApiResponse<string>.Fail("Masjid not found."));

//            return Ok(ApiResponse<string>.Ok("Visit recorded."));
//        }

//        [HttpGet("featured")]
//        public async Task<ActionResult<List<MasjidViewModel>>> GetFeaturedMasjids()
//        {
//            var masjids = await _service.GetFeaturedMasjidsAsync();
//            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(masjids));
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using ViewModels;
using Services;
using Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasjidController : ControllerBase
    {
        private readonly MasjidService _service;
        private readonly MediaService _mediaService;

        public MasjidController(MasjidService service, MediaService mediaService)
        {
            _service = service;
            _mediaService = mediaService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<MasjidViewModel>>> GetAll()
        {
            var masjids = await _service.GetAllMasjidsAsync();
            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(masjids));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MasjidViewModel>> GetById(int id)
        {
            var masjid = await _service.GetMasjidByIdAsync(id);
            if (masjid == null) return NotFound();
            return Ok(ApiResponse<MasjidViewModel>.Ok(masjid));
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<MasjidViewModel>>> Search([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetMasjidsPagedAsync(query, page, size);
            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(result));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] MasjidCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var masjidId = await _service.AddMasjidAsync(model);

                // Handle media uploads if any
                if (model.MediaFiles != null && model.MediaFiles.Any())
                {
                    foreach (var file in model.MediaFiles)
                    {
                        var mediaModel = new MediaCreateViewModel
                        {
                            MasjidId = masjidId,
                            File = file
                        };
                        await _mediaService.UploadMediaAsync(mediaModel);
                    }
                }

                return Ok(ApiResponse<string>.Ok("Masjid Created Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Failed to create masjid: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MasjidEditViewModel model)
        {
            if (id != model.Id) return BadRequest("ID mismatch.");

            try
            {
                // Handle media deletions if any
                if (model.MediaIdsToDelete != null && model.MediaIdsToDelete.Any())
                {
                    foreach (var mediaId in model.MediaIdsToDelete)
                    {
                        await _mediaService.DeleteMediaAsync(mediaId);
                    }
                }

                // Handle new media uploads if any
                if (model.NewMediaFiles != null && model.NewMediaFiles.Any())
                {
                    foreach (var file in model.NewMediaFiles)
                    {
                        var mediaModel = new MediaCreateViewModel
                        {
                            MasjidId = id,
                            File = file
                        };
                        await _mediaService.UploadMediaAsync(mediaModel);
                    }
                }

                var result = await _service.UpdateMasjidAsync(model);
                if (!result) return NotFound();
                return Ok(ApiResponse<string>.Ok("Masjid Updated Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Failed to update masjid: {ex.Message}"));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteMasjidAsync(id);
                if (!result) return NotFound();
                return Ok(ApiResponse<string>.Ok("Masjid Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Failed to delete masjid: {ex.Message}"));
            }
        }

        [HttpGet]
        [Route("{id}/details")]
        public async Task<ActionResult<MasjidDetailsViewModel>> GetMasjidDetails(int id, [FromQuery] string? lang = null)
        {
            var result = await _service.GetMasjidDetailsAsync(id, lang);
            if (result == null) return NotFound();
            return Ok(ApiResponse<MasjidDetailsViewModel>.Ok(result));
        }

        [HttpPost("{id}/visit")]
        [Authorize]
        public async Task<IActionResult> RegisterVisit(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var success = await _service.RegisterVisitAsync(id, userId);
            if (!success) return NotFound(ApiResponse<string>.Fail("Masjid not found."));

            return Ok(ApiResponse<string>.Ok("Visit recorded."));
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<MasjidViewModel>>> GetFeaturedMasjids()
        {
            var masjids = await _service.GetFeaturedMasjidsAsync();
            return Ok(ApiResponse<List<MasjidViewModel>>.Ok(masjids));
        }
    }
}
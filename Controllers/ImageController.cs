using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
namespace Photoforge_Server.Controllers
{
    [ApiController]
    [Route("image/")]
    public class ImageController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("filters/")]
        [Consumes("application/octet-stream", "multipart/form-data")]
        public async Task<IActionResult> ApplyFilter([FromForm] ApplyFilterFormModel form)
        {



            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", form.Image.FileName);
            using (var img = Image.Load(form.Image.OpenReadStream()))
            {

                img.Mutate(x => x.GaussianBlur(5));
                img.Save(filePath);
            }
            //GaussianBlurExtensions.GaussianBlur(img, 3);
            //await form.Image.CopyToAsync(stream);

            return Ok("File uploaded successfully.");
        }

        [HttpPost]
        public IActionResult MergeLayers(IFormFile[] files)
        {

            return Ok();
        }
    }
}

public class ApplyFilterFormModel
{
    public IFormFile Image { get; set; }
    public float brightnees { get; set; }
    public float vibrance { get; set; }
    public float hue { get; set; }

}
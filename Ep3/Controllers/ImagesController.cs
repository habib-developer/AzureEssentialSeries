using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ep3.Controllers
{
    public class ImagesController : Controller
    {
        private readonly BlobServiceClient _blobClient;
        private readonly BlobContainerClient _containerClient;
        const string ContainerName = "my-images-profile";
        public ImagesController()
        {
            _blobClient = new BlobServiceClient("UseDevelopmentStorage=true");
            _containerClient = _blobClient.GetBlobContainerClient(ContainerName);
            _containerClient.CreateIfNotExists();
        }
        // GET: ImagesController
        public ActionResult Index()
        {
            var blobs = _containerClient.GetBlobs();
            return View(blobs);
        }

        // GET: ImagesController/Details/5
        public ActionResult Details(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var url = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read,DateTimeOffset.UtcNow.AddDays(1));
            ViewBag.url = url;
            return View();
        }
        // GET : Downloading file
        public ActionResult Download(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);

            var ms = new MemoryStream();
            blobClient.DownloadTo(ms);
            ms.Position = 0;

            return File(ms, "application/octent-stream", blobName);
        }
        // GET: ImagesController/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: ImagesController/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IFormFile file)
        {
            try
            {
                if(file is null || file.Length == 0)
                {
                    return BadRequest();
                }
                _containerClient.UploadBlob(file.FileName, file.OpenReadStream());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // POST: ImagesController/Delete/5
        [HttpGet]
        public ActionResult Delete(string blobName)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(blobName);

                blobClient.DeleteIfExists();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

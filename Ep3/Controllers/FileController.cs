using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ep3.Controllers
{
    public class FileController : Controller
    {
        readonly BlobServiceClient _client;
        readonly BlobContainerClient _containerClient;
        const string ContainerName = "my-file-container";
        public FileController()
        {
            _client = new BlobServiceClient("UseDevelopmentStorage=true");
            _containerClient = _client.GetBlobContainerClient(ContainerName);
        }
        // GET: FileController
        public ActionResult Index()
        {
            var files = _containerClient.GetBlobs();
            return View(files);
        }

        // GET: FileController/Details/5
        public ActionResult Details(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            ViewBag.uri =blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read,DateTimeOffset.Now.AddDays(1));
            return View();
        }

        // GET: FileController/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: FileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IFormFile file)
        {
            try
            {
                var container = _containerClient.UploadBlob(file.FileName, file.OpenReadStream());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            blobClient.Delete();
            return RedirectToAction("Index");
        }
        public ActionResult Download(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var ms = new MemoryStream();
            var file = blobClient.DownloadTo(ms);
            ms.Position = 0;
            return File(ms, "application/octet-stream", blobName);
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace TafelFlyingServices.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        public string Type { get; set; }
        public decimal Amount { get; set; }
        public Photo Photo { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public int InvoiceNumber
        {
            get
            {
                var _db = new ApplicationDbContext();
                try
                {
                    return _db.Trips.Find(TripId).InvoiceNumber;
                }
                catch
                {
                    return _db.Trips.Find(Trip.TripId).InvoiceNumber;
                }
            }
        }
    }

    public class ExpenseModel
    {
        public int ExpenseId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public Photo PhotoSource { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public int TripId { get; set; }
    }

    public class Photo
    {
        public String ContentType { get; set; }
        public byte[] ImageBytes { get; set; }
        public String SourceFilename { get; set; }
    }

    public class ImageResult : ActionResult
    {
        public ImageResult(String sourceFilename, String contentType)
        {
            SourceFilename = sourceFilename;
            ContentType = contentType;
        }

        //This is used for when you have the actual image in byte form
        //  which is more important for this post.
        public ImageResult(byte[] sourceStream, String contentType)
        {
            ImageBytes = sourceStream;
            ContentType = contentType;
        }

        public String ContentType { get; set; }
        public byte[] ImageBytes { get; set; }
        public String SourceFilename { get; set; }

        //This is used for times where you have a physical location

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = ContentType;

            //Check to see if this is done from bytes or physical location
            //  If you're really paranoid you could set a true/false flag in
            //  the constructor.
            if (ImageBytes != null)
            {
                var stream = new MemoryStream(ImageBytes);
                stream.WriteTo(response.OutputStream);
                stream.Dispose();
            }
            else
            {
                response.TransmitFile(SourceFilename);
            }
        }
    }
}
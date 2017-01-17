using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace RatiusCommunityApp.Models
{
    public class imageForBlob
    {



        public static byte[] ResizeImageFile(byte[] imageFile, int targetSize) // Set targetSize to 1024
        {
            using (System.Drawing.Image oldImage = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
            {
                Size newSize = CalculateDimensions(oldImage.Size, targetSize);
                using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                        MemoryStream m = new MemoryStream();
                        newImage.Save(m, ImageFormat.Jpeg);
                        return m.GetBuffer();
                    }
                }
            }
        }


        public static Size CalculateDimensions(Size oldSize, int targetSize)
        {
            Size newSize = new Size();
            if (oldSize.Height > oldSize.Width)
            {
                newSize.Width = (int)(oldSize.Width * ((float)targetSize / (float)oldSize.Height));
                newSize.Height = targetSize;
            }
            else
            {
                newSize.Width = targetSize;
                newSize.Height = (int)(oldSize.Height * ((float)targetSize / (float)oldSize.Width));
            }
            return newSize;
        }








        string image;
         public string ConvertImageForBlob()
        {
             
            var httpRequest = System.Web.HttpContext.Current.Request;
            HttpContext.Current.Server.ScriptTimeout = 990000;
           
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        Byte[] fileData = new Byte[httpRequest.Files[0].InputStream.Length];
                        if (postedFile.ContentLength != 0)
                        {
                            httpRequest.Files[0].InputStream.Read(fileData, 0, Convert.ToInt32(httpRequest.Files[0].InputStream.Length));

                            CloudStorageAccount storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ratiuscommunityapp"));
                            CloudBlobClient client = storage.CreateCloudBlobClient();
                            CloudBlobContainer container = client.GetContainerReference("mycontainer");
                            container.CreateIfNotExists();
                            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                            CloudBlockBlob blockblob = container.GetBlockBlobReference("myblob" + Guid.NewGuid() + ".jpg");
                            fileData = ResizeImageFile(fileData, 512);
                            using (MemoryStream ms = new MemoryStream(fileData))
                            {
                                blockblob.UploadFromStream(ms);
                                image = blockblob.Uri.AbsoluteUri;
                            }
                        }
                    }
                 
                }
             
            
            return image;
    }

         public string ConvertfileForBlob()
         {

             var httpRequest = System.Web.HttpContext.Current.Request;
             HttpContext.Current.Server.ScriptTimeout = 990000;

             if (httpRequest.Files.Count > 0)
             {
                 var docfiles = new List<string>();
                 foreach (string file in httpRequest.Files)
                 {
                     var postedFile = httpRequest.Files[file];
                     var filename = postedFile.FileName;
                     FileInfo fi = new FileInfo(filename);
                     string fileType = fi.Extension;
                     Byte[] fileData = new Byte[httpRequest.Files[0].InputStream.Length];
                     if (postedFile.ContentLength != 0)
                     {
                         httpRequest.Files[0].InputStream.Read(fileData, 0, Convert.ToInt32(httpRequest.Files[0].InputStream.Length));

                         CloudStorageAccount storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ratiuscommunityapp"));
                         CloudBlobClient client = storage.CreateCloudBlobClient();
                         CloudBlobContainer container = client.GetContainerReference("mycontainer");
                         container.CreateIfNotExists();
                         container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                         CloudBlockBlob blockblob = container.GetBlockBlobReference("myblob" + Guid.NewGuid()  + fileType);
                         if (fileType == ".jpg")
                         {
                             fileData = ResizeImageFile(fileData, 768);
                          
                         }
                         using (MemoryStream ms = new MemoryStream(fileData))
                         {
                             blockblob.UploadFromStream(ms);

                         }
                         image = blockblob.Uri.AbsoluteUri;
                     }
                 }

             }


             return image;
         }



         public string ConvertImageForBlob(string file,int count)
         {

             var httpRequest = System.Web.HttpContext.Current.Request;           
                 var docfiles = new List<string>();
                 
                     var postedFile = httpRequest.Files[file];
                     if (postedFile.ContentLength != 0)
                     {
                         Byte[] fileData = new Byte[httpRequest.Files[count].InputStream.Length];
                         httpRequest.Files[count].InputStream.Read(fileData, 0, Convert.ToInt32(httpRequest.Files[count].InputStream.Length));

                         CloudStorageAccount storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ratiuscommunityapp"));
                         CloudBlobClient client = storage.CreateCloudBlobClient();
                         CloudBlobContainer container = client.GetContainerReference("mycontainer");
                         container.CreateIfNotExists();
                         container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                         CloudBlockBlob blockblob = container.GetBlockBlobReference("myblob" + Guid.NewGuid() + ".jpg");
                         fileData = ResizeImageFile(fileData, 512);

                         using (MemoryStream ms = new MemoryStream(fileData))
                         {
                             blockblob.UploadFromStream(ms);
                             image = blockblob.Uri.AbsoluteUri;
                         }



                     }

             return image;
         }


}}
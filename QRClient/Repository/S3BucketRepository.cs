using Amazon.S3;
using Amazon.S3.Transfer;
using System.Drawing;

namespace QRClient.Repository
{
    public class S3BucketRepository : IS3BucketRepository
    {
        public void InsertIntoBucket(Bitmap imageBitmap, string qRstring)
        {
            // TODO: Insert your credentials
            var accessKeyId = "lol";
            var secretAccessKey = "lol";
            var region = Amazon.RegionEndpoint.APSouth1; // Change the region to your desired region
            // TODO: Insert your bucket Name
            var bucketName = "qrbucket";
            var keyName = $"{qRstring}.png";

            // Convert the bitmap to a MemoryStream
            using (MemoryStream stream = new MemoryStream())
            {
                imageBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                // Create an S3 client
                using (var s3Client = new AmazonS3Client(accessKeyId, secretAccessKey, region))
                {
                    // Create a TransferUtility
                    using (var transferUtility = new TransferUtility(s3Client))
                    {
                        // Upload the image to S3
                        transferUtility.Upload(stream, bucketName, keyName);
                    }
                }
            }
        }
    }
}

using Amazon.S3;
using Amazon.S3.Transfer;
using System.Drawing;

namespace QRClient.Repository
{
    public class S3BucketRepository : IS3BucketRepository
    {
        public void InsertIntoRepo(Bitmap imageBitmap)
        {
            var accessKeyId = "your_access_key_id";
            var secretAccessKey = "your_secret_access_key";
            var region = Amazon.RegionEndpoint.USEast1; // Change the region to your desired region

            var bucketName = "your_s3_bucket_name";
            var keyName = "your_image_name.png";

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

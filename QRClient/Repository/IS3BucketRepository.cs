using System.Drawing;

namespace QRClient.Repository
{
    public interface IS3BucketRepository
    {
        void InsertIntoBucket(Bitmap imageBitmap, string qRstring);
    }
}
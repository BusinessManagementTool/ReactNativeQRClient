using System.Drawing;

namespace QRClient.Repository
{
    public interface IS3BucketRepository
    {
        void InsertIntoRepo(Bitmap imageBitmap);
    }
}
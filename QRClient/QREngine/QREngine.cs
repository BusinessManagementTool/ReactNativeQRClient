using QRClient.Models;
using QRClient.Repository;
using System.Drawing;
using System.Text.Json;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace QRClient.QREngine
{
    public class QREngine : IQREngine
    {
        IS3BucketRepository s3BucketRepository;
        public QREngine(IS3BucketRepository _s3BucketRepository)
        {
            s3BucketRepository = _s3BucketRepository;
        }
        public string QRGenerator(IFormCollection formCollection, IWebHostEnvironment _webHostEnvironment)
        {
            var writter = new QRCodeWriter();
            BarCodeData barCodeData = new BarCodeData
            {
                QRstring = formCollection["QRstring"],
                CostPrice = Convert.ToDouble(formCollection["costprice"]),
                SellingPrice = Convert.ToDouble(formCollection["sellingprice"])
            };
            var jsonContent = JsonSerializer.Serialize(barCodeData);
            var resultBit = writter.encode(jsonContent, ZXing.BarcodeFormat.QR_CODE, 200, 200);
            var matrix = resultBit;
            int scale = 2;
            Bitmap result = new Bitmap(matrix.Width * scale, matrix.Height * scale);

            for (int i = 0; i < matrix.Height; i++)
            {
                for (int j = 0; j < matrix.Width; j++)
                {
                    Color pixel = matrix[i, j] ? Color.Black : Color.White;
                    for (int x = 0; x < scale; x++)
                    {
                        for (int y = 0; y < scale; y++)
                        {
                            result.SetPixel(i * scale + x, j * scale + y, pixel);
                        }
                    }
                }
            }
            //string webRootPath = _webHostEnvironment.WebRootPath;
            //result.Save(webRootPath + $"\\Images\\{barCodeData.QRstring}.png");
            s3BucketRepository.InsertIntoRepo(result);
            return $"\\Images\\{barCodeData.QRstring}.png";
        }

        public void QRReader(IWebHostEnvironment _webHostEnvironment)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var path = webRootPath + "\\Images\\QrcodeNew.png";
            var reader = new BarcodeReaderGeneric();
            Bitmap image = (Bitmap)Image.FromFile(path);
            using (image)
            {
                LuminanceSource source;
                source = new BitmapLuminanceSource(image);
                Result result = reader.Decode(source);
            }
        }

    }
}

namespace QRClient.QREngine
{
    public interface IQREngine
    {
        string QRGenerator(IFormCollection formCollection, IWebHostEnvironment _webHostEnvironment);

        void QRReader(IWebHostEnvironment _webHostEnvironment);
    }
}
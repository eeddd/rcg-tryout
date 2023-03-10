

namespace TRYOUT.Services
{
    public class Base64Service : IConverterService
    {
        string IConverterService.Convert(string text)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
        }
    }
}

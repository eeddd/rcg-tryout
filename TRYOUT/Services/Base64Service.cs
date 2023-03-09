

namespace TRYOUT.Services
{
    public class Base64Service : IBase64Service
    {
        string IBase64Service.ConvertToBase64(string text)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
        }
    }
}

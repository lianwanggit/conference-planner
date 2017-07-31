using SixLabors.Primitives;
using System.IO;

namespace FrontEnd.Services
{
    public interface IImageProcessor
    {
        void GenerateAvatar(Stream source, Size size, float cornerRadius, out MemoryStream dest);
    }
}

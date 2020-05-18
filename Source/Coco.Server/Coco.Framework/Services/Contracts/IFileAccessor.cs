using System.Text;

namespace Coco.Framework.Services.Contracts
{
    public interface IFileAccessor
    {
        string ReadAllText(string path, Encoding encoding);
    }
}

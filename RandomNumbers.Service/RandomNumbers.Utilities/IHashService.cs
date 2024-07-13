using System.Threading.Tasks;

namespace RandomNumbers.Utilities
{
    public interface IHashService
    {
        Task<string> HashText(string plainText);
    }
}

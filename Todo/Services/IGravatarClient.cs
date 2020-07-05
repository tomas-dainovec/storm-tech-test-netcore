using System.Threading.Tasks;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public interface IGravatarClient
    {
        Task<GravatarProfile> GetGravatarProfile(string emailHash);
    }
}
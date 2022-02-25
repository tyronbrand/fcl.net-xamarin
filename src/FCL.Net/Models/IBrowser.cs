using System.Threading.Tasks;

namespace FCL.Net.Models
{
    public interface IBrowser
    {
        Task<FclAuthServiceResponse> AuthenticateAsync(AuthenticateParams authenticateParams);
    }
}

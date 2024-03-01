using GenerateurCodes.MVC.Models;

namespace GenerateurCodes.MVC.Interfaces
{
    public interface IGenerateurCodesService
    {
        public Task<HttpResponseMessage> DemanderCode(DemandeCodeAcces demandeCodeAcces);
    }
}

using IdentityServer4.Models;
using IdentityServer4.Stores;
using wms.ids.business.Services;

namespace wms.ids.web
{
    /// <summary>
    /// https://identityserver.github.io/Documentation/docsv2/configuration/clients.html
    /// </summary>
    public class IDSClientStore : IClientStore
    {
        private readonly IClientService _clientService;
     
        public IDSClientStore(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var clients = await _clientService.ReadClient();
            var client = clients.SingleOrDefault(i => i.ClientId == clientId || i.ClientName == clientId);

            return client;
        }
    }
}
using Duende.IdentityServer.Models;

namespace CRM.IdentityServer.Config
{
	public static class IdentityConfig
	{
		public static IEnumerable<Client> GetClients() =>
			[
			new() {
				ClientId = "crm_client",
				AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
				ClientSecrets = { new Secret("secret".Sha256()) },
				AllowedScopes = { "crm_api" , "offline_access"},
				AllowOfflineAccess = true
			}
			];


		public static IEnumerable<ApiScope> GetApiScopes() =>
			[
			new ApiScope("crm_api", "CRM API")
			];

		public static IEnumerable<ApiResource> GetApiResources() =>
			[
			new ApiResource("crm_api_resource", "CRM API Resource")
			{
				Scopes = { "crm_api" }
			}];
	}

}

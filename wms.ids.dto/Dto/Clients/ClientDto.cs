namespace wms.ids.dto
{
    public class ClientDto
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
        public int Flow { get; set; }
        public bool? Enable { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string RedirectUri { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool? RequireConsent { get; set; }
        public bool? AllowRememberConsent { get; set; }
        public bool AllowClientCredentialsOnly { get; set; }
        public string LogoutUri { get; set; }
        public bool LogoutSessionRequired { get; set; }
        public bool RequireSignOutPrompt { get; set; }
        public bool? AllowAccessTokensViaBrowser { get; set; }
        public string AllowedCustomGrantTypes { get; set; }
        public int? IdentityTokenLifetime { get; set; }
        public int? AccessTokenLifetime { get; set; }
        public int? AuthorizationCodeLifetime { get; set; }
        public int? AbsoluteRefreshTokenLifetime { get; set; }
        public int? SlidingRefreshTokenLifetime { get; set; }
        public int? RefreshTokenUsage { get; set; }
        public int? RefreshTokenExpiration { get; set; }
        public bool? UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int? AccessTokenType { get; set; }
        public bool? EnableLocalLogin { get; set; }
        public bool? IncludeJwtId { get; set; }
        public bool? AlwaysSendClientClaims { get; set; }
        public bool PrefixClientClaims { get; set; }
        public bool IsInit { get; set; }
        public bool IsDefault { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

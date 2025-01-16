
GO
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([ID], [ClientID], [ClientName], [ClientSecret], [Flow], [Enable], [PostLogoutRedirectUri], [RedirectUri], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowClientCredentialsOnly], [LogoutUri], [LogoutSessionRequired], [RequireSignOutPrompt], [AllowAccessTokensViaBrowser], [AllowedCustomGrantTypes], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [RefreshTokenExpiration], [UpdateAccessTokenClaimsOnRefresh], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [PrefixClientClaims], [IsInit], [IsDefault], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (1, N'auth-uat.stc.vn', N'auth-uat.stc.vn', N'1xyz2', 4, 1, NULL, N'https://localhost:7102/swagger/ui/o2c-html;http://localhost:4823/swagger/ui/o2c-html', NULL, NULL, 0, 0, 0, NULL, 0, 0, 1, 1, 86400, 86400, 86400, 2592000, 2592000, 1, 0, NULL, NULL, 1, NULL, NULL, NULL, 0, 1, 1, CAST(N'2024-09-28T11:21:12.683' AS DateTime), 1, CAST(N'2024-09-28T11:21:12.683' AS DateTime), 0)

INSERT [dbo].[Client] ([ID], [ClientID], [ClientName], [ClientSecret], [Flow], [Enable], [PostLogoutRedirectUri], [RedirectUri], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowClientCredentialsOnly], [LogoutUri], [LogoutSessionRequired], [RequireSignOutPrompt], [AllowAccessTokensViaBrowser], [AllowedCustomGrantTypes], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [RefreshTokenExpiration], [UpdateAccessTokenClaimsOnRefresh], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [PrefixClientClaims], [IsInit], [IsDefault], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (2, N'Swagger.ClientCredential', N'Swagger.ClientCredential', N'8Czc8z', 1, 1, NULL, N'https://localhost:7102/swagger/oauth2-redirect.html;', NULL, NULL, 0, 0, 0, NULL, 0, 0, 1, 1, 86400, 86400, 86400, 2592000, 2592000, 1, 0, NULL, NULL, 1, NULL, NULL, NULL, 0, 1, 1, CAST(N'2024-09-28T11:21:12.683' AS DateTime), 1, CAST(N'2024-09-28T11:21:12.683' AS DateTime), 0)

INSERT [dbo].[Client] ([ID], [ClientID], [ClientName], [ClientSecret], [Flow], [Enable], [PostLogoutRedirectUri], [RedirectUri], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowClientCredentialsOnly], [LogoutUri], [LogoutSessionRequired], [RequireSignOutPrompt], [AllowAccessTokensViaBrowser], [AllowedCustomGrantTypes], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [RefreshTokenExpiration], [UpdateAccessTokenClaimsOnRefresh], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [PrefixClientClaims], [IsInit], [IsDefault], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (3, N'non-auth.stc.vn', N'non-auth.stc.vn', N'1xyz2', 4, 1, NULL, N'https://localhost:7102/swagger/ui/o2c-html;http://localhost:4823/swagger/ui/o2c-html', NULL, NULL, 0, 0, 0, NULL, 0, 0, 1, 1, 86400, 86400, 86400, 2592000, 2592000, 1, 0, NULL, NULL, 1, NULL, NULL, NULL, 0, 1, 1, CAST(N'2024-09-28T11:21:12.683' AS DateTime), 1, CAST(N'2024-09-28T11:21:12.683' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Client] OFF
GO
SET IDENTITY_INSERT [dbo].[ClientScope] ON 

INSERT [dbo].[ClientScope] ([ID], [ClientID], [ScopeID], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted])
VALUES (1, 1, 1, 1, CAST(N'2024-09-28T11:22:48.877' AS DateTime), 1, CAST(N'2024-09-28T11:22:48.877' AS DateTime), 0)

INSERT [dbo].[ClientScope] ([ID], [ClientID], [ScopeID], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (2, 2, 2, 1, CAST(N'2024-09-28T13:58:57.963' AS DateTime), 1, CAST(N'2024-09-28T13:58:57.963' AS DateTime), 0)

INSERT [dbo].[ClientScope] ([ID], [ClientID], [ScopeID], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (3, 3, 3, 1, CAST(N'2024-09-28T11:22:48.877' AS DateTime), 1, CAST(N'2024-09-28T11:22:48.877' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[ClientScope] OFF
GO
SET IDENTITY_INSERT [dbo].[Scope] ON 

INSERT [dbo].[Scope] ([ScopeID], [ScopeName], [DisplayName], [Description], [ScopeType], [Enable], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (1, N'auth-api', N'auth-api', N'auth-api', 1, 1, 1, CAST(N'2024-09-28T11:24:20.930' AS DATETIME), 1, CAST(N'2024-09-28T11:24:20.930' AS DATETIME), 0)

INSERT [dbo].[Scope] ([ScopeID], [ScopeName], [DisplayName], [Description], [ScopeType], [Enable], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (2, N'Swagger', N'Swagger', N'Swagger', 1, 1, 1, CAST(N'2024-09-28T13:58:29.803' AS DateTime), 1, CAST(N'2024-09-28T13:58:29.803' AS DateTime), 0)

INSERT [dbo].[Scope] ([ScopeID], [ScopeName], [DisplayName], [Description], [ScopeType], [Enable], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (3, N'non-auth-api', N'non-auth-api', N'non-auth-api', 1, 1, 1, CAST(N'2024-09-28T11:24:20.930' AS DateTime), 1, CAST(N'2024-09-28T11:24:20.930' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Scope] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserID], [Username], [FullName], [Password], [Avatar], [IsAdmin], [Gender], [UserTypeID], [LastChangedPassword], [IsActive], [ActiveDate], [IsLocked], [LockedDate], [IsInit], [IsRequireVerify], [Enable2FA], [Secret2FAKey], [CreatedUser], [CreatedDate], [UpdatedUser], [UpdatedDate], [IsDeleted]) 
VALUES (1, N'system', N'Hệ thống', N'25f9e794323b453885f5181f1b624d0b', NULL, 1, 1, 1, CAST(N'2024-09-28T11:30:50.410' AS DateTime), 1, CAST(N'2024-09-28T11:30:50.410' AS DateTime), 0, NULL, 0, 0, 0, NULL, 1, CAST(N'2024-09-28T11:30:50.410' AS DateTime), 1, CAST(N'2024-09-28T11:30:50.410' AS DateTime), 0)

SET IDENTITY_INSERT [dbo].[User] OFF
GO

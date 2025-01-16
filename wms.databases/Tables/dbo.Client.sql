CREATE TABLE [dbo].[Client](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ClientID] [nvarchar](50) NULL,
	[ClientName] [nvarchar](255) NULL,
	[ClientSecret] [nvarchar](255) NULL,
	[Flow] [int] NULL,
	[Enable] [bit] NULL,
	[PostLogoutRedirectUri] [nvarchar](2000) NULL,
	[RedirectUri] [varchar](max) NULL,
	[ClientUri] [nvarchar](255) NULL,
	[LogoUri] [nvarchar](255) NULL,
	[RequireConsent] [bit] NULL,
	[AllowRememberConsent] [bit] NULL,
	[AllowClientCredentialsOnly] [bit] NULL,
	[LogoutUri] [nvarchar](255) NULL,
	[LogoutSessionRequired] [bit] NULL,
	[RequireSignOutPrompt] [bit] NULL,
	[AllowAccessTokensViaBrowser] [bit] NULL,
	[AllowedCustomGrantTypes] [bit] NULL,
	[IdentityTokenLifetime] [int] NULL,
	[AccessTokenLifetime] [int] NULL,
	[AuthorizationCodeLifetime] [int] NULL,
	[AbsoluteRefreshTokenLifetime] [int] NULL,
	[SlidingRefreshTokenLifetime] [int] NULL,
	[RefreshTokenUsage] [tinyint] NULL,
	[RefreshTokenExpiration] [tinyint] NULL,
	[UpdateAccessTokenClaimsOnRefresh] [bit] NULL,
	[AccessTokenType] [tinyint] NULL,
	[EnableLocalLogin] [bit] NULL,
	[IncludeJwtId] [bit] NULL,
	[AlwaysSendClientClaims] [bit] NULL,
	[PrefixClientClaims] [bit] NULL,
	[IsInit] [bit] NULL,
	[IsDefault] [bit] NULL,
	[CreatedUser] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedUser] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_Flow]  DEFAULT ((0)) FOR [Flow]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_RequireConsent]  DEFAULT ((0)) FOR [RequireConsent]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_IsInit]  DEFAULT ((1)) FOR [IsInit]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_IsDefault]  DEFAULT ((0)) FOR [IsDefault]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_CreatedUser]  DEFAULT ((1)) FOR [CreatedUser]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_UpdatedUser]  DEFAULT ((1)) FOR [UpdatedUser]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_IsDeleted]  DEFAULT ((1)) FOR [IsDeleted]
GO
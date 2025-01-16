CREATE TABLE [dbo].[Scope](
	[ScopeID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ScopeName] [nvarchar](50) NULL,
	[DisplayName] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ScopeType] [tinyint] NULL,
	[Enable] [bit] NULL,
	[CreatedUser] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedUser] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Scope] PRIMARY KEY CLUSTERED 
(
	[ScopeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Scope] ADD  CONSTRAINT [DF_Scope_CreatedUser]  DEFAULT ((1)) FOR [CreatedUser]
GO

ALTER TABLE [dbo].[Scope] ADD  CONSTRAINT [DF_Scope_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Scope] ADD  CONSTRAINT [DF_Scope_UpdatedUser]  DEFAULT ((1)) FOR [UpdatedUser]
GO

ALTER TABLE [dbo].[Scope] ADD  CONSTRAINT [DF_Scope_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

ALTER TABLE [dbo].[Scope] ADD  CONSTRAINT [DF_Scope_IsDeleted]  DEFAULT ((1)) FOR [IsDeleted]
GO
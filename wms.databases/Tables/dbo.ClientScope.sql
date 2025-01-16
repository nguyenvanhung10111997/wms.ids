CREATE TABLE [dbo].[ClientScope](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ClientID] [int] NOT NULL,
	[ScopeID] [int] NOT NULL,
	[CreatedUser] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedUser] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ClientScope] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ClientScope] ADD  CONSTRAINT [DF_Client_Scope_CreatedUser]  DEFAULT ((1)) FOR [CreatedUser]
GO

ALTER TABLE [dbo].[ClientScope] ADD  CONSTRAINT [DF_Client_Scope_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ClientScope] ADD  CONSTRAINT [DF_Client_Scope_UpdatedUser]  DEFAULT ((1)) FOR [UpdatedUser]
GO

ALTER TABLE [dbo].[ClientScope] ADD  CONSTRAINT [DF_Client_Scope_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

ALTER TABLE [dbo].[ClientScope] ADD  CONSTRAINT [DF_Client_Scope_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

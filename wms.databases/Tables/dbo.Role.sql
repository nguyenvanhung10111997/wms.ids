CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[RoleName] [nvarchar](512) NOT NULL,
	[Description] [nvarchar](4000) NULL,
	[CreatedUser] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedUser] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_RoleName]  DEFAULT ('') FOR [RoleName]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_CreatedUser]  DEFAULT ((1)) FOR [CreatedUser]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_UpdatedUser]  DEFAULT ((1)) FOR [UpdatedUser]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

CREATE TABLE [dbo].[Permission](
	[PermissionID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[PermissionName] [nvarchar](512) NULL,
	[IsActive] [bit] NULL,
	[Description] [nvarchar](4000) NULL,
	[CreatedUser] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedUser] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Permission] ADD  DEFAULT ('') FOR [PermissionName]
GO

ALTER TABLE [dbo].[Permission] ADD  DEFAULT ((1)) FOR [CreatedUser]
GO

ALTER TABLE [dbo].[Permission] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Permission] ADD  DEFAULT ((1)) FOR [UpdatedUser]
GO

ALTER TABLE [dbo].[Permission] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO

ALTER TABLE [dbo].[Permission] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
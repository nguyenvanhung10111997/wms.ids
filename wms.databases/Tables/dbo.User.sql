CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Username] [nvarchar](512) NULL,
	[FullName] [nvarchar](512) NULL,
	[Password] [varchar](512) NULL,
	[Avatar] [nvarchar](512) NULL,
	[IsAdmin] [bit] NULL,
	[Gender] [int] NULL,
	[UserTypeID] [int] NULL,
	[LastChangedPassword] [datetime] NULL,
	[IsActive] [bit] NULL,
	[ActiveDate] [datetime] NULL,
	[IsLocked] [bit] NULL,
	[LockedDate] [datetime] NULL,
	[IsInit] [bit] NOT NULL,
	[IsRequireVerify] [bit] NULL,
	[Enable2FA] [bit] NOT NULL,
	[Secret2FAKey] [varchar](128) NULL,
	[CreatedUser] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedUser] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_UserTypeID]  DEFAULT ((0)) FOR [UserTypeID]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsActived]  DEFAULT ((0)) FOR [IsActive]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsLocked]  DEFAULT ((0)) FOR [IsLocked]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsInit]  DEFAULT ((1)) FOR [IsInit]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsRequireVerify]  DEFAULT ((1)) FOR [IsRequireVerify]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__User__Enable2FA__18427513]  DEFAULT ((0)) FOR [Enable2FA]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreatedUser]  DEFAULT ((0)) FOR [CreatedUser]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_UpdatedUser]  DEFAULT ((0)) FOR [UpdatedUser]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

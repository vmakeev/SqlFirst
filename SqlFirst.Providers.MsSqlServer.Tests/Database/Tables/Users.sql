USE [SqlFirstTestDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DisplayedName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [uniqueidentifier] NULL,
	[RoleGroupId] [bigint] NULL,
	[IsMale] [bit] NULL,
	[DateOfBirth] [date] NULL,
	[ExternalId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoleGroups] FOREIGN KEY([RoleGroupId])
REFERENCES [dbo].[UserRoleGroups] ([Id])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoleGroups]
GO



USE [SqlFirstTestDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GroupsToRoles](
	[RoleId] [bigint] NOT NULL,
	[RoleGroupId] [bigint] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GroupsToRoles]  WITH CHECK ADD  CONSTRAINT [FK_GroupsToRoles_UserRoleGroups] FOREIGN KEY([RoleGroupId])
REFERENCES [dbo].[UserRoleGroups] ([Id])
GO

ALTER TABLE [dbo].[GroupsToRoles] CHECK CONSTRAINT [FK_GroupsToRoles_UserRoleGroups]
GO

ALTER TABLE [dbo].[GroupsToRoles]  WITH CHECK ADD  CONSTRAINT [FK_GroupsToRoles_UserRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserRoles] ([Id])
GO

ALTER TABLE [dbo].[GroupsToRoles] CHECK CONSTRAINT [FK_GroupsToRoles_UserRoles]
GO



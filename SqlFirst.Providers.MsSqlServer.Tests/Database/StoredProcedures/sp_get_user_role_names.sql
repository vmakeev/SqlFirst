USE [SqlFirstTestDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_get_user_role_names] 
	@email nvarchar(50)
AS
BEGIN

	SET NOCOUNT ON;

	select 
	u.DisplayedName as UserName, 
	u.Email as UserEmail, 
	r.Name as RoleName
	from 
		Users u 
		left join UserRoleGroups rg on rg.Id = u.RoleGroupId
		left join GroupsToRoles gtr on gtr.RoleGroupId = rg.Id
		left join UserRoles r on r.Id = gtr.RoleId

	where (@email is null OR u.Email = @email)
END
GO



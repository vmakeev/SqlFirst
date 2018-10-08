USE [SqlFirstTestDb]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE[dbo].[sp_delete_users_in_groups]
	@groupIds dbo.IntegerList readonly
AS
BEGIN


SET NOCOUNT ON;

delete from Users

where RoleGroupId in (select IntValue from @groupIds)
END

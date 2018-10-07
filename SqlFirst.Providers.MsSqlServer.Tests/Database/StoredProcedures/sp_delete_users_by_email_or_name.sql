USE[SqlFirstTestDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE[dbo].[sp_delete_users_by_email_or_name]
@dictionary dbo.KeyValueStringList readonly
AS
BEGIN


SET NOCOUNT ON;

delete from Users

where email in (select[key] from @dictionary) and displayedname in (select[value] from @dictionary)
END
GO



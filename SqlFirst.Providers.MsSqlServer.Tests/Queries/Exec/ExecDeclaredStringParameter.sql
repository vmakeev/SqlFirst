-- begin variables

declare @emailAddress nvarchar(50);

-- end

exec sp_get_user_role_names @email = @emailAddress

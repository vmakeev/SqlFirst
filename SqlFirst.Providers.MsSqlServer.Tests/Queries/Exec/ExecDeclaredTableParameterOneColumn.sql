-- begin variables

declare @groupIds IntegerList;

-- end

exec sp_delete_users_in_groups @groupIds
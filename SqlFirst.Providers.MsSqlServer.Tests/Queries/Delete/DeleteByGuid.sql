
-- begin variables

declare @externalId uniqueidentifier;

-- end

delete from Users
where ExternalId = @externalId
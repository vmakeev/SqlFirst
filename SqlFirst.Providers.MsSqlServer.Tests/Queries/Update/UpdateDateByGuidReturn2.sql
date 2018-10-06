-- begin variables

declare @externalId uniqueidentifier;
declare @birthDate datetime;

-- end

update Users
set DateOfBirth = @birthDate
output INSERTED.id
where ExternalId = @externalId
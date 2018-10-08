-- begin variables

declare @externalId uniqueidentifier;
declare @birthDate datetime;

-- end

update Users
set DateOfBirth = @birthDate
output INSERTED.id, INSERTED.Email
where ExternalId = @externalId
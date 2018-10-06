
-- begin variables

declare @externalId uniqueidentifier;
declare @birthDate datetime;

-- end

update Users
set DateOfBirth = @birthDate
where ExternalId = @externalId
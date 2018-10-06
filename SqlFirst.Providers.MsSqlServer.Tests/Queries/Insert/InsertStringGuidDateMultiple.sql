
-- begin variables

declare @email_N varchar(255);
declare @externalId_N uniqueidentifier;
declare @birthDate_N datetime;

-- end

insert into Users (Email, ExternalId, DateOfBirth)
values (@email_N, @externalId_N, @birthDate_N);
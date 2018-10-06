-- begin sqlFirstOptions

-- test my option

-- end


-- begin variables

declare @email varchar(255);
declare @externalId uniqueidentifier;
declare @birthDate datetime;

-- end

insert into Users (Email, ExternalId, DateOfBirth)
values (@email, @externalId, @birthDate);
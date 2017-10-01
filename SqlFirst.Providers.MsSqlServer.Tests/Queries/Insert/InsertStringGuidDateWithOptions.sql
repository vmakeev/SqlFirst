-- begin sqlFirstOptions

-- test my option

/* some not interesting
comments */

-- generate item  struct inpc

-- end

-- begin variables

declare @userKey varchar(255);
declare @caseId uniqueidentifier;
declare @createDateUtc datetime;

-- end

insert into CaseSubscriptions (UserKey, CaseId, CreateDateUtc)
values (@userKey, @caseId, @createDateUtc);
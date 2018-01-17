
-- begin variables

declare @userKey_N varchar(255);
declare @caseId uniqueidentifier;
declare @createDateUtc datetime;

-- end

insert into CaseSubscriptions (UserKey, CaseId, CreateDateUtc)
output inserted.id, inserted.userkey
values (@userKey_N, @caseId, @createDateUtc);
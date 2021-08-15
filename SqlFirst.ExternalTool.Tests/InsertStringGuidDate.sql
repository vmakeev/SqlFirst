insert into CaseSubscriptions (UserKey, CaseId, CreateDateUtc)
output inserted.userKey, inserted.id
values (@userKey_N, @caseId, @createDateUtc);
--values (@userKey, @caseId, @createDateUtc);
--values (@userKey, @caseId, @date);

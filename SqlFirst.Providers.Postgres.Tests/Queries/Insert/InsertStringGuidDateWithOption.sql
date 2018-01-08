-- begin sqlFirstOptions

-- test my option

-- end

insert into CaseSubscriptions (UserKey, CaseId, CreateDateUtc)
values (@userKey, @caseId, @createDateUtc);
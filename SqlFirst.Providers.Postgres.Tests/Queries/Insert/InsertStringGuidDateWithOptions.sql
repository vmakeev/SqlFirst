-- begin sqlFirstOptions

-- test my option

/* some not interesting
comments */

-- generate item  struct inpc

-- end

insert into CaseSubscriptions (UserKey, CaseId, CreateDateUtc)
values (@userKey, @caseId, @createDateUtc);

-- begin  mySpecialSection

--simple test

-- end

update CaseSubscriptions
set CreateDateUtc = @createDateUtc
where CaseId = @caseId
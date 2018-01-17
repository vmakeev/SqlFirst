
-- begin variables

declare @caseId uniqueidentifier;
declare @createDateUtc datetime;

-- end

update CaseSubscriptions
set CreateDateUtc = @createDateUtc
output INSERTED.id, INSERTED.CaseId
where CaseId = @caseId
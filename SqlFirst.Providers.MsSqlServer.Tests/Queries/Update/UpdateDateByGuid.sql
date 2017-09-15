
-- begin queryParameters

declare @caseId uniqueidentifier;
declare @createDateUtc datetime;

-- end

update CaseSubscriptions
set CreateDateUtc = @createDateUtc
where CaseId = @caseId
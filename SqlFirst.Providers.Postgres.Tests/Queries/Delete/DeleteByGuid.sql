﻿
-- begin variables

declare @caseId uniqueidentifier;

-- end

delete from CaseSubscriptions
where CaseId = @caseId
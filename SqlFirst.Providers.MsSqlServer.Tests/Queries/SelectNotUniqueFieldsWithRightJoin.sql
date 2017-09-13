﻿select top 10 cs.Id, de.Id from CaseSubscriptions cs  with(nolock)
right join DocumentEvents de with(nolock) on cs.CaseId = de.CaseId
where de.id is not null
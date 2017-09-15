
-- begin queryParameters

declare @userKey varchar(255);
declare @skip int;
declare @take int;

-- end

select CaseId, CreateDateUtc
from CaseSubscriptions with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only
/*exec sp_describe_undeclared_parameters N'select CaseId, CreateDateUtc
from CaseSubscribes with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only'*/

select CaseId, CreateDateUtc
from CaseSubscribes with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only
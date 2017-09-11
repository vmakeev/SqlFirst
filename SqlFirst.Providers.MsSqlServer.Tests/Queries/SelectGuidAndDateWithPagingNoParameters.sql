select CaseId, CreateDateUtc
from CaseSubscribes with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only
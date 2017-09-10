-- begin queryParameters

declare @userKey varchar(MAX) ='test'; 
declare @skip int = 42;

--end

select CaseId, CreateDateUtc
from CaseSubscribes with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only
/*
 Некоторое количество
 произвольного 
		текста
*/

-- begin sqlFirstOptions

-- enable Async

-- end




select CaseId, CreateDateUtc
from CaseSubscriptions
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip
limit @take
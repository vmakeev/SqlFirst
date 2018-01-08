/*
 Некоторое количество
 произвольного 
		текста
*/

-- begin sqlFirstOptions
-- disable all
-- enable Async

-- end
-- begin  mySpecialSection

--simple test

-- end



select CaseId, CreateDateUtc
from CaseSubscriptions 
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip 
limit @take
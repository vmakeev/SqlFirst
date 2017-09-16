/*
 Некоторое количество
 произвольного 
		текста
*/

-- begin sqlFirstOptions
-- disable all
-- enable Async

-- end
-- begin variables

declare @userKey varchar(MAX) ='test'; 

--end
-- begin  mySpecialSection

--simple test

-- end


-- begin variables

declare @take int = 42;

--end

select CaseId, CreateDateUtc
from CaseSubscriptions with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only
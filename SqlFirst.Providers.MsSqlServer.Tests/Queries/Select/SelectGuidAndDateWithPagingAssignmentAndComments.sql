
--    begin    variables

declare @userKey varchar(255) ='test' ; -- Это комментарий
declare @skip int = 42;/*
комментариев много 
		не бывает
		*/
declare @take int;

/*
 Можно будет добавить еще параметров
*/

--end

select CaseId, CreateDateUtc
from CaseSubscriptions with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only
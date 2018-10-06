
--    begin    variables

declare @email varchar(255) ='test@mail.com' ; -- Это комментарий
declare @skip int = 42;/*
комментариев много 
		не бывает
		*/
declare @take int;

/*
 Можно будет добавить еще параметров
*/

--end

select ExternalId, DateOfBirth
from Users with(nolock)
where Email = @email
order by Id desc
offset @skip rows
fetch next @take rows only
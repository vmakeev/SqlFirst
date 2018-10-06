
-- begin variables

declare @email varchar(255);
declare @skip int;
declare @take int;

-- end

select ExternalId, DateOfBirth
from Users with(nolock)
where Email = @email
order by Id desc
offset @skip rows
fetch next @take rows only
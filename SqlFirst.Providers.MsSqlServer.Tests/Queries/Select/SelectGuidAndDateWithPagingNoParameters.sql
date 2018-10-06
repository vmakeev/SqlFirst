select ExternalId, DateOfBirth
from Users with(nolock)
where Email = @email
order by Id desc
offset @skip rows
fetch next @take rows only
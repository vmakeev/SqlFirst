﻿
-- begin sqlFirstOptions

-- enable Async

-- end

-- begin variables

declare @email varchar(MAX) ='test@mail.com'; 

--end

-- begin variables

declare @take int = 42;

--end

select ExternalId, DateOfBirth
from Users with(nolock)
where Email = @email
order by Id desc
offset @skip rows
fetch next @take rows only
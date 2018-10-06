﻿select top 10 u.Email, rg.Name from Users u  with(nolock)
left join UserRoleGroups rg with(nolock) on rg.Id = u.RoleGroupId
where rg.id is not null
select top 10 u.Id as UserId, rg.Id as RoleGroupId from Users u  with(nolock)
inner join UserRoleGroups rg with(nolock) on rg.Id = u.RoleGroupId
where rg.id is not null
select top 10 cs.UserKey, de.ShardName from CaseSubscriptions cs  with(nolock)
inner join DocumentEvents de with(nolock) on cs.CaseId = de.CaseId
where de.id is not null
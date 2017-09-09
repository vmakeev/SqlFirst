select top 10 cs.UserKey, de.ShardName from CaseSubscribes cs  with(nolock)
left join DocumentEvents de with(nolock) on cs.CaseId = de.CaseId
where de.id is not null

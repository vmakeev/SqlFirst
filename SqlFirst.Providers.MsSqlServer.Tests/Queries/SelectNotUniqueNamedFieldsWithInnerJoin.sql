select top 10 cs.Id as SubscriptionId, de.Id as EventId from CaseSubscriptions cs  with(nolock)
inner join DocumentEvents de with(nolock) on cs.CaseId = de.CaseId
where de.id is not null
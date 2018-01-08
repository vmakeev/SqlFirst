select cs.Id as SubscriptionId, de.Id as EventId from CaseSubscriptions cs
inner join DocumentEvents de on cs.CaseId = de.CaseId
where de.id is not null
limit 10
select cs.Id, de.Id from CaseSubscriptions cs
left join DocumentEvents de on cs.CaseId = de.CaseId
where de.id is not null
limit 10
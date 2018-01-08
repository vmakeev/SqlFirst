select caseid, createdateutc
from casesubscriptions
where userkey = @userkey
order by createdateutc desc
limit @take
offset @skip
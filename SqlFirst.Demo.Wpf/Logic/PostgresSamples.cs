namespace SqlFirst.Demo.Wpf.Logic
{
	public class PostgresSamples : SamplesBase
	{
		protected override string GetSelectQuery()
		{
			return @"-- begin sqlFirstOptions

-- generate result class properties auto virtual
-- use querytext string
-- generate methods sync async get_first get_all

-- end

select *
from caseevents
where UserKey = @userKey
order by finddateutc desc
offset @skip
limit @take";
		}

		protected override string GetInsertQuery()
		{
			return @"-- begin sqlFirstOptions

-- generate result class properties auto virtual
-- generate parameter class
-- use querytext string
-- generate methods sync async add_single add_multiple

-- end

insert into caseevents (userKey, inn, ogrn, caseid, shardname, finddateutc) 
values (@userKey_N, @inn_N, @ogrn_N, @caseId_N, @shardName_N, @findDateUtc)
RETURNING id, userKey";
		}
	}
}
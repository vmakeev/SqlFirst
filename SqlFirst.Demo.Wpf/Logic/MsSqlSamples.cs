namespace SqlFirst.Demo.Wpf.Logic
{
	public class MsSqlSamples : SamplesBase
	{
		protected override string GetSelectQuery()
		{
			return @"-- begin sqlFirstOptions

-- generate result class properties auto virtual
-- use querytext string
-- generate methods sync async get_first get_all

-- end

-- begin variables 

declare @userKey varchar(MAX) ='test'; 

-- end

select *
from caseevents with(nolock)
where UserKey = @userKey
order by finddateutc desc
offset @skip rows
fetch next @take rows only";
		}

		protected override string GetInsertQuery()
		{
			return @"-- begin sqlFirstOptions

-- generate result class properties auto virtual
-- generate parameter class
-- use querytext string
-- generate methods sync async add_single add_multiple

-- end

-- begin variables 

declare @userKey_N varchar(MAX) ='test'; 

-- end

insert into caseevents (userKey, inn, ogrn, caseid, shardname, finddateutc) 
output inserted.id, inserted.userKey
values (@userKey_N, @inn_N, @ogrn_N, @caseId_N, @shardName_N, @findDateUtc)";
		}

		/// <inheritdoc />
		protected override string GetStoredProcedureQuery()
		{
			return @"EXEC	[dbo].[GetQueryStatForDatesShort]
		@dateFrom = @myDateFrom,
		@dateTo = @myDateTo;";
		}
	}
}
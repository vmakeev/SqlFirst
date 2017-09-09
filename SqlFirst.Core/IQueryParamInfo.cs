namespace SqlFirst.Core
{
	public interface IQueryParamInfo
	{
		string CsType { get; set; }
		int Length { get; set; }
		int Precision { get; set; }
		int Scale { get; set; }
		string CsName { get; set; }
		string DbName { get; set; }
		string DbType { get; set; }
	}
}
using System.Collections.Generic;

namespace SqlFirst.Core
{
	/// <summary>
	/// Значения по умолчанию опций кодогенератора SqlFirst
	/// </summary>
	public interface IOptionDefaults: IReadOnlyDictionary<string, IReadOnlyDictionary<string, bool>>
	{
		
	}
}
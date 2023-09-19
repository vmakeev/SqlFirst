using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Data
{
	internal class QueryObjectTemplate
	{
		private static readonly ILogger _log = LogManager.GetLogger<QueryObjectTemplate>();

		private readonly object _locker = new object();

		private readonly List<IQueryObjectAbility> _abilities = new List<IQueryObjectAbility>();

		public bool IsExists(string abilityName)
		{
			lock (_locker)
			{
				return IsExistsInternal(abilityName);
			}
		}

		public void AddAbility<T>(Func<bool> predicate) where T : IQueryObjectAbility, new()
		{
			if (predicate())
			{
				AddAbility(new T());
			}
		}

		public void AddAbility<T>() where T : IQueryObjectAbility, new()
		{
			AddAbility(new T());
		}

		public void AddAbility(IQueryObjectAbility ability)
		{
			lock (_locker)
			{
				if (IsExists(ability.Name))
				{
					throw new CodeGenerationException($"Ability [{ability.Name}] already exists.");
				}

				_abilities.Add(ability);

				_log.LogDebug($"Ability [{ability.Name}] added as [{ability.GetType().Name}]");
			}
		}

		public void RemoveAbility(string abilityName)
		{
			lock (_locker)
			{
				_abilities.RemoveAll(p => p.Name == abilityName);
			}
		}

		public void ClearAbilities()
		{
			lock (_locker)
			{
				_abilities.Clear();
			}
		}

		public IQueryObjectData GenerateData(ICodeGenerationContext context)
		{
			IQueryObjectData result = new QueryObjectData();

			lock (_locker)
			{
				foreach (IQueryObjectAbility ability in _abilities)
				{
					EnsureDependencies(ability);
					result = ability.Apply(context, result);
				}
			}

			return result;
		}

		private bool IsExistsInternal(string abilityName)
		{
			return _abilities.Any(p => p.Name == abilityName);
		}

		private void EnsureDependencies(IQueryObjectAbility ability)
		{
			foreach (string dependency in ability.GetDependencies())
			{
				if (_abilities.All(p => p.Name != dependency))
				{
					throw new CodeGenerationException($"Query object ability's [{ability.Name}] dependency [{dependency}] not resolved.");
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class PrepareCommandWithTimeoutAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string method = Snippet.Query.Methods.Common.PrepareCommandWithTimeout.Render();

			string property = Snippet.Property.Auto.AutoProperty.Render(
				new
				{
					Type = CSharpCodeHelper.GetTypeBuiltInName(typeof(TimeSpan?)),
					Name = "CommandTimeout"
				});
			string propertyCommentary = Snippet.Commentary.XmlDocumentationCommentary.Render(new { Content = "Таймаут исполнения команды" });

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.AppendItems(method);
			result.Properties = result.Properties.AppendItems(propertyCommentary + property);
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.Data");

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies()
		{
			yield break;
		}

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.PrepareCommand;
	}
}

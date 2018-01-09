using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlFirst.Codegen.Text.Templating
{
	public class RenderableTemplate : IRenderableTemplate
	{
		#region Nested

		protected class SnippetField
		{
			public string Placeholder { get; set; }

			public string Name { get; set; }

			public string EnumerableDelimiter { get; set; }

			public byte IndentSize { get; set; }

			public string Prefix { get; set; }

			public string Postfix { get; set; }
		}

		private class PropertyEqualityComparer : IEqualityComparer<SnippetField>
		{
			/// <summary>Determines whether the specified objects are equal.</summary>
			/// <param name="x">The first object of type T to compare.</param>
			/// <param name="y">The second object of type T to compare.</param>
			/// <returns>true if the specified objects are equal; otherwise, false.</returns>
			public bool Equals(SnippetField x, SnippetField y)
			{
				return string.Equals(x?.Placeholder, y?.Placeholder);
			}

			/// <summary>Returns a hash code for the specified object.</summary>
			/// <param name="obj">The <see cref="T:System.Object"></see> for which a hash code is to be returned.</param>
			/// <returns>A hash code for the specified object.</returns>
			/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj">obj</paramref> is a reference type and <paramref name="obj">obj</paramref> is null.</exception>
			public int GetHashCode(SnippetField obj)
			{
				return obj?.Placeholder.GetHashCode() ?? 0;
			}
		}

		#endregion

		private static readonly PropertyEqualityComparer _propertyEqualityComparer = new PropertyEqualityComparer();
		private readonly string _template;

		protected static readonly Regex _templatesRegex = new Regex(@"(?<placeholder>\$((?<indent>\d{1,2})\|)?(\`(?<prefix>[^\`]+)\`)?(?<name>[a-zA-Z0-9_]+)(\`(?<postfix>[^\`]+)\`)?(\|(?<delimiter>[^\$]*))?\$(~\r\n?)?)", RegexOptions.Compiled);
		private readonly SnippetField[] _snippetFields;
		private static readonly object[] _emptyArray = new object[0];

		/// <inheritdoc />
		public RenderableTemplate(string template)
		{
			_template = template ?? throw new ArgumentNullException(nameof(template));
			_snippetFields = GetItems(_template).Distinct(_propertyEqualityComparer).ToArray();
		}

		/// <inheritdoc />
		public string Render(object model)
		{
			if (model == null)
			{
				return _template;
			}

			IReadOnlyDictionary<string, MethodInfo> typeMetadata = MetadataCache.GetTypePropertiesMetadata(model.GetType());

			var sb = new StringBuilder(_template);
			foreach (SnippetField snippetField in _snippetFields)
			{
				string value = string.Empty;

				if (typeMetadata.TryGetValue(snippetField.Name, out MethodInfo methodInfo))
				{
					object modelValue = methodInfo.Invoke(model, _emptyArray);
					value = GetValue(modelValue, snippetField.EnumerableDelimiter);

					if (!string.IsNullOrEmpty(value))
					{
						value = string.Concat(snippetField.Prefix, value, snippetField.Postfix);

						if (snippetField.IndentSize > 0)
						{
							value = value.Indent("\t", snippetField.IndentSize);
						}
					}
				}

				sb.Replace(snippetField.Placeholder, value);
			}

			return sb.ToString();
		}

		protected string GetValue(object modelValue, string enumerableDelimiter)
		{
			switch (modelValue)
			{
				case null:
					return string.Empty;

				case string s:
					return s;

				case IRenderable renderable:
					return renderable.Render();

				case IEnumerable enumerable:
					var buffer = new LinkedList<string>();
					foreach (object enumerableValue in enumerable)
					{
						string enumerableValueString = GetValue(enumerableValue, enumerableDelimiter);
						buffer.AddLast(enumerableValueString);
					}
					return string.Join(enumerableDelimiter ?? string.Empty, buffer);

				default:
					return Convert.ToString(modelValue, CultureInfo.InvariantCulture);
			}
		}

		protected IEnumerable<SnippetField> GetItems(string template)
		{
			foreach (Match match in _templatesRegex.Matches(template))
			{
				var field = new SnippetField
				{
					Placeholder = match.Groups["placeholder"].Value,
					Name = match.Groups["name"].Value,
					EnumerableDelimiter = Unescape(match.Groups["delimiter"].Value),
					Prefix = Unescape(match.Groups["prefix"].Value),
					Postfix = Unescape(match.Groups["postfix"].Value)
				};

				if (byte.TryParse(match.Groups["indent"].Value, out byte indent))
				{
					field.IndentSize = indent;
				}

				yield return field;
			}
		}

		private string Unescape(string value)
		{
			if (!value.Contains(@"\"))
			{
				return value;
			}

			var sb = new StringBuilder(value);

			sb
			.Replace(@"\r", "\r")
			.Replace(@"\n", "\n")
			.Replace(@"\t", "\t");

			return sb.ToString();
		}

		public string Render()
		{
			return Render(null);
		}
	}
}
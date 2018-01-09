using System.Collections.Generic;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Fields
{
	internal class FieldSnippet : SqlFirstSnippet
	{
		#region Nested

		internal class ConstRenderableTemplate : RenderableTemplate
		{
			/// <inheritdoc />
			public ConstRenderableTemplate(string template)
				: base(template)
			{
			}

			public string Render(IEnumerable<string> modifiers, string type, string name, string value)
			{
				return Render(new
				{
					Modificators = modifiers,
					Type = type,
					Name = name,
					Value = value
				});
			}

			public string Render(string modifiers, string type, string name, string value)
			{
				return Render(new[] { modifiers }, type, name, value);
			}
		}

		internal class FieldRenderableTemplate : RenderableTemplate
		{
			/// <inheritdoc />
			public FieldRenderableTemplate(string template)
				: base(template)
			{
			}

			public string Render(IEnumerable<string> modifiers, string type, string name)
			{
				return Render(new
				{
					Modificators = modifiers,
					Type = type,
					Name = name
				});
			}

			public string Render(string modifiers, string type, string name)
			{
				return Render(new[] { modifiers }, type, name);
			}
		}

		internal class FieldWithValueRenderableTemplate : RenderableTemplate
		{
			/// <inheritdoc />
			public FieldWithValueRenderableTemplate(string template)
				: base(template)
			{
			}

			public string Render(IEnumerable<string> modifiers, string type, string name, string value)
			{
				return Render(new
				{
					Modificators = modifiers,
					Type = type,
					Name = name,
					Value = value
				});
			}

			public string Render(string modifiers, string type, string name, string value)
			{
				return Render(new[] { modifiers }, type, name, value);
			}
		}
		

		#endregion

		private ConstRenderableTemplate _constRenderableTemplate;
		private FieldRenderableTemplate _fieldRenderableTemplate;
		private FieldWithValueRenderableTemplate _fieldWithValueRenderableTemplate;

		public IRenderableTemplate BackingField => GetRenderableTemplate();

		public IRenderableTemplate BackingFieldWithValue => GetRenderableTemplate();

		public IRenderableTemplate ReadOnlyField => GetRenderableTemplate();

		public ConstRenderableTemplate Const => GetConstRenderableTemplate();

		public FieldRenderableTemplate Field => GetFieldRenderableTemplate();

		public FieldWithValueRenderableTemplate FieldWithValue => GetFieldWithValueRenderableTemplate();

		public FieldSnippet()
			: base("Fields")
		{
		}

		private ConstRenderableTemplate GetConstRenderableTemplate()
		{
			return _constRenderableTemplate ?? (_constRenderableTemplate = new ConstRenderableTemplate(GetText(nameof(Const))));
		}

		private FieldRenderableTemplate GetFieldRenderableTemplate()
		{
			return _fieldRenderableTemplate ?? (_fieldRenderableTemplate = new FieldRenderableTemplate(GetText(nameof(Field))));
		}

		private FieldWithValueRenderableTemplate GetFieldWithValueRenderableTemplate()
		{
			return _fieldWithValueRenderableTemplate ?? (_fieldWithValueRenderableTemplate = new FieldWithValueRenderableTemplate(GetText(nameof(FieldWithValue))));
		}
	}
}
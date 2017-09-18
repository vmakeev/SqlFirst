using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;

namespace SqlFirst.Demo.Wpf
{
	public sealed class AvalonEditBehaviour : Behavior<TextEditor>
	{
		public string BindableText
		{
			get => (string)GetValue(BindableTextProperty);
			set => SetValue(BindableTextProperty, value);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			if (AssociatedObject != null)
			{
				AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
			}
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (AssociatedObject != null)
			{
				AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
			}
		}

		[SuppressMessage("ReSharper", "InconsistentNaming")]
		public static readonly DependencyProperty BindableTextProperty =
			DependencyProperty.Register("BindableText", typeof(string), typeof(AvalonEditBehaviour),
				new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

		private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
		{
			if (sender is TextEditor textEditor)
			{
				if (textEditor.Document != null)
				{
					BindableText = textEditor.Document.Text;
				}
			}
		}

		private static void PropertyChangedCallback(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var behavior = dependencyObject as AvalonEditBehaviour;
			TextEditor editor = behavior?.AssociatedObject;
			if (editor?.Document != null)
			{
				int caretOffset = editor.CaretOffset;
				editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue?.ToString() ?? string.Empty;
				editor.CaretOffset = Math.Min(caretOffset, editor.Document.Text?.Length ?? 0);
			}
		}
	}
}
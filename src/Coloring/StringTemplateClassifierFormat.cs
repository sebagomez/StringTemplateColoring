using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace StringTemplateColoring.Coloring
{
	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STKeyword)]
	[Name("StringTemplateKeywordFormat")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class KeywordFormat : ClassificationFormatDefinition
	{
		public KeywordFormat()
		{
			this.DisplayName = "This is a keyword"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STKeyword);
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction)]
	[Name("TemplateFunctionFormatFormat")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TemplateFunctionFormat : ClassificationFormatDefinition
	{
		public TemplateFunctionFormat()
		{
			this.DisplayName = "This is the header of a template"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction);
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen)]
	[Name("TemplateOpenFormat")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TemplateOpenFormat : ClassificationFormatDefinition
	{
		public TemplateOpenFormat()
		{
			this.DisplayName = "This is the <<"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen);
			this.BackgroundColor = Colors.Yellow;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2)]
	[Name("TemplateOpen2Format")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TemplateOpen2Format : ClassificationFormatDefinition
	{
		public TemplateOpen2Format()
		{
			this.DisplayName = "This is the <%"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2);
			this.BackgroundColor = Colors.Lime;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STVariable)]
	[Name("VariableFormat")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class VariableFormat : ClassificationFormatDefinition
	{
		public VariableFormat()
		{
			this.DisplayName = "This is a $variable$"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STVariable);
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STTemplateCall)]
	[Name("TemplateCallFormat")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TemplateCallFormat : ClassificationFormatDefinition
	{
		public TemplateCallFormat()
		{
			this.DisplayName = "This is a $template_call()$"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STTemplateCall);
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STComment)]
	[Name("CommentFormat")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class CommentFormat : ClassificationFormatDefinition
	{
		public CommentFormat()
		{
			this.DisplayName = "This is a comment"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STComment);
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = StringTemplateTokens.StringTemplateTokenHelper.STPlain)]
	[Name("PlainFormat")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class PlainFormat : ClassificationFormatDefinition
	{
		public PlainFormat()
		{
			this.DisplayName = "This is plain text, not template code"; //human readable version of the name
			this.ForegroundColor = StringTemplateColors.GetTokenColor(StringTemplateTokens.StringTemplateTokenHelper.STPlain);
		}
	}
}

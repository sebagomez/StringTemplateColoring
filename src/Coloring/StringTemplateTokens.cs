namespace StringTemplateColoring.Coloring
{
	internal class StringTemplateTokens
	{
		public enum StringTemplateTokenTypes
		{
			STKeyword, STTemplateFunction, STTemplateOpen, STTemplateOpen2, STVariable, STTemplateCall, STComment
		}

		public sealed class StringTemplateTokenHelper
		{
			public const string STKeyword = "STKeyword";
			public const string STTemplateFunction = "STTemplateFunction";
			public const string STTemplateOpen = "STTemplateOpen";
			public const string STTemplateOpen2 = "STTemplateOpen2";
			public const string STVariable = "STVariable";
			public const string STTemplateCall = "STTemplateCall";
			public const string STComment = "STComment";
		}
	}
}

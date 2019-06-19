using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace sebagomez.StringTemplateColoring
{
	internal static class StringTemplateClassifierClassificationDefinition
	{
		[Export(typeof(ClassificationTypeDefinition))]
		[Name(StringTemplateTokens.StringTemplateTokenHelper.STKeyword)]
		private static ClassificationTypeDefinition keyword;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name(StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction)]
		private static ClassificationTypeDefinition function;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen)]
		private static ClassificationTypeDefinition tplOpen;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2)]
		private static ClassificationTypeDefinition tplOpen2;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name(StringTemplateTokens.StringTemplateTokenHelper.STVariable)]
		private static ClassificationTypeDefinition variable;
	}
}

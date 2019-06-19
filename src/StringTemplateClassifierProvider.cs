using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace sebagomez.StringTemplateColoring
{
	[Export(typeof(IClassifierProvider))]
	[ContentType(StringHelper.STRING_TEMPLATE)]
	public class StringTemplateClassifierProvider : IClassifierProvider
	{
		[Export]
		[Name(StringHelper.STRING_TEMPLATE)]
		[BaseDefinition("code")]
		public static ContentTypeDefinition StringTemplateContentType { get; set; }

		[Export]
		[FileExtension(".st")]
		[ContentType(StringHelper.STRING_TEMPLATE)]
		public static FileExtensionToContentTypeDefinition STContentTypeDefinition { get; set; }

		[Export]
		[FileExtension(".stg")]
		[ContentType(StringHelper.STRING_TEMPLATE)]
		public static FileExtensionToContentTypeDefinition STGContentTypeDefinition { get; set; }

		// Disable "Field is never assigned to..." compiler's warning. Justification: the field is assigned by MEF.
#pragma warning disable 649

		/// <summary>
		/// Classification registry to be used for getting a reference
		/// to the custom classification type later.
		/// </summary>
		[Import]
		private IClassificationTypeRegistryService classificationRegistry { get; set; }

#pragma warning restore 649

		public IClassifier GetClassifier(ITextBuffer textBuffer)
		{
			return textBuffer.Properties.GetOrCreateSingletonProperty<StringTemplateClassifier>(creator: () => new StringTemplateClassifier(this.classificationRegistry));
		}
	}
}

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace StringTemplateColoring.Coloring
{
	internal class StringTemplateClassifier : IClassifier
	{
		private IClassificationType classificationType;
		IClassificationTypeRegistryService m_registry;

		internal StringTemplateClassifier(IClassificationTypeRegistryService registry)
		{
			m_registry = registry;
			this.classificationType = registry.GetClassificationType("StringTemplateClassifier");
		}

#pragma warning disable 67
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67

		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			return StringTemplateCodeHelper.GetTokens(span, m_registry);
		}
	}
}
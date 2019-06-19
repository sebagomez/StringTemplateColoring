using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace sebagomez.StringTemplateColoring
{
	internal class StringTemplateCodeHelper
	{
		//https://medium.com/factory-mind/regex-tutorial-a-simple-cheatsheet-by-examples-649dc1c3f285
		const string HEADER_REG = @".*::=";
		const string SUB_TPL_OPEN = @"\u003C\u003C"; //<<
		const string SUB_TPL_CLOSE = @"\u003E\u003E"; //>>
		const string SUB_TPL_OPEN2 = @"\u003C\u0025"; //<%
		const string SUB_TPL_CLOSE2 = @"\u0025\u003E"; //%>
		const string VARIABLE_REG = @"\$[a-zA-Z\.]+\$";
		const string TEMPLATE_CALL = @"\$.*\(.*\)\$";

		static readonly string[] KEYWORDS = new string[] { "group", "delimiters" };

		static object lockObject = new object();

		public static IList<ClassificationSpan> GetTokens(SnapshotSpan snapSpan, IClassificationTypeRegistryService registry)
		{
			lock (lockObject)
			{
				List<ClassificationSpan> result = new List<ClassificationSpan>();
				var lines = snapSpan.Snapshot.Lines.Where(l => l.Start >= snapSpan.Start && l.End <= snapSpan.End);
				//var lines = snapSpan.Snapshot.Lines;
				bool inCommentBlock = false;

				foreach (var line in lines)
				{
					SnapshotSpan span = line.Extent;

					int curLoc = line.Start.Position;
					string formattedLine = line.GetText();
					

					Regex reg = new Regex(HEADER_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						int index = formattedLine.IndexOf("(");
						if (index > 0)
						{
							result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + curLoc), index), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction)));
							result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, formattedLine.IndexOf("::=") + curLoc), 3), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction)));
						}
						else
							result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + curLoc), match.Length), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction)));

					}

					reg = new Regex(SUB_TPL_OPEN);
					foreach (Match match in reg.Matches(formattedLine))
					{
						result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + curLoc), match.Length), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen)));
					}

					reg = new Regex(SUB_TPL_CLOSE);
					foreach (Match match in reg.Matches(formattedLine))
					{
						result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + curLoc), match.Length), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen)));
					}

					reg = new Regex(SUB_TPL_OPEN2);
					foreach (Match match in reg.Matches(formattedLine))
					{
						result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + curLoc), match.Length), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2)));
					}

					reg = new Regex(SUB_TPL_CLOSE2);
					foreach (Match match in reg.Matches(formattedLine))
					{
						result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + curLoc), match.Length), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2)));
					}

					reg = new Regex(TEMPLATE_CALL);
					foreach (Match match in reg.Matches(formattedLine))
					{
						result.Add(new ClassificationSpan(new SnapshotSpan(new SnapshotPoint(span.Snapshot, match.Index + curLoc), match.Length), registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STVariable)));
					}

					foreach (string keyword in KEYWORDS)
					{
						reg = new Regex($@"\b{keyword}");
						foreach (Match match in reg.Matches(formattedLine))
						{
							AddClassificatioType(result, span, match, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
						}
					}

					if (!inCommentBlock && result.Count > 0)
						return result;
				}

				return result;
			}

		}

		static void AddClassificatioType(List<ClassificationSpan> result, SnapshotSpan span, Match match, IClassificationType classificationType)
		{
			int start = match == null ? 0 : match.Index;
			int length = match == null ? span.Length : match.Length;
			SnapshotSpan regSpan = new SnapshotSpan(span.Snapshot, span.Start.Position + start, length);
			result.Add(new ClassificationSpan(regSpan, classificationType));
		}

	}
}

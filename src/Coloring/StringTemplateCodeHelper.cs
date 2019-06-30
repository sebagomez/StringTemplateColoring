using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace StringTemplateColoring.Coloring
{
	internal class StringTemplateCodeHelper
	{
		//https://medium.com/factory-mind/regex-tutorial-a-simple-cheatsheet-by-examples-649dc1c3f285
		const string HEADER_REG = @"(\w*\()(.*)(\)::=)";
		const string SUB_TPL_MULTILINE = @".*(\u003C\u003C|\u003E\u003E).*"; //<< | >>
		const string SUB_TPL_IGNORE_NL = @".*(\u003C\u0025).*(\u0025\u003E).*"; //<% | %>
		const string TEMPLATE_REG = @"\$[^\$:]*\$"; //$template$
		const string ITERATOR_REG = @"(\$.*)(\:\{)+(.*)\|(.*)(\};)(.*\$)"; // 
		const string COMMENT_REG = @"\$!(.*)!\$"; // comment
		const string KEYWORD_REG = @"\b(group|delimiters|first|last|rest|trunc|strip|length|default|implements|optional|interface|super)\b";
		const string STATMENT_REG = @"\b(if|else|elseif|endif)\b";

		static readonly string[] FLOW_STATEMENT = new string[] { "if", "else", "elseif", "endif" };

		static object lockObject = new object();

		public static IList<ClassificationSpan> GetTokens(SnapshotSpan snapSpan, IClassificationTypeRegistryService registry)
		{
			lock (lockObject)
			{
				List<ClassificationSpan> result = new List<ClassificationSpan>();
				var lines = snapSpan.Snapshot.Lines.Where(l => l.Start >= snapSpan.Start && l.End <= snapSpan.End);
				//var lines = snapSpan.Snapshot.Lines;
				//bool inCommentBlock = false;

				foreach (var line in lines)
				{
					SnapshotSpan span = line.Extent;

					//int curLoc = line.Start.Position;
					string formattedLine = line.GetText();

					Regex reg = new Regex(".*");
					AddClassificatioType(result, span, 0, formattedLine.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STPlain));


					reg = new Regex(HEADER_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						for (int i = 0; i < match.Groups.Count; i++)
						{
							Group g = match.Groups[i];
							switch (i)
							{
								case 1:
								case 3:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction));
									break;
								case 2:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STVariable));
									break;
								default:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STPlain));
									break;
							}
						}
					}

					reg = new Regex(SUB_TPL_MULTILINE);
					foreach (Match match in reg.Matches(formattedLine))
					{
						AddGroupClassificationType(result, span, match, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen));
						//AddClassificatioType(result, span, match.Index, match.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen));
					}

					reg = new Regex(SUB_TPL_IGNORE_NL);
					foreach (Match match in reg.Matches(formattedLine))
					{
						AddGroupClassificationType(result, span, match, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2));
						//AddClassificatioType(result, span, match.Index, match.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2));
					}

					reg = new Regex(TEMPLATE_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						reg = new Regex(STATMENT_REG);
						foreach (Match match1 in reg.Matches(match.Value))
						{
							AddGroupClassificationType(result, span, match1, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
						}
						AddClassificatioType(result, span, match.Index, match.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STVariable));
					}

					reg = new Regex(ITERATOR_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						for (int i = 0; i < match.Groups.Count; i++)
						{
							Group g = match.Groups[i];
							switch (i)
							{
								case 0:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STPlain));
									break;
								case 2:
								case 5:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
									break;
								case 1:
								case 3:
								case 6:
								case 7:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STVariable));
									break;
								case 4:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STPlain));
									break;

								default:
									break;
							}
						}
					}

					reg = new Regex(KEYWORD_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						AddGroupClassificationType(result, span, match, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
					}

					reg = new Regex(COMMENT_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						AddClassificatioType(result, span, match.Index, match.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STComment));
					}
				}

				return result;
			}

		}

		private static void AddGroupClassificationType(List<ClassificationSpan> result, SnapshotSpan span, Match match, IClassificationType classificationType)
		{
			for (int i = 0; i < match.Groups.Count; i++)
			{
				Group g = match.Groups[i];
				switch (i)
				{
					case 0:
						break;
					default:
						AddClassificatioType(result, span, g.Index, g.Length, classificationType);
						break;
				}
			}
		}

		static void AddClassificatioType(List<ClassificationSpan> result, SnapshotSpan span, int start, int length, IClassificationType classificationType)
		{
			SnapshotSpan regSpan = new SnapshotSpan(span.Snapshot, span.Start.Position + start, length);
			result.Add(new ClassificationSpan(regSpan, classificationType));
		}

	}
}

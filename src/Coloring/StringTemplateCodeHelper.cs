using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace StringTemplateColoring.Coloring
{
	internal class StringTemplateCodeHelper
	{
		//https://medium.com/factory-mind/regex-tutorial-a-simple-cheatsheet-by-examples-649dc1c3f285
		const string HEADER_REG = @"(\w*\()(.*)(\)::=)";
		const string SUB_TPL_OPEN = @"\u003C\u003C"; //<<
		const string SUB_TPL_CLOSE = @"\u003E\u003E"; //>>
		const string SUB_TPL_OPEN2 = @"\u003C\u0025"; //<%
		const string SUB_TPL_CLOSE2 = @"\u0025\u003E"; //%>
		const string VARIABLE_REG = @"\$[^\$:]*\$"; //$template$
		const string ITERATOR_REG = @"(\$.*)(\:\{)+(.*)\|(.*)(\};)(.*\$)"; // 
		const string COMMENT_REG = @"\$!(.*)!\$"; // comment
		const string KEYWORD_REG = @"(group|delimiters|first|last|rest|trunc|strip|length|default|implements|optional|interface|super)";
		const string IF_STATMENT_REG = @"\$.*(if).*\(.*\).*\$";
		const string ELSEIF_STATMENT_REG = @"\$.*(elseif).*\(.*\).*\$";
		const string ENDIF_STATMENT_REG = @"\$.*(endif).*\$";
		const string ELSE_STATMENT_REG = @"\$.*(else).*\$";

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

					int curLoc = line.Start.Position;
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

					reg = new Regex(VARIABLE_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
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
						for (int i = 0; i < match.Groups.Count; i++)
						{
							Group g = match.Groups[i];
							switch (i)
							{
								case 0:
									break;
								default:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
									break;
							}
						}

					}

					reg = new Regex(IF_STATMENT_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						for (int i = 0; i < match.Groups.Count; i++)
						{
							Group g = match.Groups[i];
							switch (i)
							{
								case 0:
									break;
								default:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
									break;
							}
						}
					}

					reg = new Regex(ELSE_STATMENT_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						for (int i = 0; i < match.Groups.Count; i++)
						{
							Group g = match.Groups[i];
							switch (i)
							{
								case 0:
									break;
								default:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
									break;
							}
						}
					}

					reg = new Regex(ELSEIF_STATMENT_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						for (int i = 0; i < match.Groups.Count; i++)
						{
							Group g = match.Groups[i];
							switch (i)
							{
								case 0:
									break;
								default:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
									break;
							}
						}
					}

					reg = new Regex(ENDIF_STATMENT_REG);
					foreach (Match match in reg.Matches(formattedLine))
					{
						for (int i = 0; i < match.Groups.Count; i++)
						{
							Group g = match.Groups[i];
							switch (i)
							{
								case 0:
									break;
								default:
									AddClassificatioType(result, span, g.Index, g.Length, registry.GetClassificationType(StringTemplateTokens.StringTemplateTokenHelper.STKeyword));
									break;
							}
						}
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

		static void AddClassificatioType(List<ClassificationSpan> result, SnapshotSpan span, int start, int length, IClassificationType classificationType)
		{
			SnapshotSpan regSpan = new SnapshotSpan(span.Snapshot, span.Start.Position + start, length);
			result.Add(new ClassificationSpan(regSpan, classificationType));
		}

	}
}

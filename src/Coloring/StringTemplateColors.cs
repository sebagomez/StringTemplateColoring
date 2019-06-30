using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;

namespace StringTemplateColoring.Coloring
{
	internal class StringTemplateColors
	{
		public enum Theme
		{
			Dark,
			Light,
			Blue,
			ANY
		}

		static StringTemplateColors()
		{
			CalculateTheme();
			VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
		}

		static void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
		{
			CalculateTheme();
		}

		static void CalculateTheme()
		{
			System.Drawing.Color backgroundColor = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
			switch (backgroundColor.Name)
			{
				case "ff252526":
					s_theme = Theme.Dark;
					break;
				case "ffffffff":
					s_theme = Theme.Blue;
					break;
				case "fff5f5f5":
					s_theme = Theme.Light;
					break;
				default:
					s_theme = Theme.ANY;
					break;
			}
		}

		static Theme s_theme;
		public static Theme CurrentTheme
		{
			get { return s_theme; }
		}

		#region Colors
		//https://docs.microsoft.com/en-us/visualstudio/extensibility/ux-guidelines/color-value-reference-for-visual-studio?view=vs-2019
		//https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.colors?view=netframework-4.8
		static Dictionary<string, Dictionary<Theme, Color>> s_colors = new Dictionary<string, Dictionary<Theme, Color>>
		{
			{ StringTemplateTokens.StringTemplateTokenHelper.STTemplateFunction, new Dictionary<Theme, Color>
				{
					{ Theme.Dark, Colors.LightSeaGreen },
					{ Theme.ANY, Colors.Purple }
				}
			},
			{ StringTemplateTokens.StringTemplateTokenHelper.STKeyword, new Dictionary<Theme, Color>
				{
					{ Theme.Dark, Colors.DodgerBlue },
					{ Theme.ANY, Colors.Blue }
				}
			},
			{ StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen, new Dictionary<Theme, Color>
				{
					{ Theme.ANY, Colors.Black }
				}
			},
			{ StringTemplateTokens.StringTemplateTokenHelper.STTemplateOpen2, new Dictionary<Theme, Color>
				{
					{ Theme.ANY, Colors.Black }
				}
			},
			{ StringTemplateTokens.StringTemplateTokenHelper.STVariable, new Dictionary<Theme, Color>
				{
					{ Theme.ANY, Colors.Plum }
				}
			},
			{ StringTemplateTokens.StringTemplateTokenHelper.STTemplateCall, new Dictionary<Theme, Color>
				{
					{ Theme.ANY, Colors.NavajoWhite }
				}
			},
			{ StringTemplateTokens.StringTemplateTokenHelper.STComment, new Dictionary<Theme, Color>
				{
					{ Theme.ANY, Colors.DarkGreen }
				}
			},
			{ StringTemplateTokens.StringTemplateTokenHelper.STPlain, new Dictionary<Theme, Color>
				{
					{ Theme.Dark, Colors.Silver },
					{ Theme.ANY, Colors.DimGray }
				}
			}
		};

		public static System.Windows.Media.Color GetTokenColor(string token)
		{
			if (s_colors[token].ContainsKey(CurrentTheme))
				return s_colors[token][CurrentTheme];

			return s_colors[token][Theme.ANY];
		}

		#endregion
	}
}

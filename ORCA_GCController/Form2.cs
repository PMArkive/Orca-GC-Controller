using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sgry.Azuki;
using Sgry.Azuki.Highlighter;


namespace GCController
{
    public partial class Form2 : Form
    {
        private static readonly Color ColorScheme_Back = Color.DarkSlateGray;
        public Form2()
        {
            InitializeComponent();
            azukiControl1.ColorScheme = new ColorScheme()
            {
                BackColor = ColorScheme_Back
            };
            azukiControl1.ColorScheme.SetColor(CharClass.Normal, Color.White, ColorScheme_Back);
            azukiControl1.ColorScheme.SetColor(CharClass.Number, Color.GreenYellow, ColorScheme_Back);

            // 
            var keywordHighlighter = new KeywordHighlighter();
            keywordHighlighter.AddKeywordSet(new string[]
            {
                "Hit", "Press", "Start", "Wait"
            }, CharClass.Keyword);

            keywordHighlighter.AddRegex("-[a-z]", CharClass.Keyword2);
            keywordHighlighter.AddLineHighlight("#", CharClass.DocComment);

            azukiControl1.Highlighter = keywordHighlighter;


        }
    }
}

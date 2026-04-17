using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace FTDPClient.Style
{
    // This class defines the gradient colors for 
    // the MenuStrip and the ToolStrip.
    class CustomProfessionalColors : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin
        { get { return ColorTranslator.FromHtml("#FAF9F5"); } }

        public override Color ToolStripGradientMiddle
        { get { return ColorTranslator.FromHtml("#ECE7E0"); } }

        public override Color ToolStripGradientEnd
        { get { return ColorTranslator.FromHtml("#C0C0A8"); } }
        public override Color ToolStripBorder
            { get { return ColorTranslator.FromHtml("#A3A37C"); } }
        public override Color ButtonSelectedHighlight
        {
            get
            {
                return base.ButtonPressedHighlight;
            }
        }
        //public override Color MenuStripGradientBegin
        //{ get {
        //    return Color.FromArgb(255,236,233,216);
        //} }

        //public override Color MenuStripGradientEnd
        //{ get { return Color.FromArgb(255, 251, 250, 247); } }
    }
    // This type demonstrates a custom renderer. It overrides the
    // OnRenderMenuItemBackground and OnRenderButtonBackground methods
    // to customize the backgrounds of MenuStrip items and ToolStrip buttons.
    class CustomProfessionalRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                using (Brush b = new SolidBrush(ProfessionalColors.SeparatorLight))
                {
                    e.Graphics.FillEllipse(b, e.Item.ContentRectangle);
                }
            }
            else
            {
                using (Pen p = new Pen(ProfessionalColors.SeparatorLight))
                {
                    e.Graphics.DrawEllipse(p, e.Item.ContentRectangle);
                }
            }
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle r = Rectangle.Inflate(e.Item.ContentRectangle, -2, -2);

            if (e.Item.Selected)
            {
                using (Brush b = new SolidBrush(ProfessionalColors.SeparatorLight))
                {
                    e.Graphics.FillRectangle(b, r);
                }
            }
            else
            {
                using (Pen p = new Pen(ProfessionalColors.SeparatorLight))
                {
                    e.Graphics.DrawRectangle(p, r);
                }
            }
        }
    }

}

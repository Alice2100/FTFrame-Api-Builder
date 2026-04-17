//样式编辑适配类，Write by maobb.2005-7-10
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using mshtml;
using FTDPClient.classes;
using FTDPClient.consts;
using FTDPClient.Page;
using FTDPClient.functions;

namespace FTDPClient.StyleEditorAdapter
{
    /// <summary>
    /// StyleEditorAdapter 的摘要说明。
    /// </summary>
    /// 
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class StyleEditorAdapter
    {
        public static string NotSet = res._pageware.GetString("NotSet");
        public StyleEditorAdapter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static void setPartStyle(IHTMLElement e, string ControlPartID, string PartStyleName, IHTMLElement PartElement)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            if (e.tagName.Equals("dscomstyle"))
            {
                PageWare.updatePartStyle(ControlPartID, PartStyleName, e.style.cssText);
                if (PartElement != null)
                {
                    try
                    {
                        PartElement.innerHTML = PageWare.getPartHtml(ControlPartID);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.StartsWith("HRESULT"))
                        {
                            //Ele.parentElement.tagName="div";
                            //Ele.innerHTML=getPartHtml("ctlid",CtlName,PartName,curPartSetXmlDom.OuterXml);
                            //if parent element is <p> <td>...,span will accur error
                            string Eleid = PartElement.getAttribute("idname", 0).ToString();
                            string Eleheight = PartElement.style.height.ToString();
                            string Elewidth = PartElement.style.width.ToString();
                            string Elename = PartElement.getAttribute("partname", 0).ToString();
                            int ElesourceIndex = PartElement.sourceIndex;
                            PartElement.innerHTML = "";
                            PartElement.style.cssText = "";
                            IHTMLElement itp = PartElement.parentElement;
                            itp.outerHTML = itp.outerHTML.Replace(PartElement.outerHTML, globalConst.PageWare.getControlEditHead(Eleid, Elename, Elewidth, Eleheight) + PageWare.getPartHtml(ControlPartID) + globalConst.PageWare.getControlEditTail());
                            PartElement = (IHTMLElement)(((IHTMLDocument2)form.getEditor().editocx.DOM).all.item(ElesourceIndex, ElesourceIndex));
                            if (!PageWare.isPartElement(PartElement)) MsgBox.Warning("Part Element Lost!");
                            //new MsgBox(Ele.outerHTML);
                            //					//new MsgBox(form.getEditor().editocx.getCurElement().outerHTML);
                            //Ele=getPartElement(form.getEditor().editocx.getCurElement());
                            //new MsgBox(Ele.outerHTML);
                            //IHTMLDocument2 doc2=(IHTMLDocument2)itp.document;
                            //new MsgBox(doc2.all.length.ToString());

                            //					foreach(IHTMLElement ie in (IHTMLElementCollection)itp.all)
                            //					{
                            //						if(PageWare.isPartElement(ie))
                            //						{
                            //							if(ie.getAttribute("idname",0).ToString().Equals(Eleid))
                            //							{
                            //								Ele=ie;
                            //							}
                            //						}
                            //					}

                        }

                    }
                }
            }
        }
        public class FontEditAdapter
        {
            private IHTMLElement e;
            private string CID;
            private string PName;
            private IHTMLElement PE;
            public FontEditAdapter(IHTMLElement ele, string ControlPartID, string PartStyleName, IHTMLElement PartElement)
            {
                e = ele;
                CID = ControlPartID;
                PName = PartStyleName;
                PE = PartElement;
            }
            [DescriptionAttribute("Sets or retrieves the color of the text of the object."),
            CategoryAttribute("Font Color")]
            public Color color
            {
                get
                {

                    if (e.style.color == null) return Color.Empty;
                    return ColorTranslator.FromHtml(e.style.color.ToString());

                }
                set
                {
                    e.style.color = ColorTranslator.ToHtml(value);
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(fontFamilyConverter)),
           DescriptionAttribute("Sets or retrieves the name of the font used for text in the object."),
            CategoryAttribute("Font Setting")]
            public string fontFamily
            {
                get
                {
                    if (e.style.fontFamily == null) return NotSet;
                    return e.style.fontFamily;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.fontFamily = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves a value that indicates the font size used for text in the object."),
            CategoryAttribute("Font Setting")]
            public string fontSize
            {
                get
                {
                    if (e.style.fontSize == null) return "";
                    return e.style.fontSize.ToString();
                }
                set
                {
                    e.style.fontSize = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(fontStyleConverter)),
           DescriptionAttribute("Sets or retrieves the font style of the object as italic, normal, or oblique."),
            CategoryAttribute("Font Setting")]
            public string fontStyle
            {
                get
                {
                    if (e.style.fontStyle == null) return NotSet;
                    return e.style.fontStyle;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.fontStyle = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(fontVariantConverter)),
           DescriptionAttribute("Sets or retrieves whether the text of the object is in small capital letters."),
            CategoryAttribute("Font Setting")]
            public string fontVariant
            {
                get
                {
                    if (e.style.fontVariant == null) return NotSet;
                    return e.style.fontVariant;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.fontVariant = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(fontWeightConverter)),
           DescriptionAttribute("Sets or retrieves the weight of the font of the object."),
            CategoryAttribute("Font Setting")]
            public string fontWeight
            {
                get
                {
                    if (e.style.fontWeight == null) return NotSet;
                    return e.style.fontWeight;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.fontWeight = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }


            [DescriptionAttribute("Sets or retrieves whether the text in the object is underlined."),
            CategoryAttribute("Font Text")]
            public bool textUnderline
            {
                get
                {
                    return e.style.textDecorationUnderline;
                }
                set
                {
                    e.style.textDecorationUnderline = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves a Boolean value indicating whether the text in the object has a line drawn through it."),
            CategoryAttribute("Font Text")]
            public bool textLineThrough
            {
                get
                {
                    return e.style.textDecorationLineThrough;
                }
                set
                {
                    e.style.textDecorationLineThrough = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves a Boolean value indicating whether the text in the object has a line drawn over it."),
            CategoryAttribute("Font Text")]
            public bool textOverline
            {
                get
                {
                    return e.style.textDecorationOverline;
                }
                set
                {
                    e.style.textDecorationOverline = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of additional space between letters in the object."),
            CategoryAttribute("Font Spacing")]
            public string letterSpacing
            {
                get
                {
                    if (e.style.letterSpacing == null) return null;
                    return e.style.letterSpacing.ToString();
                }
                set
                {
                    e.style.letterSpacing = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of additional space between words in the object."),
            CategoryAttribute("Font Spacing")]
            public string wordSpacing
            {
                get
                {
                    if (e.style.wordSpacing == null) return null;
                    return e.style.wordSpacing.ToString();
                }
                set
                {
                    e.style.wordSpacing = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(whiteSpaceConverter)),
           DescriptionAttribute("Sets or retrieves whether the text in the object is underlined."),
            CategoryAttribute("Font Spacing")]
            public string whiteSpace
            {
                get
                {
                    if (e.style.whiteSpace == null) return NotSet;
                    return e.style.whiteSpace;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.whiteSpace = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the distance between lines in the object."),
            CategoryAttribute("Font Spacing")]
            public string lineHeight
            {
                get
                {
                    if (e.style.lineHeight == null) return null;
                    return e.style.lineHeight.ToString();
                }
                set
                {
                    e.style.lineHeight = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
        }
        public class textAlignConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { "(NotSet)", "left", "right", "center", "justify" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class BackGroundEditAdapter
        {
            private IHTMLElement e;
            private string CID;
            private string PName;
            private IHTMLElement PE;
            public BackGroundEditAdapter(IHTMLElement ele, string ControlPartID, string PartStyleName, IHTMLElement PartElement)
            {
                e = ele;
                CID = ControlPartID;
                PName = PartStyleName;
                PE = PartElement;
            }
            [DescriptionAttribute("Sets or retrieves the color behind the content of the object."),
            CategoryAttribute("(Default)")]
            public Color backgoundColor
            {
                get
                {

                    if (e.style.backgroundColor == null) return Color.Empty;
                    return ColorTranslator.FromHtml(e.style.backgroundColor.ToString());

                }
                set
                {
                    e.style.backgroundColor = ColorTranslator.ToHtml(value);
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [EditorAttribute(typeof(SourceEditor), typeof(UITypeEditor)),
           TypeConverterAttribute(typeof(SourceEditorConverter)),
                DescriptionAttribute("Sets or retrieves the background image of the object."),
            CategoryAttribute("(Default)")]
            public string backgoundImage_file
            {
                get
                {

                    if (e.style.backgroundImage == null) return "";
                    if (e.style.backgroundImage.StartsWith("url("))
                    {
                        return e.style.backgroundImage.Substring(4, e.style.backgroundImage.Length - 5);
                    }
                    return e.style.backgroundImage;

                }
                set
                {
                    if (value.Equals(""))
                    {
                        e.style.backgroundImage = null;
                    }
                    else
                    {
                        e.style.backgroundImage = "url(" + value + ")";
                    }
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the background image of the object."),
            CategoryAttribute("(Default)")]
            public string backgoundImage_url
            {
                get
                {

                    if (e.style.backgroundImage == null) return "";
                    if (e.style.backgroundImage.StartsWith("url("))
                    {
                        return e.style.backgroundImage.Substring(4, e.style.backgroundImage.Length - 5);
                    }
                    return e.style.backgroundImage;

                }
                set
                {
                    if (value.Equals(""))
                    {
                        e.style.backgroundImage = null;
                    }
                    else
                    {
                        e.style.backgroundImage = "url(" + value + ")";
                    }
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(backgroundRepeatConverter)),
           DescriptionAttribute("Sets or retrieves how the backgroundImage property of the object is tiled."),
            CategoryAttribute("行为")]
            public string backgroundRepeat
            {
                get
                {
                    if (e.style.backgroundRepeat == null) return NotSet;
                    return e.style.backgroundRepeat;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.backgroundRepeat = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(backgroundAttachmentConverter)),
           DescriptionAttribute("Sets or retrieves how the background image is attached to the object within the document."),
            CategoryAttribute("行为")]
            public string backgroundAttachment
            {
                get
                {
                    if (e.style.backgroundAttachment == null) return NotSet;
                    return e.style.backgroundAttachment;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.backgroundAttachment = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(backgroundPositionXConverter)),
           DescriptionAttribute("Sets or retrieves the x-coordinate of the backgroundPosition property."),
            CategoryAttribute("行为")]
            public string backgroundPositionX
            {
                get
                {
                    if (e.style.backgroundPositionX == null) return NotSet;
                    return e.style.backgroundPositionX.ToString();

                }
                set
                {
                    if (NotSet.Equals(value)) value = "";
                    e.style.backgroundPositionX = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(backgroundPositionYConverter)),
           DescriptionAttribute("Sets or retrieves the y-coordinate of the backgroundPosition property."),
            CategoryAttribute("行为")]
            public string backgroundPositionY
            {
                get
                {
                    if (e.style.backgroundPositionY == null) return NotSet;
                    return e.style.backgroundPositionY.ToString();

                }
                set
                {
                    if (NotSet.Equals(value)) value = "";
                    e.style.backgroundPositionY = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
        }

        public class backgroundRepeatConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"repeat", 
																	"repeat-x", 
																	"repeat-y",
																	"no-repeat"});
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class fontFamilyConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                string[] ffname = new string[globalConst.SysFonts.Families.Length + 1];
                ffname.SetValue(NotSet, 0);
                int i = 1;
                foreach (FontFamily ff in globalConst.SysFonts.Families)
                {
                    ffname.SetValue(ff.Name, i);
                    i++;
                }
                return new StandardValuesCollection(ffname);
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class fontStyleConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "normal", "italic", "oblique" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class whiteSpaceConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "normal", "nowrap", "pre" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class styleFloatConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "none", "left", "right" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class visibilityConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "inherit", "visible", "hidden" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class verticalAlignConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"baseline","auto","sub"
			,"super","top","middle","bottom","text-top","text-bottom"});
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class overflowConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "visible", "scroll", "hidden", "auto" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class displayConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"inline","none","block"
,"inline-block","list-item","table-header-group","table-footer-group"});
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class cursorConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"auto","all-scroll","col-resize","crosshair","default","hand","help","move","no-drop","not-allowed","pointer","progress","row-resize","text","url(uri)","vertical-text","wait",
																	"E-resize","S-resize","W-resize","N-resize","NE-resize","NW-resize","SE-resize","SW-resize"});
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return false;
            }
        }
        public class fontVariantConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "normal", "small-caps" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class borderStyleConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "none", "dotted", "dashed", "solid", "double", "groove", "ridge", "inset", "window-inset", "outset" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class fontWeightConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "normal", "bold", "bolder", "lighter", "100", "200", "300", "400", "500", "600", "700", "800" });
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class backgroundAttachmentConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"scroll", 
																	"fixed"});
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class backgroundPositionXConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"left", 
																	"center","right"});
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class backgroundPositionYConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"top", 
																	"center","bottom"});
            }
            public override bool GetStandardValuesSupported(
                ITypeDescriptorContext context)
            {
                return true;
            }
            public override bool GetStandardValuesExclusive(
                ITypeDescriptorContext context)
            {
                return true;
            }
        }
        public class BorderEditAdapter
        {
            private IHTMLElement e;
            private string CID;
            private string PName;
            private IHTMLElement PE;
            public BorderEditAdapter(IHTMLElement ele, string ControlPartID, string PartStyleName, IHTMLElement PartElement)
            {
                e = ele;
                CID = ControlPartID;
                PName = PartStyleName;
                PE = PartElement;
            }
            [DescriptionAttribute("Sets or retrieves the width of the left, right, top, and bottom borders of the object."),
            CategoryAttribute("Border All")]
            public string borderWidth
            {
                get
                {
                    if (e.style.borderWidth == null) return "";
                    return e.style.borderWidth.ToString();
                }
                set
                {
                    e.style.borderWidth = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the border color of the object."),
            CategoryAttribute("Border All")]
            public Color borderColor
            {
                get
                {

                    if (e.style.borderColor == null) return Color.Empty;
                    if (e.style.borderColor.ToString().IndexOf(" ") > 0) return Color.Empty;
                    return ColorTranslator.FromHtml(e.style.borderColor.ToString());

                }
                set
                {
                    e.style.borderColor = ColorTranslator.ToHtml(value);
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(borderStyleConverter)),
           DescriptionAttribute("Sets or retrieves the style of the left, right, top, and bottom borders of the object."),
            CategoryAttribute("Border All")]
            public string borderStyle
            {
                get
                {
                    if (e.style.borderStyle == null) return NotSet;
                    return e.style.borderStyle;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.borderStyle = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the left border of the object."),
            CategoryAttribute("Border Left")]
            public string borderLeftWidth
            {
                get
                {
                    if (e.style.borderLeftWidth == null) return "";
                    return e.style.borderLeftWidth.ToString();
                }
                set
                {
                    e.style.borderLeftWidth = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color of the left border of the object."),
            CategoryAttribute("Border Left")]
            public Color borderLeftColor
            {
                get
                {

                    if (e.style.borderLeftColor == null) return Color.Empty;
                    return ColorTranslator.FromHtml(e.style.borderLeftColor.ToString());

                }
                set
                {
                    e.style.borderLeftColor = ColorTranslator.ToHtml(value);
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(borderStyleConverter)),
           DescriptionAttribute("Sets or retrieves the style of the left border of the object."),
            CategoryAttribute("Border Left")]
            public string borderLeftStyle
            {
                get
                {
                    if (e.style.borderLeftStyle == null) return NotSet;
                    return e.style.borderLeftStyle;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.borderLeftStyle = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the right border of the object."),
            CategoryAttribute("Border Right")]
            public string borderRightWidth
            {
                get
                {
                    if (e.style.borderRightWidth == null) return "";
                    return e.style.borderRightWidth.ToString();
                }
                set
                {
                    e.style.borderRightWidth = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color of the right border of the object."),
            CategoryAttribute("Border Right")]
            public Color borderRightColor
            {
                get
                {

                    if (e.style.borderRightColor == null) return Color.Empty;
                    return ColorTranslator.FromHtml(e.style.borderRightColor.ToString());

                }
                set
                {
                    e.style.borderRightColor = ColorTranslator.ToHtml(value);
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(borderStyleConverter)),
           DescriptionAttribute("Sets or retrieves the style of the right border of the object."),
            CategoryAttribute("Border Right")]
            public string borderRightStyle
            {
                get
                {
                    if (e.style.borderRightStyle == null) return NotSet;
                    return e.style.borderRightStyle;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.borderRightStyle = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the top border of the object."),
            CategoryAttribute("Border Top")]
            public string borderTopWidth
            {
                get
                {
                    if (e.style.borderTopWidth == null) return "";
                    return e.style.borderTopWidth.ToString();
                }
                set
                {
                    e.style.borderTopWidth = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color of the top border of the object."),
            CategoryAttribute("Border Top")]
            public Color borderTopColor
            {
                get
                {

                    if (e.style.borderTopColor == null) return Color.Empty;
                    return ColorTranslator.FromHtml(e.style.borderTopColor.ToString());

                }
                set
                {
                    e.style.borderTopColor = ColorTranslator.ToHtml(value);
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(borderStyleConverter)),
           DescriptionAttribute("Sets or retrieves the style of the top border of the object."),
            CategoryAttribute("Border Top")]
            public string borderTopStyle
            {
                get
                {
                    if (e.style.borderTopStyle == null) return NotSet;
                    return e.style.borderTopStyle;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.borderTopStyle = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the bottom border of the object."),
            CategoryAttribute("Border Bottom")]
            public string borderBottomWidth
            {
                get
                {
                    if (e.style.borderBottomWidth == null) return "";
                    return e.style.borderBottomWidth.ToString();
                }
                set
                {
                    e.style.borderBottomWidth = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color of the bottom border of the object."),
            CategoryAttribute("Border Bottom")]
            public Color borderBottomColor
            {
                get
                {

                    if (e.style.borderBottomColor == null) return Color.Empty;
                    return ColorTranslator.FromHtml(e.style.borderBottomColor.ToString());

                }
                set
                {
                    e.style.borderBottomColor = ColorTranslator.ToHtml(value);
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(borderStyleConverter)),
           DescriptionAttribute("Sets or retrieves the style of the bottom border of the object."),
            CategoryAttribute("Border Bottom")]
            public string borderBottomStyle
            {
                get
                {
                    if (e.style.borderBottomStyle == null) return NotSet;
                    return e.style.borderBottomStyle;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.borderBottomStyle = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
        }

        public class PadingEditAdapter
        {
            private IHTMLElement e;
            private string CID;
            private string PName;
            private IHTMLElement PE;
            public PadingEditAdapter(IHTMLElement ele, string ControlPartID, string PartStyleName, IHTMLElement PartElement)
            {
                e = ele;
                CID = ControlPartID;
                PName = PartStyleName;
                PE = PartElement;
            }
            [TypeConverter(typeof(textAlignConverter)),
           DescriptionAttribute("Sets or retrieves whether the text in the object is left-aligned, right-aligned, centered, or justified."),
            CategoryAttribute("Align")]
            public string textAlign
            {
                get
                {
                    if (e.style.textAlign == null) return NotSet;
                    return e.style.textAlign;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.textAlign = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(verticalAlignConverter)),
           DescriptionAttribute("Sets or retrieves the vertical alignment of the object."),
            CategoryAttribute("Align")]
            public string @verticalAlign
            {
                get
                {
                    if (e.style.verticalAlign == null) return NotSet;
                    return e.style.verticalAlign.ToString();
                }
                set
                {
                    if (NotSet.Equals(value)) value = "";
                    e.style.verticalAlign = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object."),
            CategoryAttribute("Size")]
            public string @width
            {
                get
                {
                    if (e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Size")]
            public string @height
            {
                get
                {
                    if (e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the position of the object relative to the top of the next positioned object in the document hierarchy."),
            CategoryAttribute("Size")]
            public string @top
            {
                get
                {
                    if (e.style.top == null) return "";
                    return e.style.top.ToString();
                }
                set
                {
                    e.style.top = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the position of the object relative to the left edge of the next positioned object in the document hierarchy."),
            CategoryAttribute("Size")]
            public string @left
            {
                get
                {
                    if (e.style.left == null) return "";
                    return e.style.left.ToString();
                }
                set
                {
                    e.style.left = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the bottom margin of the object."),
            CategoryAttribute("Margin")]
            public string @marginBottom
            {
                get
                {
                    if (e.style.marginBottom == null) return "";
                    return e.style.marginBottom.ToString();
                }
                set
                {
                    e.style.marginBottom = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the top margin of the object."),
            CategoryAttribute("Margin")]
            public string @marginTop
            {
                get
                {
                    if (e.style.marginTop == null) return "";
                    return e.style.marginTop.ToString();
                }
                set
                {
                    e.style.marginTop = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the right margin of the object."),
            CategoryAttribute("Margin")]
            public string @marginRight
            {
                get
                {
                    if (e.style.marginRight == null) return "";
                    return e.style.marginRight.ToString();
                }
                set
                {
                    e.style.marginRight = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the left margin of the object."),
            CategoryAttribute("Margin")]
            public string @marginLeft
            {
                get
                {
                    if (e.style.marginLeft == null) return "";
                    return e.style.marginLeft.ToString();
                }
                set
                {
                    e.style.marginLeft = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the top, right, bottom, and left margins of the object."),
            CategoryAttribute("Margin")]
            public string @margin
            {
                get
                {
                    return e.style.margin;
                }
                set
                {
                    e.style.margin = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of space to insert between the object and its margin or, if there is a border, between the object and its border."),
            CategoryAttribute("Padding")]
            public string @padding
            {
                get
                {
                    return e.style.padding;
                }
                set
                {
                    e.style.padding = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of space to insert between the bottom border of the object and the content."),
            CategoryAttribute("Padding")]
            public string @paddingBottom
            {
                get
                {
                    if (e.style.paddingBottom == null) return "";
                    return e.style.paddingBottom.ToString();
                }
                set
                {
                    e.style.paddingBottom = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of space to insert between the top border of the object and the content."),
            CategoryAttribute("Padding")]
            public string @paddingTop
            {
                get
                {
                    if (e.style.paddingTop == null) return "";
                    return e.style.paddingTop.ToString();
                }
                set
                {
                    e.style.paddingTop = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of space to insert between the right border of the object and the content."),
            CategoryAttribute("Padding")]
            public string @paddingRight
            {
                get
                {
                    if (e.style.paddingRight == null) return "";
                    return e.style.paddingRight.ToString();
                }
                set
                {
                    e.style.paddingRight = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of space to insert between the left border of the object and the content."),
            CategoryAttribute("Padding")]
            public string @paddingLeft
            {
                get
                {
                    if (e.style.paddingLeft == null) return "";
                    return e.style.paddingLeft.ToString();
                }
                set
                {
                    e.style.paddingLeft = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Pixel")]
            public int @pixelHeight
            {
                get
                {
                    return e.style.pixelHeight;
                }
                set
                {
                    e.style.pixelHeight = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object."),
            CategoryAttribute("Pixel")]
            public int @pixelWidth
            {
                get
                {
                    return e.style.pixelWidth;
                }
                set
                {
                    e.style.pixelWidth = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the top position of the object."),
            CategoryAttribute("Pixel")]
            public int @pixelTop
            {
                get
                {
                    return e.style.pixelTop;
                }
                set
                {
                    e.style.pixelTop = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the left position of the object."),
            CategoryAttribute("Pixel")]
            public int @pixelLeft
            {
                get
                {
                    return e.style.pixelLeft;
                }
                set
                {
                    e.style.pixelLeft = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object in the units specified by the height attribute."),
            CategoryAttribute("Position")]
            public float @posHeight
            {
                get
                {
                    return e.style.posHeight;
                }
                set
                {
                    e.style.@posHeight = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object in the units specified by the width attribute."),
            CategoryAttribute("Position")]
            public float @posWidth
            {
                get
                {
                    return e.style.posWidth;
                }
                set
                {
                    e.style.posWidth = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the top position of the object in the units specified by the top attribute."),
            CategoryAttribute("Position")]
            public float @posTop
            {
                get
                {
                    return e.style.posTop;
                }
                set
                {
                    e.style.posTop = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the left position of the object in the units specified by the left attribute."),
            CategoryAttribute("Position")]
            public float @posLeft
            {
                get
                {
                    return e.style.posLeft;
                }
                set
                {
                    e.style.posLeft = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }

        }
        public class OtherEditAdapter
        {
            private IHTMLElement e;
            private string CID;
            private string PName;
            private IHTMLElement PE;
            public OtherEditAdapter(IHTMLElement ele, string ControlPartID, string PartStyleName, IHTMLElement PartElement)
            {
                e = ele;
                CID = ControlPartID;
                PName = PartStyleName;
                PE = PartElement;
            }
            [DescriptionAttribute("Sets or retrieves the persisted representation of the style rule."),
            CategoryAttribute("Others")]
            public string @cssText
            {
                get
                {
                    return e.style.cssText;
                }
                set
                {
                    e.style.cssText = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(cursorConverter)),
           DescriptionAttribute("Sets or retrieves the type of cursor to display as the mouse pointer moves over the object."),
            CategoryAttribute("Others")]
            public string @cursor
            {
                get
                {
                    if (e.style.cursor == null) return NotSet;
                    return e.style.cursor;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.cursor = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(displayConverter)),
           DescriptionAttribute("Sets or retrieves whether the object is rendered."),
            CategoryAttribute("Others")]
            public string @display
            {
                get
                {
                    if (e.style.display == null) return NotSet;
                    return e.style.display;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.display = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(overflowConverter)),
           DescriptionAttribute("Sets or retrieves a value indicating how to manage the content of the object when the content exceeds the height or width of the object."),
            CategoryAttribute("Others")]
            public string @overflow
            {
                get
                {
                    if (e.style.overflow == null) return NotSet;
                    return e.style.overflow;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.overflow = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(styleFloatConverter)),
           DescriptionAttribute("Sets or retrieves on which side of the object the text will flow."),
            CategoryAttribute("Others")]
            public string @styleFloat
            {
                get
                {
                    if (e.style.styleFloat == null) return NotSet;
                    return e.style.styleFloat;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.styleFloat = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [TypeConverter(typeof(visibilityConverter)),
           DescriptionAttribute("Sets or retrieves whether the content of the object is displayed."),
            CategoryAttribute("Others")]
            public string @visibility
            {
                get
                {
                    if (e.style.visibility == null) return NotSet;
                    return e.style.visibility;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    e.style.visibility = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }
            [DescriptionAttribute("Sets or retrieves the stacking order of positioned objects."),
            CategoryAttribute("Others")]
            public string @zIndex
            {
                get
                {
                    return e.style.zIndex.ToString();
                }
                set
                {
                    e.style.zIndex = value;
                    setPartStyle(e, CID, PName, PE);
                }
            }

        }
    }

}

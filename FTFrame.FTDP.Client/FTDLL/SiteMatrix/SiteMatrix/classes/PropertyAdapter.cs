//builder by maobb,2005-7-8 to 2005-7-14
using System;
using mshtml;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using SiteMatrix.classes;

namespace SiteMatrix.Adapter
{
    /// <summary>
    /// PropertyAdapter 的摘要说明。
    /// </summary>
    /// 
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class Property
    {
        public static string NotSet = SiteMatrix.consts.res._pageware.GetString("NotSet");
        [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
        public class PropertyAdapter
        {

            public PropertyAdapter()
            {
                System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            }
            public static string getEleAttr(IHTMLElement e, string name)
            {
                System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
                if (e.getAttribute(name, 0) == null) return "";
                return e.getAttribute(name, 0).ToString();
            }
            public static void setEleAttr(IHTMLElement e, string name, string _value)
            {
                System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
                if (e.getAttribute(name, 0) != null)
                {
                    if (_value.Equals("") || _value == null)
                    {
                        e.removeAttribute(name, 0);
                        return;
                    }
                    e.setAttribute(name, _value, 0);
                    return;
                }
                if (_value.Equals("") || _value == null) return;
                e.setAttribute(name, _value, 0);
            }

        }
        public class scrollConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"yes", 
																	"no","auto"});
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
        public class bgPropertiesConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"empty string", 
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
                return false;
            }
        }
        public class enctypeConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"UrlEncoded", 
																	"Multipart","multipart/form-data"});
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
        public class methodConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"get", 
																	"post"});
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
        public class frameBorderConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"yes", 
																	"no"});
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
        public class scrollingConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"auto", 
																	"no","yes"});
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
        public class alignFieldSetConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"absbottom", 
																	"absmiddle","baseline","bottom","left","middle","right","texttop","top"});
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
        public class alignLegendConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"bottom", 
																	"center","left","right","top"});
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
        public class alignDivConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[]{NotSet,"center", 
																	"justify","left","right"});
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
        public class alignTableConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "left", "center", "right" });
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
        public class valignConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "middle", "baseline", "bottom", "top" });
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
        public class anchorConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "_blank", "_media", "_parent", "_search", "_self", "_top" });
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
        public class rulesConverter : StringConverter
        {
            public override StandardValuesCollection
                GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new string[] { NotSet, "all", "cols", "groups", "none", "rows" });
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
        public class LabelElement
        //public class LabelElement:ExpandableObjectConverter
        {
            private IHTMLElement e;
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object."),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the object to which the given label object is assigned."),
            CategoryAttribute("Behavior")]
            public string @for
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "for");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "for", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            public LabelElement(IHTMLElement ele)
            {
                e = ele;
            }
        }
        public class InputTextElement
        {
            private IHTMLElement e;
            public InputTextElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the size of the control."),
            CategoryAttribute("Appearance")]
            public string @size
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "size");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "size", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLInputTextElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLInputTextElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the maximum number of characters that the user can enter in a text control."),
            CategoryAttribute("Behavior")]
            public int maxLength
            {
                get
                {
                    return ((HTMLInputElement)(e)).maxLength;
                }
                set
                {
                    ((HTMLInputElement)(e)).maxLength = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicated whether the content of the object is read-only."),
            CategoryAttribute("Behavior")]
            public bool readOnly
            {
                get
                {
                    return ((HTMLInputElement)(e)).readOnly;
                }
                set
                {
                    ((HTMLInputElement)(e)).readOnly = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)), 
            DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)),
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key up."),
            CategoryAttribute("Events")]
            public string onKeyUp
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyUp");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyUp", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key down."),
            CategoryAttribute("Events")]
            public string onKeyDown
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyDown");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyDown", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if(e.style==null || e.style.width==null)return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the displayed value for the control object. This value is returned to the server when the control object is submitted."), CategoryAttribute("Misc")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
        }
        public class TextAreaElement
        {
            private IHTMLElement e;
            public TextAreaElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the number of horizontal rows contained in the object."),
            CategoryAttribute("Appearance")]
            public int @rows
            {
                get
                {
                    return ((IHTMLTextAreaElement)(e)).rows;
                }
                set
                {
                    ((IHTMLTextAreaElement)(e)).rows = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object."),
            CategoryAttribute("Appearance")]
            public int @cols
            {
                get
                {
                    return ((IHTMLTextAreaElement)(e)).cols;
                }
                set
                {
                    ((IHTMLTextAreaElement)(e)).cols = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }

            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((HTMLTextAreaElement)(e)).disabled;
                }
                set
                {
                    ((HTMLTextAreaElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLTextAreaElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLTextAreaElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicated whether the content of the object is read-only."),
            CategoryAttribute("Behavior")]
            public bool readOnly
            {
                get
                {
                    return ((HTMLTextAreaElement)(e)).readOnly;
                }
                set
                {
                    ((HTMLTextAreaElement)(e)).readOnly = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLTextAreaElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLTextAreaElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key up."),
            CategoryAttribute("Events")]
            public string onKeyUp
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyUp");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyUp", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key down."),
            CategoryAttribute("Events")]
            public string onKeyDown
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyDown");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyDown", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user repositions the scroll box in the scroll bar on the object."),
            CategoryAttribute("Events")]
            public string onScroll
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onScroll");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onScroll", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class PassWordElement
        {
            private IHTMLElement e;
            public PassWordElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the size of the control."),
            CategoryAttribute("Appearance")]
            public string @size
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "size");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "size", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLInputTextElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLInputTextElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the maximum number of characters that the user can enter in a text control."),
            CategoryAttribute("Behavior")]
            public int maxLength
            {
                get
                {
                    return ((HTMLInputElement)(e)).maxLength;
                }
                set
                {
                    ((HTMLInputElement)(e)).maxLength = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicated whether the content of the object is read-only."),
            CategoryAttribute("Behavior")]
            public bool readOnly
            {
                get
                {
                    return ((HTMLInputElement)(e)).readOnly;
                }
                set
                {
                    ((HTMLInputElement)(e)).readOnly = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class InputButtonElement
        {
            private IHTMLElement e;
            public InputButtonElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the displayed value for the control object. This value is returned to the server when the control object is submitted."),
            CategoryAttribute("Appearance")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLInputButtonElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLInputButtonElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)),
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class InputSubmitElement
        {
            private IHTMLElement e;
            public InputSubmitElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the displayed value for the control object. This value is returned to the server when the control object is submitted."),
            CategoryAttribute("Appearance")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLInputButtonElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLInputButtonElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class InputResetElement
        {
            private IHTMLElement e;
            public InputResetElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the displayed value for the control object. This value is returned to the server when the control object is submitted."),
            CategoryAttribute("Appearance")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLInputButtonElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLInputButtonElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class InputImageElement
        {
            private IHTMLElement e;
            public InputImageElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves a text alternative to the graphic."),
            CategoryAttribute("Behavior")]
            public string @alt
            {
                get
                {
                    return ((IHTMLInputImage)(e)).alt;
                }
                set
                {
                    ((IHTMLInputImage)(e)).alt = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLInputImage)(e)).disabled;
                }
                set
                {
                    ((IHTMLInputImage)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }

            [EditorAttribute(typeof(SourceEditor), typeof(UITypeEditor)),
           TypeConverterAttribute(typeof(SourceEditorConverter)),
                DescriptionAttribute("Sets or retrieves a URL to be loaded by the object."),
            CategoryAttribute("Behavior")]
            public string @src_file
            {
                get
                {
                    return ((IHTMLInputImage)(e)).src;
                }
                set
                {
                    ((IHTMLInputImage)(e)).src = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves a URL to be loaded by the object."),
            CategoryAttribute("Behavior")]
            public string @src_url
            {
                get
                {
                    return ((IHTMLInputImage)(e)).src;
                }
                set
                {
                    ((IHTMLInputImage)(e)).src = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)), 
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the horizontal margin for the object."),
            CategoryAttribute("Layout")]
            public int hSpace
            {
                get
                {
                    return ((IHTMLInputImage)(e)).hspace;
                }
                set
                {
                    ((IHTMLInputImage)(e)).hspace = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the vertical margin for the object."),
            CategoryAttribute("Layout")]
            public int vSpace
            {
                get
                {
                    return ((IHTMLInputImage)(e)).vspace;
                }
                set
                {
                    ((IHTMLInputImage)(e)).vspace = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the displayed value for the control object. This value is returned to the server when the control object is submitted."), CategoryAttribute("Misc")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class InputCheckBoxElement
        {
            private IHTMLElement e;
            public InputCheckBoxElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the state of the check box or radio button."),
            CategoryAttribute("Behavior")]
            public bool @checked
            {
                get
                {
                    return ((HTMLInputElement)(e)).@checked;
                }
                set
                {
                    ((HTMLInputElement)(e)).@checked = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((HTMLInputElement)(e)).disabled;
                }
                set
                {
                    ((HTMLInputElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)),
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class InputRadioElement
        {
            private IHTMLElement e;
            public InputRadioElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the state of the check box or radio button."),
            CategoryAttribute("Behavior")]
            public bool @checked
            {
                get
                {
                    return ((HTMLInputElement)(e)).@checked;
                }
                set
                {
                    ((HTMLInputElement)(e)).@checked = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((HTMLInputElement)(e)).disabled;
                }
                set
                {
                    ((HTMLInputElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)),
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class SelectElement
        {
            private IHTMLElement e;
            public SelectElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the number of rows in the list box."),
            CategoryAttribute("Appearance")]
            public int @size
            {
                get
                {
                    return ((IHTMLSelectElement)(e)).size;
                }
                set
                {
                    ((IHTMLSelectElement)(e)).size = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLSelectElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLSelectElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("The multiple property sets or returns whether or not multiple items can be selected in a dropdown list."),
            CategoryAttribute("Behavior")]
            public bool @multiple
            {
                get
                {
                    return ((IHTMLSelectElement)(e)).multiple;
                }
                set
                {
                    ((IHTMLSelectElement)(e)).multiple = value;
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)),
            DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)), 
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class InputHiddenElement
        {
            private IHTMLElement e;
            public InputHiddenElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
        }
        public class InputFileElement
        {
            private IHTMLElement e;
            public InputFileElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the size of the control."),
            CategoryAttribute("Appearance")]
            public string @size
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "size");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "size", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLInputFileElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLInputFileElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLInputElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLInputElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLInputElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLInputElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the displayed value for the control object. This value is returned to the server when the control object is submitted."), CategoryAttribute("Misc")]
            public string @value
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "value");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "value", value);
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class FieldSetElement
        {
            private IHTMLElement e;
            public FieldSetElement(IHTMLElement ele)
            {
                e = ele;
            }
            [TypeConverter(typeof(alignFieldSetConverter)),
            DescriptionAttribute("Sets or retrieves how the object is aligned with adjacent text."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((IHTMLFieldSetElement)(e)).align == null) return NotSet;
                    return ((IHTMLFieldSetElement)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLFieldSetElement)(e)).align = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }

            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLFieldSetElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLFieldSetElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLFieldSetElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLFieldSetElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Fires when the contents of the object or selection have changed."),
            CategoryAttribute("Events")]
            public string onChange
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onChange");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onChange", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user repositions the scroll box in the scroll bar on the object. "),
            CategoryAttribute("Events")]
            public string onScroll
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onScroll");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onScroll", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class LegendElement
        {
            private IHTMLElement e;
            public LegendElement(IHTMLElement ele)
            {
                e = ele;
            }

            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }

            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class AnchorElement
        {
            private IHTMLElement e;
            public AnchorElement(IHTMLElement ele)
            {
                e = ele;
            }

            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)), 
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the object with either mouse button."),
            CategoryAttribute("Events")]
            public string onMouseDown
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseDown");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseDown", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user releases a mouse button while the mouse is over the object."),
            CategoryAttribute("Events")]
            public string onMouseUp
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseUp");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseUp", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((HTMLAnchorElement)(e)).disabled;
                }
                set
                {
                    ((HTMLAnchorElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLAnchorElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLAnchorElement)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the destination URL or anchor point."),
            CategoryAttribute("Behavior")]
            public string @href
            {
                get
                {
                    return ((HTMLAnchorElement)(e)).href;
                }
                set
                {
                    ((HTMLAnchorElement)(e)).href = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLAnchorElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLAnchorElement)(e)).tabIndex = value;
                }
            }
            [TypeConverter(typeof(anchorConverter)),
            DescriptionAttribute("Sets or retrieves the window or frame at which to target content."),
            CategoryAttribute("Appearance")]
            public string @target
            {
                get
                {

                    if (((HTMLAnchorElement)(e)).target == null) return NotSet;
                    return ((HTMLAnchorElement)(e)).target;

                }
                set
                {
                    if (NotSet.Equals(value))
                    {
                        PropertyAdapter.setEleAttr(e, "target", "");
                        return;
                    }
                    ((HTMLAnchorElement)(e)).target = value;
                }
            }
        }
        public class ImageElement
        {
            private IHTMLElement e;
            public ImageElement(IHTMLElement ele)
            {
                e = ele;
            }

            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)), 
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the user double-clicks the object."),
            CategoryAttribute("Events")]
            public string onDblClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onDblClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onDblClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the object with either mouse button."),
            CategoryAttribute("Events")]
            public string onMouseDown
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseDown");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseDown", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user releases a mouse button while the mouse is over the object."),
            CategoryAttribute("Events")]
            public string onMouseUp
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseUp");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseUp", value);
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Layout")]
            public int @height
            {
                get
                {
                    return ((HTMLImg)(e)).height;
                }
                set
                {
                    ((HTMLImg)(e)).height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the calculated width of the object."),
            CategoryAttribute("Layout")]
            public int @width
            {
                get
                {
                    return ((HTMLImg)(e)).width;
                }
                set
                {
                    ((HTMLImg)(e)).width = value;
                }
            }
            [TypeConverter(typeof(alignFieldSetConverter)),
            DescriptionAttribute("Sets or retrieves how the object is aligned with adjacent text."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((HTMLImg)(e)).align == null) return NotSet;
                    return ((HTMLImg)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((HTMLImg)(e)).align = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves a text alternative to the graphic."),
            CategoryAttribute("Appearance")]
            public string @alt
            {
                get
                {
                    return ((HTMLImg)(e)).alt;
                }
                set
                {
                    ((HTMLImg)(e)).alt = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the border to draw around the object."),
            CategoryAttribute("Appearance")]
            public string @border
            {
                get
                {
                    if (((HTMLImg)(e)).border == null) return "";
                    return ((HTMLImg)(e)).border.ToString();
                }
                set
                {
                    ((HTMLImg)(e)).border = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the horizontal margin for the object."),
            CategoryAttribute("Appearance")]
            public int @hSpace
            {
                get
                {
                    return ((HTMLImg)(e)).hspace;
                }
                set
                {
                    ((HTMLImg)(e)).hspace = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the vertical margin for the object."),
            CategoryAttribute("Appearance")]
            public int @vSpace
            {
                get
                {
                    return ((HTMLImg)(e)).vspace;
                }
                set
                {
                    ((HTMLImg)(e)).vspace = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLImg)(e)).hideFocus;
                }
                set
                {
                    ((HTMLImg)(e)).hideFocus = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLImg)(e)).tabIndex;
                }
                set
                {
                    ((HTMLImg)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [EditorAttribute(typeof(SourceEditor), typeof(UITypeEditor)),
           TypeConverterAttribute(typeof(SourceEditorConverter)),
            DescriptionAttribute("Sets or retrieves a URL to be loaded by the object."), CategoryAttribute("Misc")]
            public string @src_file
            {
                get
                {
                    return ((HTMLImg)(e)).src;
                }
                set
                {
                    ((HTMLImg)(e)).src = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves a URL to be loaded by the object."), CategoryAttribute("Misc")]
            public string @src_url
            {
                get
                {
                    //return PropertyAdapter.getEleAttr(e, "src");
                    return ((HTMLImg)(e)).src;
                }
                set
                {
                    //PropertyAdapter.setEleAttr(e, "src", value);
                    ((HTMLImg)(e)).src = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class TabelElement
        {
            private IHTMLElement e;
            public TabelElement(IHTMLElement ele)
            {
                e = ele;
            }

            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the size of the object is about to change."),
            CategoryAttribute("Events")]
            public string onResize
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onResize");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onResize", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [TypeConverter(typeof(alignTableConverter)),
            DescriptionAttribute("Sets or retrieves a value that indicates the table alignment."),
            CategoryAttribute("Layout")]
            public string @align
            {
                get
                {

                    if (((IHTMLTable)(e)).align == null) return NotSet;
                    return ((IHTMLTable)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLTable)(e)).align = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Layout")]
            public string @height
            {
                get
                {
                    if (((IHTMLTable)(e)).height == null) return "";
                    return ((IHTMLTable)(e)).height.ToString();
                }
                set
                {
                    ((IHTMLTable)(e)).height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object."),
            CategoryAttribute("Layout")]
            public string @width
            {
                get
                {
                    if (((IHTMLTable)(e)).width == null) return "";
                    return ((IHTMLTable)(e)).width.ToString();
                }
                set
                {
                    ((IHTMLTable)(e)).width = value;
                }
            }
            [EditorAttribute(typeof(SourceEditor), typeof(UITypeEditor)),
           TypeConverterAttribute(typeof(SourceEditorConverter)),
                DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics in the object."),
            CategoryAttribute("Appearance")]
            public string @background_file
            {
                get
                {
                    return ((IHTMLTable)(e)).background;
                }
                set
                {
                    ((IHTMLTable)(e)).background = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics in the object."),
            CategoryAttribute("Appearance")]
            public string @background_url
            {
                get
                {
                    return ((IHTMLTable)(e)).background;
                }
                set
                {
                    ((IHTMLTable)(e)).background = value;
                }
            }
            [DescriptionAttribute("Deprecated. Sets or retrieves the background color behind the object."),
            CategoryAttribute("Appearance")]
            public Color @bgColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTable)(e)).bgColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTable)(e)).bgColor = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the border to draw around the object."),
            CategoryAttribute("Appearance")]
            public string @border
            {
                get
                {
                    if (((IHTMLTable)(e)).border == null) return "";
                    return ((IHTMLTable)(e)).border.ToString();
                }
                set
                {
                    ((IHTMLTable)(e)).border = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the border color of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTable)(e)).borderColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTable)(e)).borderColor = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorDark
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTable)(e)).borderColorDark.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTable)(e)).borderColorDark = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorLight
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTable)(e)).borderColorLight.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTable)(e)).borderColorLight = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of space between the border of the cell and the content of the cell."),
            CategoryAttribute("Appearance")]
            public string @cellPadding
            {
                get
                {
                    if (((IHTMLTable)(e)).cellPadding == null) return "";
                    return ((IHTMLTable)(e)).cellPadding.ToString();
                }
                set
                {
                    ((IHTMLTable)(e)).cellPadding = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the amount of space between cells in a table."),
            CategoryAttribute("Appearance")]
            public string @cellSpacing
            {
                get
                {
                    if (((IHTMLTable)(e)).cellSpacing == null) return "";
                    return ((IHTMLTable)(e)).cellSpacing.ToString();
                }
                set
                {
                    ((IHTMLTable)(e)).cellSpacing = value;
                }
            }
            [TypeConverter(typeof(rulesConverter)),
            DescriptionAttribute("Sets or retrieves which dividing lines (inner borders) are displayed."),
            CategoryAttribute("Appearance")]
            public string @rules
            {
                get
                {

                    if (((IHTMLTable)(e)).rules == null) return NotSet;
                    return ((IHTMLTable)(e)).rules;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLTable)(e)).rules = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }

            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLTable)(e)).hideFocus;
                }
                set
                {
                    ((HTMLTable)(e)).hideFocus = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLTable)(e)).tabIndex;
                }
                set
                {
                    ((HTMLTable)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class TableTrElement
        {
            private IHTMLElement e;
            public TableTrElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }

            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Deprecated. Sets or retrieves the background color behind the object."),
            CategoryAttribute("Appearance")]
            public Color @bgColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableRow)(e)).bgColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableRow)(e)).bgColor = ColorTranslator.ToHtml(value);
                }
            }

            [DescriptionAttribute("Sets or retrieves the border color of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableRow)(e)).borderColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableRow)(e)).borderColor = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorDark
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableRow)(e)).borderColorDark.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableRow)(e)).borderColorDark = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorLight
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableRow)(e)).borderColorLight.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableRow)(e)).borderColorLight = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Appearance")]
            public string @height
            {
                get
                {
                    if (((HTMLTableRow)(e)).height == null) return "";
                    return ((HTMLTableRow)(e)).height.ToString();

                }
                set
                {
                    ((HTMLTableRow)(e)).height = value;
                }
            }
            [TypeConverter(typeof(valignConverter)),
            DescriptionAttribute("Sets or retrieves how text and other content are vertically aligned within the object that contains them."),
            CategoryAttribute("Layout")]
            public string @vAlign
            {
                get
                {

                    if (((HTMLTableRow)(e)).vAlign == null) return NotSet;
                    return ((HTMLTableRow)(e)).vAlign;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((HTMLTableRow)(e)).vAlign = value;
                }
            }
            [TypeConverter(typeof(alignDivConverter)),
            DescriptionAttribute("Sets or retrieves the alignment of the object relative to the display or table."),
            CategoryAttribute("Layout")]
            public string @align
            {
                get
                {

                    if (((HTMLTableRow)(e)).align == null) return NotSet;
                    return ((HTMLTableRow)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((HTMLTableRow)(e)).align = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class TableTdElement
        {
            private IHTMLElement e;
            public TableTdElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }

            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user change the dimensions of the object in a control selection."),
            CategoryAttribute("Events")]
            public string onResize
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onResize");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onResize", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Layout")]
            public string @height
            {
                get
                {
                    if (((IHTMLTableCell)(e)).height == null) return "";
                    return ((IHTMLTableCell)(e)).height.ToString();
                }
                set
                {
                    ((IHTMLTableCell)(e)).height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object."),
            CategoryAttribute("Layout")]
            public string @width
            {
                get
                {
                    if (((IHTMLTableCell)(e)).width == null) return "";
                    return ((IHTMLTableCell)(e)).width.ToString();
                }
                set
                {
                    ((IHTMLTableCell)(e)).width = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the number columns in the table that the object should span."),
            CategoryAttribute("Layout")]
            public int @colSpan
            {
                get
                {
                    return ((IHTMLTableCell)(e)).colSpan;
                }
                set
                {
                    ((IHTMLTableCell)(e)).colSpan = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves how many rows in a table the cell should span."),
            CategoryAttribute("Layout")]
            public int @rowSpan
            {
                get
                {
                    return ((IHTMLTableCell)(e)).rowSpan;
                }
                set
                {
                    ((IHTMLTableCell)(e)).rowSpan = value;
                }
            }
            [TypeConverter(typeof(alignDivConverter)),
            DescriptionAttribute("Sets or retrieves the alignment of the object relative to the display or table."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((IHTMLTableCell)(e)).align == null) return NotSet;
                    return ((IHTMLTableCell)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLTableCell)(e)).align = value;
                }
            }
            [TypeConverter(typeof(valignConverter)),
            DescriptionAttribute("Sets or retrieves how text and other content are vertically aligned within the object that contains them."),
            CategoryAttribute("Appearance")]
            public string @vAlign
            {
                get
                {

                    if (((IHTMLTableCell)(e)).vAlign == null) return NotSet;
                    return ((IHTMLTableCell)(e)).vAlign;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLTableCell)(e)).vAlign = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the border color of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).borderColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).borderColor = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorDark
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).borderColorDark.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).borderColorDark = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorLight
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).borderColorLight.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).borderColorLight = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Deprecated. Sets or retrieves the background color behind the object."),
            CategoryAttribute("Appearance")]
            public Color @bgColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).bgColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).bgColor = ColorTranslator.ToHtml(value);
                }
            }
            [EditorAttribute(typeof(SourceEditor), typeof(UITypeEditor)),
           TypeConverterAttribute(typeof(SourceEditorConverter)),
                DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics in the object."),
            CategoryAttribute("Appearance")]
            public string @background_file
            {
                get
                {
                    return ((IHTMLTableCell)(e)).background;

                }
                set
                {
                    ((IHTMLTableCell)(e)).background = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics in the object."),
            CategoryAttribute("Appearance")]
            public string @background_url
            {
                get
                {
                    return ((IHTMLTableCell)(e)).background;

                }
                set
                {
                    ((IHTMLTableCell)(e)).background = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves whether the browser automatically performs wordwrap."),
            CategoryAttribute("Behavior")]
            public bool @noWrap
            {
                get
                {
                    return ((IHTMLTableCell)(e)).noWrap;

                }
                set
                {
                    ((IHTMLTableCell)(e)).noWrap = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class TableThElement
        {
            private IHTMLElement e;
            public TableThElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }

            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user change the dimensions of the object in a control selection."),
            CategoryAttribute("Events")]
            public string onResize
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onResize");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onResize", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Layout")]
            public string @height
            {
                get
                {
                    if (((IHTMLTableCell)(e)).height == null) return "";
                    return ((IHTMLTableCell)(e)).height.ToString();
                }
                set
                {
                    ((IHTMLTableCell)(e)).height = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object."),
            CategoryAttribute("Layout")]
            public string @width
            {
                get
                {
                    if (((IHTMLTableCell)(e)).width == null) return "";
                    return ((IHTMLTableCell)(e)).width.ToString();
                }
                set
                {
                    ((IHTMLTableCell)(e)).width = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the number columns in the table that the object should span."),
            CategoryAttribute("Layout")]
            public int @colSpan
            {
                get
                {
                    return ((IHTMLTableCell)(e)).colSpan;
                }
                set
                {
                    ((IHTMLTableCell)(e)).colSpan = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves how many rows in a table the cell should span."),
            CategoryAttribute("Layout")]
            public int @rowSpan
            {
                get
                {
                    return ((IHTMLTableCell)(e)).rowSpan;
                }
                set
                {
                    ((IHTMLTableCell)(e)).rowSpan = value;
                }
            }
            [TypeConverter(typeof(alignDivConverter)),
            DescriptionAttribute("Sets or retrieves the alignment of the object relative to the display or table."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((IHTMLTableCell)(e)).align == null) return NotSet;
                    return ((IHTMLTableCell)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLTableCell)(e)).align = value;
                }
            }
            [TypeConverter(typeof(valignConverter)),
            DescriptionAttribute("Sets or retrieves how text and other content are vertically aligned within the object that contains them."),
            CategoryAttribute("Appearance")]
            public string @vAlign
            {
                get
                {

                    if (((IHTMLTableCell)(e)).vAlign == null) return NotSet;
                    return ((IHTMLTableCell)(e)).vAlign;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLTableCell)(e)).vAlign = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the border color of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).borderColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).borderColor = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorDark
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).borderColorDark.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).borderColorDark = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color for one of the two colors used to draw the 3-D border of the object."),
            CategoryAttribute("Appearance")]
            public Color @borderColorLight
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).borderColorLight.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).borderColorLight = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Deprecated. Sets or retrieves the background color behind the object."),
            CategoryAttribute("Appearance")]
            public Color @bgColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLTableCell)(e)).bgColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLTableCell)(e)).bgColor = ColorTranslator.ToHtml(value);
                }
            }
            [EditorAttribute(typeof(SourceEditor), typeof(UITypeEditor)),
           TypeConverterAttribute(typeof(SourceEditorConverter)),
                DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics in the object."),
            CategoryAttribute("Appearance")]
            public string @background_file
            {
                get
                {
                    return ((IHTMLTableCell)(e)).background;

                }
                set
                {
                    ((IHTMLTableCell)(e)).background = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics in the object."),
            CategoryAttribute("Appearance")]
            public string @background_url
            {
                get
                {
                    return ((IHTMLTableCell)(e)).background;

                }
                set
                {
                    ((IHTMLTableCell)(e)).background = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves whether the browser automatically performs wordwrap."),
            CategoryAttribute("Behavior")]
            public bool @noWrap
            {
                get
                {
                    return ((IHTMLTableCell)(e)).noWrap;

                }
                set
                {
                    ((IHTMLTableCell)(e)).noWrap = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class FontElement
        {
            private IHTMLElement e;
            public FontElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the color to be used by the object."),
            CategoryAttribute("Appearance")]
            public Color @color
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLFontElement)(e)).color.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLFontElement)(e)).color = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the current typeface family."),
            CategoryAttribute("Appearance")]
            public string @face
            {
                get
                {
                    return ((IHTMLFontElement)(e)).face;
                }
                set
                {
                    ((IHTMLFontElement)(e)).face = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the size of the control."),
            CategoryAttribute("Appearance")]
            public string @size
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "size");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "size", value);
                }
            }
        }
        public class SpanElement
        {
            private IHTMLElement e;
            public SpanElement(IHTMLElement ele)
            {
                e = ele;
            }

            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }

            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLSpanElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLSpanElement)(e)).hideFocus = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLSpanElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLSpanElement)(e)).tabIndex = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class DivElement
        {
            private IHTMLElement e;
            public DivElement(IHTMLElement ele)
            {
                e = ele;
            }

            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }

            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user repositions the scroll box in the scroll bar on the object."),
            CategoryAttribute("Events")]
            public string onScroll
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onScroll");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onScroll", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [TypeConverter(typeof(alignDivConverter)),
            DescriptionAttribute("Sets or retrieves the alignment of the object relative to the display or table."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((IHTMLDivElement)(e)).align == null) return NotSet;
                    return ((IHTMLDivElement)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLDivElement)(e)).align = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((HTMLDivElement)(e)).disabled;
                }
                set
                {
                    ((HTMLDivElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves whether the browser automatically performs wordwrap."),
            CategoryAttribute("Behavior")]
            public bool @noWrap
            {
                get
                {
                    return ((IHTMLDivElement)(e)).noWrap;
                }
                set
                {
                    ((IHTMLDivElement)(e)).noWrap = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
        }
        public class IFrameElement
        {
            private IHTMLElement e;
            public IFrameElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Fires when the object is set as the active element."),
            CategoryAttribute("Events")]
            public string onActivate
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onActivate");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onActivate", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            public string onDeactivate
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onDeactivate");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onDeactivate", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires immediately after the browser loads the object."),
            CategoryAttribute("Events")]
            public string onLoad
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onLoad");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onLoad", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user repositions the scroll box in the scroll bar on the object."),
            CategoryAttribute("Events")]
            public string onScroll
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onScroll");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onScroll", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Layout")]
            public string @height
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "height");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "height", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the width of the object."),
            CategoryAttribute("Layout")]
            public string @width
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "width");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "width", value);
                }
            }
            [TypeConverter(typeof(alignDivConverter)),
            DescriptionAttribute("Sets or retrieves a value that indicates the table alignment."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((IHTMLIFrameElement)(e)).align == null) return NotSet;
                    return ((IHTMLIFrameElement)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLIFrameElement)(e)).align = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the space between the frames, including the 3-D border."),
            CategoryAttribute("Appearance")]
            public string @border
            {
                get
                {
                    if (((HTMLIFrameClass)(e)).border == null) return "";
                    return ((HTMLIFrameClass)(e)).border.ToString();

                }
                set
                {
                    ((HTMLIFrameClass)(e)).border = value;
                }
            }
            [TypeConverter(typeof(frameBorderConverter)),
            DescriptionAttribute("Sets or retrieves whether to display a border for the frame."),
            CategoryAttribute("Appearance")]
            public string @frameBorder
            {
                get
                {

                    if (((HTMLIFrameClass)(e)).frameBorder == null) return NotSet;
                    return ((HTMLIFrameClass)(e)).frameBorder;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((HTMLIFrameClass)(e)).frameBorder = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the horizontal margin for the object."),
            CategoryAttribute("Appearance")]
            public int @hSpace
            {
                get
                {

                    return ((HTMLIFrameClass)(e)).hspace;

                }
                set
                {
                    ((HTMLIFrameClass)(e)).hspace = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the vertical margin for the object."),
            CategoryAttribute("Appearance")]
            public int @vSpace
            {
                get
                {

                    return ((HTMLIFrameClass)(e)).vspace;

                }
                set
                {
                    ((HTMLIFrameClass)(e)).vspace = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the top and bottom margin heights before displaying the text in a frame."),
            CategoryAttribute("Appearance")]
            public string @marginHeight
            {
                get
                {
                    if (((HTMLIFrameClass)(e)).marginHeight == null) return "";
                    return ((HTMLIFrameClass)(e)).marginHeight.ToString();

                }
                set
                {
                    ((HTMLIFrameClass)(e)).marginHeight = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the left and right margin widths before displaying the text in a frame."),
            CategoryAttribute("Appearance")]
            public string @marginWidth
            {
                get
                {
                    if (((HTMLIFrameClass)(e)).marginWidth == null) return "";
                    return ((HTMLIFrameClass)(e)).marginWidth.ToString();

                }
                set
                {
                    ((HTMLIFrameClass)(e)).marginWidth = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLIFrameClass)(e)).hideFocus;
                }
                set
                {
                    ((HTMLIFrameClass)(e)).hideFocus = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLIFrameClass)(e)).tabIndex;
                }
                set
                {
                    ((HTMLIFrameClass)(e)).tabIndex = value;
                }
            }
            [TypeConverter(typeof(scrollingConverter)),
            DescriptionAttribute("Sets or retrieves whether the frame can be scrolled."),
            CategoryAttribute("Behavior")]
            public string @scrolling
            {
                get
                {

                    if (((HTMLIFrameClass)(e)).scrolling == null) return NotSet;
                    return ((HTMLIFrameClass)(e)).scrolling;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((HTMLIFrameClass)(e)).scrolling = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves a URL to be loaded by the object."),
            CategoryAttribute("Misc")]
            public string @src
            {
                get
                {

                    return ((HTMLIFrameClass)(e)).src;

                }
                set
                {
                    ((HTMLIFrameClass)(e)).src = value;
                }
            }
        }
        public class HRElement
        {
            private IHTMLElement e;
            public HRElement(IHTMLElement ele)
            {
                e = ele;
            }
            [TypeConverter(typeof(alignDivConverter)),
            DescriptionAttribute("Sets or retrieves the alignment of the object relative to the display or table."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((IHTMLHRElement)(e)).align == null) return NotSet;
                    return ((IHTMLHRElement)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLHRElement)(e)).align = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the color to be used by the object."),
            CategoryAttribute("Appearance")]
            public Color @color
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLHRElement)(e)).color.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLHRElement)(e)).color = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves whether the horizontal rule is drawn with 3-D shading."),
            CategoryAttribute("Appearance")]
            public bool @noShade
            {
                get
                {
                    return ((IHTMLHRElement)(e)).noShade;

                }
                set
                {
                    ((IHTMLHRElement)(e)).noShade = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the size of the control."),
            CategoryAttribute("Appearance")]
            public string @size
            {
                get
                {
                    if (((IHTMLHRElement)(e)).size == null) return "";
                    return ((IHTMLHRElement)(e)).size.ToString();

                }
                set
                {
                    ((IHTMLHRElement)(e)).size = value;
                }
            }
        }
        public class FormElement
        {
            private IHTMLElement e;
            public FormElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the URL to which the form content is sent for processing."),
            CategoryAttribute("Behavior")]
            public string @action
            {
                get
                {
                    return ((IHTMLFormElement)(e)).action;
                }
                set
                {
                    ((IHTMLFormElement)(e)).action = value;
                }
            }
            [TypeConverter(typeof(enctypeConverter)),
            DescriptionAttribute("Sets or retrieves the MIME encoding for the form."),
            CategoryAttribute("Behavior")]
            public string @encoding
            {
                get
                {
                    if (((IHTMLFormElement)(e)).encoding == null) return NotSet;
                    return ((IHTMLFormElement)(e)).encoding;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLFormElement)(e)).encoding = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the Multipurpose Internet Mail Extensions (MIME) encoding for the form."),
            CategoryAttribute("Behavior")]
            public string @encType
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "encType");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "encType", value);
                }
            }
            [TypeConverter(typeof(methodConverter)),
            DescriptionAttribute("Sets or retrieves how to send the form data to the server."),
            CategoryAttribute("Behavior")]
            public string @method
            {
                get
                {
                    if (((IHTMLFormElement)(e)).method == null) return NotSet;
                    return ((IHTMLFormElement)(e)).method;
                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLFormElement)(e)).method = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the window or frame at which to target content."),
            CategoryAttribute("Behavior")]
            public string @target
            {
                get
                {
                    return ((IHTMLFormElement)(e)).target;
                }
                set
                {
                    ((IHTMLFormElement)(e)).target = value;
                }
            }

            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }

            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user clicks the object with either mouse button."),
            CategoryAttribute("Events")]
            public string onMouseDown
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseDown");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseDown", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Fires when the user releases a mouse button while the mouse is over the object."),
            CategoryAttribute("Events")]
            public string onMouseUp
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseUp");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseUp", value);
                }
            }
            [DescriptionAttribute("Fires when the user resets a form."),
            CategoryAttribute("Events")]
            public string onReset
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onReset");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onReset", value);
                }
            }
            [DescriptionAttribute("Fires when a FORM is about to be submitted."),
            CategoryAttribute("Events")]
            public string onSubmit
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onSubmit");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onSubmit", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
        public class BodyElement
        {
            private IHTMLElement e;
            public BodyElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the color of all active links in the element."),
            CategoryAttribute("Appearance")]
            public Color @aLink
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLBodyElement)(e)).aLink.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLBodyElement)(e)).aLink = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Deprecated. Sets or retrieves the background color behind the object."),
            CategoryAttribute("Appearance")]
            public Color @bgColor
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLBodyElement)(e)).bgColor.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLBodyElement)(e)).bgColor = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color of the document links for the object."),
            CategoryAttribute("Appearance")]
            public Color @link
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLBodyElement)(e)).link.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLBodyElement)(e)).link = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the text (foreground) color for the document body."),
            CategoryAttribute("Appearance")]
            public Color @text
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLBodyElement)(e)).text.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLBodyElement)(e)).text = ColorTranslator.ToHtml(value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the color of links in the object that have already been visited."),
            CategoryAttribute("Appearance")]
            public Color @vLink
            {
                get
                {
                    try
                    {
                        return ColorTranslator.FromHtml(((IHTMLBodyElement)(e)).vLink.ToString());
                    }
                    catch
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    ((IHTMLBodyElement)(e)).vLink = ColorTranslator.ToHtml(value);
                }
            }

            [EditorAttribute(typeof(SourceEditor), typeof(UITypeEditor)),
           TypeConverterAttribute(typeof(SourceEditorConverter)),
                DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics on the page."),
            CategoryAttribute("Appearance")]
            public string @background_file
            {
                get
                {
                    return ((IHTMLBodyElement)(e)).background;
                }
                set
                {
                    ((IHTMLBodyElement)(e)).background = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the background picture tiled behind the text and graphics on the page."),
            CategoryAttribute("Appearance")]
            public string @background_url
            {
                get
                {
                    return ((IHTMLBodyElement)(e)).background;
                }
                set
                {
                    ((IHTMLBodyElement)(e)).background = value;
                }
            }
            [TypeConverter(typeof(bgPropertiesConverter)),
            DescriptionAttribute("Sets or retrieves the properties of the background picture."),
            CategoryAttribute("Appearance")]
            public string @bgProperties
            {
                get
                {

                    if (((IHTMLBodyElement)(e)).bgProperties == null) return NotSet;
                    return ((IHTMLBodyElement)(e)).bgProperties;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLBodyElement)(e)).bgProperties = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves whether the browser automatically performs wordwrap."),
            CategoryAttribute("Behavior")]
            public bool @noWrap
            {
                get
                {
                    return ((IHTMLBodyElement)(e)).noWrap;
                }
                set
                {
                    ((IHTMLBodyElement)(e)).noWrap = value;
                }
            }
            [TypeConverter(typeof(scrollConverter)),
            DescriptionAttribute("Sets or retrieves a value that indicates whether the scroll bars are turned on or off."),
            CategoryAttribute("Behavior")]
            public string @scroll
            {
                get
                {

                    if (((IHTMLBodyElement)(e)).scroll == null) return NotSet;
                    return ((IHTMLBodyElement)(e)).scroll;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLBodyElement)(e)).scroll = value;
                }
            }
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the left margin for the entire body of the page, overriding the default margin."),
            CategoryAttribute("Layout")]
            public string @leftMargin
            {
                get
                {
                    if (((IHTMLBodyElement)(e)).leftMargin == null) return "";
                    return ((IHTMLBodyElement)(e)).leftMargin.ToString();
                }
                set
                {
                    ((IHTMLBodyElement)(e)).leftMargin = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the right margin for the entire body of the page."),
            CategoryAttribute("Layout")]
            public string @rightMargin
            {
                get
                {
                    if (((IHTMLBodyElement)(e)).rightMargin == null) return "";
                    return ((IHTMLBodyElement)(e)).rightMargin.ToString();
                }
                set
                {
                    ((IHTMLBodyElement)(e)).rightMargin = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the margin for the top of the page."),
            CategoryAttribute("Layout")]
            public string @topMargin
            {
                get
                {
                    if (((IHTMLBodyElement)(e)).topMargin == null) return "";
                    return ((IHTMLBodyElement)(e)).topMargin.ToString();
                }
                set
                {
                    ((IHTMLBodyElement)(e)).topMargin = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the bottom margin of the entire body of the page."),
            CategoryAttribute("Layout")]
            public string @bottomMargin
            {
                get
                {
                    if (((IHTMLBodyElement)(e)).bottomMargin == null) return "";
                    return ((IHTMLBodyElement)(e)).bottomMargin.ToString();
                }
                set
                {
                    ((IHTMLBodyElement)(e)).bottomMargin = value;
                }
            }
        }
        public class ButtonElement
        {
            private IHTMLElement e;
            public ButtonElement(IHTMLElement ele)
            {
                e = ele;
            }
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
            public string @accessKey
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "accessKey");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "accessKey", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((IHTMLButtonElement)(e)).disabled;
                }
                set
                {
                    ((IHTMLButtonElement)(e)).disabled = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
            public bool @hideFocus
            {
                get
                {
                    return ((HTMLButtonElement)(e)).hideFocus;
                }
                set
                {
                    ((HTMLButtonElement)(e)).hideFocus = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLButtonElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLButtonElement)(e)).tabIndex = value;
                }
            }

            [EditorAttribute(typeof(TextEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(TextEditorConverter)),
            DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
            public string onFocus
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onFocus");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onFocus", value);
                }
            }
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
            public string onKeyPress
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onKeyPress");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onKeyPress", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Sets Style Width."),
            CategoryAttribute("Style")]
            public string @style_width
            {
                get
                {
                    if (e.style == null || e.style.width == null) return "";
                    return e.style.width.ToString();
                }
                set
                {
                    e.style.width = value;
                }
            }
            [DescriptionAttribute("Sets Style Height."),
            CategoryAttribute("Style")]
            public string @style_height
            {
                get
                {
                    if (e.style == null || e.style.height == null) return "";
                    return e.style.height.ToString();
                }
                set
                {
                    e.style.height = value;
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves attribute tag for the object."), CategoryAttribute("Misc")]
            public string @tag
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "tag");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "tag", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves attribute @type for the object."), CategoryAttribute("Misc")]
            public string @type
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "type");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "type", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }

        public class ParaElement
        {
            private IHTMLElement e;
            public ParaElement(IHTMLElement ele)
            {
                e = ele;
            }
            [TypeConverter(typeof(alignDivConverter)),
            DescriptionAttribute("Sets or retrieves the alignment of the object relative to the display or table."),
            CategoryAttribute("Appearance")]
            public string @align
            {
                get
                {

                    if (((mshtml.IHTMLParaElement)(e)).align == null) return NotSet;
                    return ((IHTMLParaElement)(e)).align;

                }
                set
                {
                    if (NotSet.Equals(value)) value = null;
                    ((IHTMLParaElement)(e)).align = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the status of the object."),
            CategoryAttribute("Behavior")]
            public bool @disabled
            {
                get
                {
                    return ((HTMLParaElement)(e)).disabled;
                }
                set
                {
                    ((HTMLParaElement)(e)).disabled = value;
                }
            }

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
            public short @tabIndex
            {
                get
                {
                    return ((HTMLParaElement)(e)).tabIndex;
                }
                set
                {
                    ((HTMLParaElement)(e)).tabIndex = value;
                }
            }

            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
            public string onClick
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onClick");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onClick", value);
                }
            }

            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
            public string onMouseOut
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOut");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOut", value);
                }
            }
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
            CategoryAttribute("Events")]
            public string onMouseOver
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "onMouseOver");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "onMouseOver", value);
                }
            }
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
            public string @id
            {
                get
                {
                    return e.id;
                }
                set
                {
                    e.id = value;
                }
            }
            [DescriptionAttribute("Sets or retrieves the name of the object."), CategoryAttribute("Misc")]
            public string @name
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "name");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "name", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
            {
                get
                {
                    return PropertyAdapter.getEleAttr(e, "title");
                }
                set
                {
                    PropertyAdapter.setEleAttr(e, "title", value);
                }
            }
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
            public string @class
            {
                get
                {
                    return e.className;
                }
                set
                {
                    e.className = value;
                }
            }
            [EditorAttribute(typeof(StyleEditor), typeof(UITypeEditor)),
            TypeConverterAttribute(typeof(StyleEditorConverter)),
            DescriptionAttribute("Set style at Style Builder."),
            CategoryAttribute("Style")]
            public object @style
            {
                get
                {
                    return e;
                }
                set
                {

                }
            }
        }
    }
}


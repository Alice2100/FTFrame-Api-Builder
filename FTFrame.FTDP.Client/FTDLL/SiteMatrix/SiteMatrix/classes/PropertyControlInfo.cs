using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SiteMatrix.consts;
using SiteMatrix.PropertyBagNameSpace;
namespace SiteMatrix.PropertySpace.ControlInfo
{
	/// <summary>
	/// Employee is our sample business or domin object. It derives from the general base class Person.
	/// </summary>
	[TypeConverter(typeof(PartInfoConverter))]
	public class PartInfo
	{
		private string _PartCaption = "";
		private string _PartID = "";
		private string _PartName="";
		
		public PartInfo()
		{
		}
        [DescriptionAttribute("Part Caption"), ReadOnlyAttribute(true)]
		public string Caption
		{
			get { return _PartCaption; }
			set {_PartCaption=value;}
		}
		[DescriptionAttribute("Part ID"),ReadOnlyAttribute(true)]
		public string ID
		{
			get { return _PartID; }
			set {_PartID=value;}
		}
		[DescriptionAttribute("Part Name"),ReadOnlyAttribute(true)]
		public string Name
		{
			get { return _PartName; }
			set {_PartName=value;}
		}

		// Meaningful text representation
//		[ReadOnlyAttribute(true)]
//		public override string ToString()
//		{
//			StringBuilder sb = new StringBuilder();
//			sb.Append(this.Name);
//			sb.Append(",");
//			sb.Append(this.Caption);
//			sb.Append(",");
//			sb.Append(this.ID);
//			return sb.ToString();
//		}
	}
	internal class PartInfoConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
		{
			if( destType == typeof(string) && value is PartInfo )
			{
				// Cast the value to an Employee type
				PartInfo emp = (PartInfo)value;

				// Return department and department role separated by comma.
				return "";
			}
			return base.ConvertTo(context,culture,value,destType);
		}
	}
	[TypeConverter(typeof(ControlInfoConverter))]
	public class ControlInfo
	{
		private string _CanPublish = "";
		private string _CanCheck = "";
		private string _Caption="";
		private string _Category="";
		private string _Company="";
		private string _Copyright="";
		private string _Description="";
		private string _Name="";
		private string _Version="";
		private string _DeployTime="";
		private string _ControlID="";

		public ControlInfo()
		{
		}
        [DescriptionAttribute("Whether this control can publish content."), ReadOnlyAttribute(true)]
		public string CanPublish
		{
			get { return _CanPublish; }
			set {_CanPublish=value;}
		}
		[DescriptionAttribute("Control instance ID"),ReadOnlyAttribute(true)]
		public string ControlID
		{
			get { return _ControlID; }
			set {_ControlID=value;}
		}
        [DescriptionAttribute("Whether this control can check content."), ReadOnlyAttribute(true)]
		public string CanCheck
		{
			get { return _CanCheck; }
			set {_CanCheck=value;}
		}
		[DescriptionAttribute("Control caption"),ReadOnlyAttribute(true)]
		public string Caption
		{
			get { return _Caption; }
			set {_Caption=value;}
		}
        [DescriptionAttribute("Control category"), ReadOnlyAttribute(true)]
		public string Category
		{
			get { return _Category; }
			set {_Category=value;}
		}
		[DescriptionAttribute("Control's company"),ReadOnlyAttribute(true)]
		public string Company
		{
			get { return _Company; }
			set {_Company=value;}
		}
        [DescriptionAttribute("Control's copyright"), ReadOnlyAttribute(true)]
		public string Copyright
		{
			get { return _Copyright; }
			set {_Copyright=value;}
		}
        [DescriptionAttribute("Control's description"), ReadOnlyAttribute(true)]
		public string Description
		{
			get { return _Description; }
			set {_Description=value;}
		}
		[DescriptionAttribute("Control's name"),ReadOnlyAttribute(true)]
		public string Name
		{
			get { return _Name; }
			set {_Name=value;}
		}
        [DescriptionAttribute("Control's version"), ReadOnlyAttribute(true)]
		public string Version
		{
			get { return _Version; }
			set {_Version=value;}
		}
		[DescriptionAttribute("Control's deploy time"),ReadOnlyAttribute(true)]
		public string DeployTime
		{
			get { return _DeployTime; }
			set {_DeployTime=value;}
		}

		// Meaningful text representation
//		[ReadOnlyAttribute(true)]
//		public override string ToString()
//		{
//			StringBuilder sb = new StringBuilder();
//			sb.Append(this.Name);
//			sb.Append(",");
//			sb.Append(this.Caption);
//			sb.Append(",");
//			sb.Append(this.ID);
//			return sb.ToString();
//		}
	}
	internal class ControlInfoConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
		{
			if( destType == typeof(string) && value is ControlInfo )
			{
				// Cast the value to an Employee type
				ControlInfo emp = (ControlInfo)value;

				// Return department and department role separated by comma.
				return "";
			}
			return base.ConvertTo(context,culture,value,destType);
		}
	}
	internal class PartsConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
		{
			if( destType == typeof(string) && value is PropertyTable )
			{
				// Cast the value to an Employee type
				PropertyTable emp = (PropertyTable)value;

				// Return department and department role separated by comma.
				return "";
			}
			return base.ConvertTo(context,culture,value,destType);
		}
	}
}
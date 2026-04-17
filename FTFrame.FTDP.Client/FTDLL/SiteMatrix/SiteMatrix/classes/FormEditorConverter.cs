//builder by maobb,2008-3-2
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using SiteMatrix.forms;
using mshtml;
using SiteMatrix.Page;
using SiteMatrix.classes;
using SiteMatrix.Adapter;
using SiteMatrix.functions;
using SiteMatrix.consts;
using SiteMatrix.database;

namespace SiteMatrix.classes
{
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class FormTextBoxType : StringConverter
    {
        public FormTextBoxType()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { res.form.GetString("String48"), res.form.GetString("String49"), res.form.GetString("String50") });
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
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class FormStatFilterType : StringConverter
    {
        public FormStatFilterType()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { res.form.GetString("String65"), res.form.GetString("String66")});
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
}

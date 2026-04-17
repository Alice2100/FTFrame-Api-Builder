using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FTFrame.Web.Core.Pages
{
    public class AaModel : PageModel
    {
        public void OnGet()
        {
            Msg = "Msg Msg";
        }
        public string Msg
        { get; set; }
    }
}

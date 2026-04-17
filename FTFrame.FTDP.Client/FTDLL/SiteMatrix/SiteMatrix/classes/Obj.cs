using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteMatrix.Obj
{
    public class ComboItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}

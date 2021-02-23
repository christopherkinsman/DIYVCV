using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIYVCV.Models.ViewModels
{
    public class ShowModule
    {
        public ModuleDto module { get; set; }

        public IEnumerable<ComponentDto> component { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIYVCV.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleBrand { get; set; }
        public string ModuleCategory { get; set; }
        public string ModuleDescription { get; set; }
        public string ModuleLink { get; set; }
        public string ModuleSchematic { get; set; }
        public bool ModuleHasPic { get; set; }
        //Record the extension of a Module's image (.png, .jpg)
        public string PicExtension { get; set; }
    }

    public class ModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleBrand { get; set; }
        public string ModuleCategory { get; set; }
        public string ModuleDescription { get; set; }
        public string ModuleLink { get; set; }
        public string ModuleSchematic { get; set; }
        public bool ModuleHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}
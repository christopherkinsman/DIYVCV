using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIYVCV.Models
{
    public class Component
    {
        [Key]
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentValue { get; set; }
        public string ModuleId { get; set; }
        public string ComponentQuantity { get; set; }
    }

    public class ComponentDto
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentValue { get; set; }
        public string ModuleId { get; set; }
        public string ComponentQuantity { get; set; }
    }
}
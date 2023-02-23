using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class FormMasters
    {
        public IEnumerable <FormSections> sections { get; set; }
        public IEnumerable <FormSections> types { get; set; }
    }
}

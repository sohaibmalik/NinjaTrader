using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
using System.Diagnostics;
using System.ComponentModel;

namespace AlsiUtils
{
    public partial class  AlsiDBDataContext
    {
        public  AlsiDBDataContext():base(AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString,mappingSource )
        {
            OnCreated();
        }
    }

    public partial class MasterMinute
    {
        public override string ToString()
        {
            return Stamp + " " + "O:" + O + " H:" + H + " L:" + L + " C:" + C;
        }
    }

}
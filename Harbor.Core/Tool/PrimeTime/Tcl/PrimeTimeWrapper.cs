using System;
using System.Collections.Generic;
using System.Text;
using Harbor.Core.Tool.PrimeTime.Model;

namespace Harbor.Core.Tool.PrimeTime.Tcl
{
    public partial class PrimeTime
    {
        private PrimeTimeModel model;

        public PrimeTime(PrimeTimeModel model)
        {
            this.model = model;
        }
    }
}

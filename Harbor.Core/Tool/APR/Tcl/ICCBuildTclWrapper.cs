using System;
using System.Collections.Generic;
using System.Text;
using Harbor.Core.Tool.APR.Model;

namespace Harbor.Core.Tool.APR.Tcl
{
    public partial class ICCBuildTcl
    {
        private BuildTclModel model;
        private PinPadTclModel pinModel;

        public ICCBuildTcl(BuildTclModel model, PinPadTclModel pinModel)
        {
            this.model = model;
            this.pinModel = pinModel;
        }
    }
}

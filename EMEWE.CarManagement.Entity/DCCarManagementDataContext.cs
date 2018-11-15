using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMEWE.CarManagement.Entity
{
    public partial class DCCarManagementDataContext
    {
        public DCCarManagementDataContext() :
            base(System.Configuration.ConfigurationManager.ConnectionStrings["EMEWEQCConnectionString"].ConnectionString)
        {
            OnCreated();
        }
    }
}

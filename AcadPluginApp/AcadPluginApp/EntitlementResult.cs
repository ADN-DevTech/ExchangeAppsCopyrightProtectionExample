using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcadPluginApp
{
    class EntitlementResult
    {
        public string UserId { get; set; }
        public string AppId { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }

    }
}

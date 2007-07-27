using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Service.Versioning
{
    public interface IVersionableContent
    {
       VersioningInfo GetVersioningInfo();
    }
}

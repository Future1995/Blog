using System;
using System.Collections.Generic;
using System.Text;

namespace HwaTec.Blog.Model
{
    public interface IEntityBase
    {
        Guid Sysid { get; set; }
    }
}

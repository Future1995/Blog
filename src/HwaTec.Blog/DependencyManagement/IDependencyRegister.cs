using System.Collections.Generic;
using System.Text;

namespace HwaTec.Blog.DependencyManagement
{
    public interface IDependencyRegister
    {
        ExecuteOrderType ExecuteOrder { get; }
    }

    /// <summary>
    /// Auto Register Interface
    /// </summary>
    public interface IScopedDependency
    {

    }
    /// <summary>
    /// Auto Register Interface
    /// </summary>
    public interface ITransistDependency
    {

    }
    /// <summary>
    /// Auto Register Interface
    /// </summary>
    public interface ISignaltonDependency
    {

    }
}

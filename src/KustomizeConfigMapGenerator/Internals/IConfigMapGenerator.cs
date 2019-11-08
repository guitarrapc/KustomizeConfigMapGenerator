using System.Threading;
using System.Threading.Tasks;

namespace KustomizeConfigMapGenerator.Internals
{
    internal interface IConfigMapGenerator
    {
        /// <summary>
        /// ConfigMapGenerator Config Name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ConfigMapGenerator Behavior
        /// </summary>
        Behavior Behavior { get; }
        /// <summary>
        /// Append will start from ConfigMapGenerator config name. Ignore ConfigMapGenerator Header.
        /// </summary>
        bool Append { get; }
        Task WriteAsync(string contents, string outputPath, bool force, CancellationToken cancellationToken = default);
    }
}

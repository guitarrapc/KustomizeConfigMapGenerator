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
        /// Ignore ConfigMapGenerator Header.
        /// </summary>
        bool SkipHeader { get; }
        /// <summary>
        /// Write configMapGenrator.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="outputPath"></param>
        /// <param name="force"></param>
        /// <param name="append"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteAsync(string contents, string outputPath, bool force, bool append, CancellationToken cancellationToken = default);
    }
}

namespace PDFtoPrinter
{
    /// <summary>
    /// Factory that creates <see cref="IProcess"/> instances.
    /// </summary>
    public interface IProcessFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IProcess"/> with given arguments.
        /// </summary>
        /// <param name="executablePath">Path to a ".exe" file.</param>
        /// <param name="options">CLI arguments.</param>
        /// <returns>Corresponding <see cref="IProcess"/> instance.</returns>
        IProcess Create(string executablePath, PrintingOptions options);
    }
}

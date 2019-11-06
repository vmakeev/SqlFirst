using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using SqlFirst.VisualStudio.Integration.Commands;
using SqlFirst.VisualStudio.Integration.Helpers;
using SqlFirst.VisualStudio.Integration.Logging;

namespace SqlFirst.VisualStudio.Integration.VSPackage
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in
	/// .vsixmanifest file.
	/// </para>
	/// </remarks>
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuidString)]
	[ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	public sealed class SqlFirstPackage : Package
	{
		/// <summary>
		/// GenerateQueryObjectsPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "3d750cc8-b827-48fb-ad38-903c0c2e6fd7";

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateQueryObjectsFromItems" /> class.
		/// </summary>
		public SqlFirstPackage()
		{
			CommonLoggingConfiguration.EnsureOutputEnabled();
			SqlFirstErrorsWindow.InitInstance(this);
		}

		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			GenerateQueryObjectsFromItems.Initialize(this);
			GenerateQueryObjectFromFolder.Initialize(this);
			BeautifySqlFile.Initialize(this);
			AddSqlFirstOptions.Initialize(this);
		}

		#endregion
	}
}
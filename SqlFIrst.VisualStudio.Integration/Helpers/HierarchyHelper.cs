using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SqlFIrst.VisualStudio.Integration.Helpers
{
	internal static class HierarchyHelper
	{
		private static object _cachedService;

		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
		public static IVsHierarchy GetIVsHierarchy(this Project project)
		{
			var applicationObject = (DTE2)Package.GetGlobalService(typeof(DTE));
			var serviceProvider = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)applicationObject;
			var solutionService = (IVsSolution)GetService(serviceProvider, typeof(SVsSolution), typeof(IVsSolution));

			if (solutionService != null && solutionService.GetProjectOfUniqueName(project.UniqueName, out IVsHierarchy projectHierarchy) == 0)
			{
				return projectHierarchy;
			}

			return null;
		}

		private static object GetService(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider,
			Type serviceType,
			Type interfaceType)
		{
			if (_cachedService != null)
			{
				return _cachedService;
			}
			object service = null;

			Guid serviceGuid = serviceType.GUID;
			Guid interfaceGuid = interfaceType.GUID;

			int hr = serviceProvider.QueryService(ref serviceGuid, ref interfaceGuid, out IntPtr servicePointer);
			if (hr != 0)
			{
				Marshal.ThrowExceptionForHR(hr);
			}
			else if (servicePointer != IntPtr.Zero)
			{
				service = Marshal.GetObjectForIUnknown(servicePointer);
				Marshal.Release(servicePointer);
			}

			return _cachedService = service;
		}
	}
}

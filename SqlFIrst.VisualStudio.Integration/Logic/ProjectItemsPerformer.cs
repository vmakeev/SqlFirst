using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using EnvDTE;
using SqlFirst.VisualStudio.Integration.Helpers;
using SqlFIrst.VisualStudio.Integration.Helpers;
using SqlFIrst.VisualStudio.Integration.Logic;

namespace SqlFirst.VisualStudio.Integration.Logic
{
	internal class ProjectItemsPerformer
	{
		private readonly SqlFirstErrorsWindow _errorWindow;
		private readonly SqlFirstStatusBar _statusBar;

		private readonly ILog _log = LogManager.GetLogger<ProjectItemsPerformer>();

		/// <inheritdoc />
		public ProjectItemsPerformer(IServiceProvider serviceProvider)
		{
			_errorWindow = new SqlFirstErrorsWindow(serviceProvider);
			_statusBar = new SqlFirstStatusBar(serviceProvider);
		}

		public Task ProcessItemsAsync(IEnumerable<ProjectItem> selected, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(() => ProcessItems(selected), cancellationToken);
		}

		public void ProcessItems(IEnumerable<ProjectItem> selected)
		{
			_errorWindow.ClearErrors();

			var errors = new LinkedList<(ProjectItem, Exception)>();
			var projects = new List<Project>();

			int objectsCount = 0;
			_statusBar.Display("SqlFirst generation started...");
			foreach (ProjectItem projectItem in selected)
			{
				try
				{
					Performer.Perform(projectItem);
					objectsCount++;
				}
				catch (Exception ex)
				{
					_log.Error(ex);
					errors.AddLast((projectItem, ex));
				}

				if (projectItem.ContainingProject != null && !projects.Contains(projectItem.ContainingProject))
				{
					projects.Add(projectItem.ContainingProject);
				}
			}

			_statusBar.Display($"SqlFirst generation successful, {objectsCount} queries processed");

			foreach (Project project in projects.Where(project => project.IsDirty))
			{
				project.Save();
			}

			if (errors.Any())
			{
				_statusBar.Display("SqlFirst generation failed");
				_errorWindow.DisplayErrors(errors);
			}
		}
	}
}

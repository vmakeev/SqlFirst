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
			_errorWindow = SqlFirstErrorsWindow.Instance;
			_statusBar = new SqlFirstStatusBar(serviceProvider);
		}

		public Task GenerateQueryObjectsAsync(IEnumerable<ProjectItem> selected, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(() => GenerateQueryObjects(selected), cancellationToken);
		}

		public void GenerateQueryObjects(IEnumerable<ProjectItem> selected)
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
					Performer.GenerateObjects(projectItem);
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

			foreach (Project project in projects.Where(project => project.IsDirty))
			{
				project.Save();
			}

			if (errors.Any())
			{
				_statusBar.Display("SqlFirst generation failed");
				_errorWindow.DisplayErrors(errors);
			}
			else
			{
				_statusBar.Display($"SqlFirst generation successful, {objectsCount} queries processed");
			}
		}

		public void BeautifyFiles(IEnumerable<ProjectItem> items)
		{
			_errorWindow.ClearErrors();

			var errors = new LinkedList<(ProjectItem, Exception)>();

			int objectsCount = 0;
			_statusBar.Display("SqlFirst format files started...");
			foreach (ProjectItem projectItem in items)
			{
				try
				{
					Performer.BeautifyFile(projectItem);
					objectsCount++;
				}
				catch (Exception ex)
				{
					_log.Error(ex);
					errors.AddLast((projectItem, ex));
				}
			}

			if (errors.Any())
			{
				_statusBar.Display("SqlFirst format files failed");
				_errorWindow.DisplayErrors(errors);
			}
			else
			{
				_statusBar.Display($"SqlFirst format files successful, {objectsCount} queries processed");
			}
		}
	}
}

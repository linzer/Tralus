namespace Tralus.Framework.Module {
	partial class FrameworkModule {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            // 
            // FrameworkModule
            // 
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Localization.PreviewControlLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.PivotGrid.PivotGridListEditorLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.PivotChart.PivotGridLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.PivotChart.ChartLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.PivotChart.AnalysisControlLocalizer));

		}

		#endregion
	}
}

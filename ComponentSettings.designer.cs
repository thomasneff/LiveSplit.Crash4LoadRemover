﻿namespace LiveSplit.UI.Components
{
	partial class CTRNitroFueledLoadRemoverSettings
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.lblVersion = new System.Windows.Forms.Label();
      this.processListComboBox = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.previewPictureBox = new System.Windows.Forms.PictureBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.croppedPreviewPictureBox = new System.Windows.Forms.PictureBox();
      this.scalingLabel = new System.Windows.Forms.Label();
      this.trackBar1 = new System.Windows.Forms.TrackBar();
      this.matchDisplayLabel = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.requiredMatchesUpDown = new System.Windows.Forms.NumericUpDown();
      this.panel1 = new System.Windows.Forms.Panel();
      this.saveDiagnosticsButton = new System.Windows.Forms.Button();
      this.updatePreviewButton = new System.Windows.Forms.Button();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.lblBlackLevel = new System.Windows.Forms.Label();
      this.chkRemoveFadeIns = new System.Windows.Forms.CheckBox();
      this.chkSaveDetectionLog = new System.Windows.Forms.CheckBox();
      this.chkRemoveTransitions = new System.Windows.Forms.CheckBox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.chkAutoSplitterDisableOnSkip = new System.Windows.Forms.CheckBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.autoSplitNameLbl = new System.Windows.Forms.Label();
      this.autoSplitCategoryLbl = new System.Windows.Forms.Label();
      this.enableAutoSplitterChk = new System.Windows.Forms.CheckBox();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.devToolsDataBaseFromCaptureImages = new System.Windows.Forms.Button();
      this.label10 = new System.Windows.Forms.Label();
      this.devToolsCaptureImageText = new System.Windows.Forms.TextBox();
      this.devToolsDatabaseButton = new System.Windows.Forms.Button();
      this.devToolsRecord = new System.Windows.Forms.CheckBox();
      this.label9 = new System.Windows.Forms.Label();
      this.devToolsCropY = new System.Windows.Forms.NumericUpDown();
      this.label8 = new System.Windows.Forms.Label();
      this.devToolsCropX = new System.Windows.Forms.NumericUpDown();
      this.label7 = new System.Windows.Forms.Label();
      this.devToolsCroppedPictureBox = new System.Windows.Forms.PictureBox();
      this.label12 = new System.Windows.Forms.Label();
      this.cmbDatabase = new System.Windows.Forms.ComboBox();
      ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.requiredMatchesUpDown)).BeginInit();
      this.panel1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.devToolsCropY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.devToolsCropX)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.devToolsCroppedPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // lblVersion
      // 
      this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblVersion.AutoSize = true;
      this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlDark;
      this.lblVersion.Location = new System.Drawing.Point(431, 490);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(37, 13);
      this.lblVersion.TabIndex = 21;
      this.lblVersion.Text = "v0.0.0";
      this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // processListComboBox
      // 
      this.processListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.processListComboBox.FormattingEnabled = true;
      this.processListComboBox.Items.AddRange(new object[] {
            "Full Display (Primary)"});
      this.processListComboBox.Location = new System.Drawing.Point(86, 6);
      this.processListComboBox.Name = "processListComboBox";
      this.processListComboBox.Size = new System.Drawing.Size(345, 21);
      this.processListComboBox.TabIndex = 22;
      this.processListComboBox.DropDown += new System.EventHandler(this.processListComboBox_DropDown);
      this.processListComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(28, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(47, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "Capture:";
      // 
      // previewPictureBox
      // 
      this.previewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.previewPictureBox.Location = new System.Drawing.Point(30, 163);
      this.previewPictureBox.Name = "previewPictureBox";
      this.previewPictureBox.Size = new System.Drawing.Size(400, 180);
      this.previewPictureBox.TabIndex = 24;
      this.previewPictureBox.TabStop = false;
      this.previewPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseClick);
      this.previewPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseDown);
      this.previewPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseMove);
      this.previewPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseUp);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(26, 129);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(121, 16);
      this.label2.TabIndex = 25;
      this.label2.Text = "Capture Preview";
      this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label11.Location = new System.Drawing.Point(27, 148);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(304, 12);
      this.label11.TabIndex = 28;
      this.label11.Text = "Left Click sets top-left corner, right click sets bottom-right corner of region";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(27, 346);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(185, 16);
      this.label4.TabIndex = 30;
      this.label4.Text = "Cropped Capture Preview";
      this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // croppedPreviewPictureBox
      // 
      this.croppedPreviewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.croppedPreviewPictureBox.Location = new System.Drawing.Point(30, 365);
      this.croppedPreviewPictureBox.Name = "croppedPreviewPictureBox";
      this.croppedPreviewPictureBox.Size = new System.Drawing.Size(300, 135);
      this.croppedPreviewPictureBox.TabIndex = 29;
      this.croppedPreviewPictureBox.TabStop = false;
      // 
      // scalingLabel
      // 
      this.scalingLabel.AutoSize = true;
      this.scalingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.scalingLabel.Location = new System.Drawing.Point(148, 3);
      this.scalingLabel.Name = "scalingLabel";
      this.scalingLabel.Size = new System.Drawing.Size(92, 16);
      this.scalingLabel.TabIndex = 32;
      this.scalingLabel.Text = "Scaling: 100%";
      // 
      // trackBar1
      // 
      this.trackBar1.LargeChange = 25;
      this.trackBar1.Location = new System.Drawing.Point(134, 20);
      this.trackBar1.Margin = new System.Windows.Forms.Padding(2);
      this.trackBar1.Maximum = 201;
      this.trackBar1.Minimum = 100;
      this.trackBar1.Name = "trackBar1";
      this.trackBar1.Size = new System.Drawing.Size(106, 45);
      this.trackBar1.SmallChange = 25;
      this.trackBar1.TabIndex = 31;
      this.trackBar1.TickFrequency = 25;
      this.trackBar1.Value = 100;
      this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
      // 
      // matchDisplayLabel
      // 
      this.matchDisplayLabel.AutoSize = true;
      this.matchDisplayLabel.Location = new System.Drawing.Point(21, 23);
      this.matchDisplayLabel.Name = "matchDisplayLabel";
      this.matchDisplayLabel.Size = new System.Drawing.Size(13, 13);
      this.matchDisplayLabel.TabIndex = 33;
      this.matchDisplayLabel.Text = "0";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label5.Location = new System.Drawing.Point(3, 3);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(116, 16);
      this.label5.TabIndex = 34;
      this.label5.Text = "Current / Required";
      // 
      // requiredMatchesUpDown
      // 
      this.requiredMatchesUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.requiredMatchesUpDown.Location = new System.Drawing.Point(67, 21);
      this.requiredMatchesUpDown.Maximum = new decimal(new int[] {
            575,
            0,
            0,
            0});
      this.requiredMatchesUpDown.Name = "requiredMatchesUpDown";
      this.requiredMatchesUpDown.Size = new System.Drawing.Size(42, 20);
      this.requiredMatchesUpDown.TabIndex = 36;
      this.requiredMatchesUpDown.Value = new decimal(new int[] {
            520,
            0,
            0,
            0});
      this.requiredMatchesUpDown.ValueChanged += new System.EventHandler(this.requiredMatchesUpDown_ValueChanged);
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.SystemColors.Control;
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panel1.Controls.Add(this.saveDiagnosticsButton);
      this.panel1.Controls.Add(this.updatePreviewButton);
      this.panel1.Controls.Add(this.label5);
      this.panel1.Controls.Add(this.requiredMatchesUpDown);
      this.panel1.Controls.Add(this.scalingLabel);
      this.panel1.Controls.Add(this.matchDisplayLabel);
      this.panel1.Controls.Add(this.trackBar1);
      this.panel1.Location = new System.Drawing.Point(29, 72);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(400, 52);
      this.panel1.TabIndex = 37;
      // 
      // saveDiagnosticsButton
      // 
      this.saveDiagnosticsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.saveDiagnosticsButton.Location = new System.Drawing.Point(251, 8);
      this.saveDiagnosticsButton.Name = "saveDiagnosticsButton";
      this.saveDiagnosticsButton.Size = new System.Drawing.Size(65, 33);
      this.saveDiagnosticsButton.TabIndex = 38;
      this.saveDiagnosticsButton.Text = "Save Diagnostics";
      this.saveDiagnosticsButton.UseVisualStyleBackColor = true;
      this.saveDiagnosticsButton.Click += new System.EventHandler(this.saveDiagnosticsButton_Click);
      // 
      // updatePreviewButton
      // 
      this.updatePreviewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.updatePreviewButton.Location = new System.Drawing.Point(324, 8);
      this.updatePreviewButton.Name = "updatePreviewButton";
      this.updatePreviewButton.Size = new System.Drawing.Size(65, 33);
      this.updatePreviewButton.TabIndex = 37;
      this.updatePreviewButton.Text = "Update Preview";
      this.updatePreviewButton.UseVisualStyleBackColor = true;
      this.updatePreviewButton.Click += new System.EventHandler(this.updatePreviewButton_Click);
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Controls.Add(this.tabPage3);
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(476, 532);
      this.tabControl1.TabIndex = 38;
      // 
      // tabPage1
      // 
      this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
      this.tabPage1.Controls.Add(this.label12);
      this.tabPage1.Controls.Add(this.cmbDatabase);
      this.tabPage1.Controls.Add(this.lblBlackLevel);
      this.tabPage1.Controls.Add(this.chkRemoveFadeIns);
      this.tabPage1.Controls.Add(this.chkSaveDetectionLog);
      this.tabPage1.Controls.Add(this.chkRemoveTransitions);
      this.tabPage1.Controls.Add(this.panel1);
      this.tabPage1.Controls.Add(this.lblVersion);
      this.tabPage1.Controls.Add(this.label4);
      this.tabPage1.Controls.Add(this.label1);
      this.tabPage1.Controls.Add(this.croppedPreviewPictureBox);
      this.tabPage1.Controls.Add(this.processListComboBox);
      this.tabPage1.Controls.Add(this.label11);
      this.tabPage1.Controls.Add(this.previewPictureBox);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(468, 506);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Setup";
      // 
      // lblBlackLevel
      // 
      this.lblBlackLevel.AutoSize = true;
      this.lblBlackLevel.Location = new System.Drawing.Point(376, 131);
      this.lblBlackLevel.Name = "lblBlackLevel";
      this.lblBlackLevel.Size = new System.Drawing.Size(78, 13);
      this.lblBlackLevel.TabIndex = 41;
      this.lblBlackLevel.Text = "Black-Level: -1";
      this.lblBlackLevel.Visible = false;
      // 
      // chkRemoveFadeIns
      // 
      this.chkRemoveFadeIns.AutoSize = true;
      this.chkRemoveFadeIns.Checked = true;
      this.chkRemoveFadeIns.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkRemoveFadeIns.Location = new System.Drawing.Point(346, 145);
      this.chkRemoveFadeIns.Name = "chkRemoveFadeIns";
      this.chkRemoveFadeIns.Size = new System.Drawing.Size(109, 17);
      this.chkRemoveFadeIns.TabIndex = 40;
      this.chkRemoveFadeIns.Text = "Remove Fade-ins";
      this.chkRemoveFadeIns.UseVisualStyleBackColor = true;
      this.chkRemoveFadeIns.Visible = false;
      this.chkRemoveFadeIns.CheckedChanged += new System.EventHandler(this.chkRemoveFadeIns_CheckedChanged);
      // 
      // chkSaveDetectionLog
      // 
      this.chkSaveDetectionLog.AutoSize = true;
      this.chkSaveDetectionLog.Checked = true;
      this.chkSaveDetectionLog.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkSaveDetectionLog.Location = new System.Drawing.Point(151, 130);
      this.chkSaveDetectionLog.Name = "chkSaveDetectionLog";
      this.chkSaveDetectionLog.Size = new System.Drawing.Size(93, 17);
      this.chkSaveDetectionLog.TabIndex = 39;
      this.chkSaveDetectionLog.Text = "Detection Log";
      this.chkSaveDetectionLog.UseVisualStyleBackColor = true;
      this.chkSaveDetectionLog.CheckedChanged += new System.EventHandler(this.chkSaveDetectionLog_CheckedChanged);
      // 
      // chkRemoveTransitions
      // 
      this.chkRemoveTransitions.AutoSize = true;
      this.chkRemoveTransitions.Checked = true;
      this.chkRemoveTransitions.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkRemoveTransitions.Location = new System.Drawing.Point(250, 130);
      this.chkRemoveTransitions.Name = "chkRemoveTransitions";
      this.chkRemoveTransitions.Size = new System.Drawing.Size(120, 17);
      this.chkRemoveTransitions.TabIndex = 38;
      this.chkRemoveTransitions.Text = "Remove Transitions";
      this.chkRemoveTransitions.UseVisualStyleBackColor = true;
      this.chkRemoveTransitions.CheckedChanged += new System.EventHandler(this.chkRemoveTransitions_CheckedChanged);
      // 
      // tabPage2
      // 
      this.tabPage2.AutoScroll = true;
      this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
      this.tabPage2.Controls.Add(this.chkAutoSplitterDisableOnSkip);
      this.tabPage2.Controls.Add(this.label6);
      this.tabPage2.Controls.Add(this.label3);
      this.tabPage2.Controls.Add(this.autoSplitNameLbl);
      this.tabPage2.Controls.Add(this.autoSplitCategoryLbl);
      this.tabPage2.Controls.Add(this.enableAutoSplitterChk);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(468, 506);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "AutoSplitter";
      // 
      // chkAutoSplitterDisableOnSkip
      // 
      this.chkAutoSplitterDisableOnSkip.AutoSize = true;
      this.chkAutoSplitterDisableOnSkip.Location = new System.Drawing.Point(150, 9);
      this.chkAutoSplitterDisableOnSkip.Name = "chkAutoSplitterDisableOnSkip";
      this.chkAutoSplitterDisableOnSkip.Size = new System.Drawing.Size(239, 17);
      this.chkAutoSplitterDisableOnSkip.TabIndex = 43;
      this.chkAutoSplitterDisableOnSkip.Text = "Disable AutoSplitter on Skip until manual Split";
      this.chkAutoSplitterDisableOnSkip.UseVisualStyleBackColor = true;
      this.chkAutoSplitterDisableOnSkip.CheckedChanged += new System.EventHandler(this.chkAutoSplitterDisableOnSkip_CheckedChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
      this.label6.Location = new System.Drawing.Point(251, 75);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(188, 16);
      this.label6.TabIndex = 42;
      this.label6.Text = "Number of Loads per Split";
      this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
      this.label3.Location = new System.Drawing.Point(25, 75);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(51, 16);
      this.label3.TabIndex = 41;
      this.label3.Text = "Splits:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // autoSplitNameLbl
      // 
      this.autoSplitNameLbl.AutoSize = true;
      this.autoSplitNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
      this.autoSplitNameLbl.Location = new System.Drawing.Point(24, 29);
      this.autoSplitNameLbl.Name = "autoSplitNameLbl";
      this.autoSplitNameLbl.Size = new System.Drawing.Size(49, 16);
      this.autoSplitNameLbl.TabIndex = 40;
      this.autoSplitNameLbl.Text = "Name";
      this.autoSplitNameLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // autoSplitCategoryLbl
      // 
      this.autoSplitCategoryLbl.AutoSize = true;
      this.autoSplitCategoryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
      this.autoSplitCategoryLbl.Location = new System.Drawing.Point(24, 49);
      this.autoSplitCategoryLbl.Name = "autoSplitCategoryLbl";
      this.autoSplitCategoryLbl.Size = new System.Drawing.Size(71, 16);
      this.autoSplitCategoryLbl.TabIndex = 39;
      this.autoSplitCategoryLbl.Text = "Category";
      this.autoSplitCategoryLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // enableAutoSplitterChk
      // 
      this.enableAutoSplitterChk.AutoSize = true;
      this.enableAutoSplitterChk.Location = new System.Drawing.Point(28, 9);
      this.enableAutoSplitterChk.Name = "enableAutoSplitterChk";
      this.enableAutoSplitterChk.Size = new System.Drawing.Size(116, 17);
      this.enableAutoSplitterChk.TabIndex = 0;
      this.enableAutoSplitterChk.Text = "Enable AutoSplitter";
      this.enableAutoSplitterChk.UseVisualStyleBackColor = true;
      this.enableAutoSplitterChk.CheckedChanged += new System.EventHandler(this.enableAutoSplitterChk_CheckedChanged);
      // 
      // tabPage3
      // 
      this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
      this.tabPage3.Controls.Add(this.devToolsDataBaseFromCaptureImages);
      this.tabPage3.Controls.Add(this.label10);
      this.tabPage3.Controls.Add(this.devToolsCaptureImageText);
      this.tabPage3.Controls.Add(this.devToolsDatabaseButton);
      this.tabPage3.Controls.Add(this.devToolsRecord);
      this.tabPage3.Controls.Add(this.label9);
      this.tabPage3.Controls.Add(this.devToolsCropY);
      this.tabPage3.Controls.Add(this.label8);
      this.tabPage3.Controls.Add(this.devToolsCropX);
      this.tabPage3.Controls.Add(this.label7);
      this.tabPage3.Controls.Add(this.devToolsCroppedPictureBox);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new System.Drawing.Size(468, 506);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Developer Tools (caution!)";
      // 
      // devToolsDataBaseFromCaptureImages
      // 
      this.devToolsDataBaseFromCaptureImages.Location = new System.Drawing.Point(154, 269);
      this.devToolsDataBaseFromCaptureImages.Name = "devToolsDataBaseFromCaptureImages";
      this.devToolsDataBaseFromCaptureImages.Size = new System.Drawing.Size(158, 36);
      this.devToolsDataBaseFromCaptureImages.TabIndex = 45;
      this.devToolsDataBaseFromCaptureImages.Text = "Compute Database";
      this.devToolsDataBaseFromCaptureImages.UseVisualStyleBackColor = true;
      this.devToolsDataBaseFromCaptureImages.Click += new System.EventHandler(this.devToolsDataBaseFromCaptureImages_Click);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label10.Location = new System.Drawing.Point(6, 259);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(142, 16);
      this.label10.TabIndex = 44;
      this.label10.Text = "CaptureImages Folder";
      // 
      // devToolsCaptureImageText
      // 
      this.devToolsCaptureImageText.Location = new System.Drawing.Point(7, 278);
      this.devToolsCaptureImageText.Name = "devToolsCaptureImageText";
      this.devToolsCaptureImageText.Size = new System.Drawing.Size(141, 20);
      this.devToolsCaptureImageText.TabIndex = 43;
      this.devToolsCaptureImageText.Text = "CaptureImages";
      // 
      // devToolsDatabaseButton
      // 
      this.devToolsDatabaseButton.Location = new System.Drawing.Point(308, 464);
      this.devToolsDatabaseButton.Name = "devToolsDatabaseButton";
      this.devToolsDatabaseButton.Size = new System.Drawing.Size(158, 36);
      this.devToolsDatabaseButton.TabIndex = 42;
      this.devToolsDatabaseButton.Text = "Compute Database";
      this.devToolsDatabaseButton.UseVisualStyleBackColor = true;
      this.devToolsDatabaseButton.Click += new System.EventHandler(this.devToolsDatabaseButton_Click);
      // 
      // devToolsRecord
      // 
      this.devToolsRecord.AutoSize = true;
      this.devToolsRecord.Location = new System.Drawing.Point(112, 359);
      this.devToolsRecord.Name = "devToolsRecord";
      this.devToolsRecord.Size = new System.Drawing.Size(98, 17);
      this.devToolsRecord.TabIndex = 41;
      this.devToolsRecord.Text = "Record Images";
      this.devToolsRecord.UseVisualStyleBackColor = true;
      this.devToolsRecord.CheckedChanged += new System.EventHandler(this.devToolsRecord_CheckedChanged);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label9.Location = new System.Drawing.Point(57, 340);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(49, 16);
      this.label9.TabIndex = 39;
      this.label9.Text = "Crop Y";
      // 
      // devToolsCropY
      // 
      this.devToolsCropY.Location = new System.Drawing.Point(60, 359);
      this.devToolsCropY.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.devToolsCropY.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
      this.devToolsCropY.Name = "devToolsCropY";
      this.devToolsCropY.Size = new System.Drawing.Size(42, 20);
      this.devToolsCropY.TabIndex = 40;
      this.devToolsCropY.Value = new decimal(new int[] {
            434,
            0,
            0,
            0});
      this.devToolsCropY.ValueChanged += new System.EventHandler(this.devToolsCropY_ValueChanged);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label8.Location = new System.Drawing.Point(3, 340);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(48, 16);
      this.label8.TabIndex = 37;
      this.label8.Text = "Crop X";
      // 
      // devToolsCropX
      // 
      this.devToolsCropX.Location = new System.Drawing.Point(6, 359);
      this.devToolsCropX.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.devToolsCropX.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
      this.devToolsCropX.Name = "devToolsCropX";
      this.devToolsCropX.Size = new System.Drawing.Size(42, 20);
      this.devToolsCropX.TabIndex = 38;
      this.devToolsCropX.Value = new decimal(new int[] {
            590,
            0,
            0,
            0});
      this.devToolsCropX.ValueChanged += new System.EventHandler(this.devToolsCropX_ValueChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label7.Location = new System.Drawing.Point(2, 382);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(185, 16);
      this.label7.TabIndex = 32;
      this.label7.Text = "Cropped Capture Preview";
      this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // devToolsCroppedPictureBox
      // 
      this.devToolsCroppedPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.devToolsCroppedPictureBox.Location = new System.Drawing.Point(6, 400);
      this.devToolsCroppedPictureBox.Name = "devToolsCroppedPictureBox";
      this.devToolsCroppedPictureBox.Size = new System.Drawing.Size(300, 100);
      this.devToolsCroppedPictureBox.TabIndex = 31;
      this.devToolsCroppedPictureBox.TabStop = false;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(28, 48);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(56, 13);
      this.label12.TabIndex = 43;
      this.label12.Text = "Database:";
      // 
      // cmbDatabase
      // 
      this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbDatabase.FormattingEnabled = true;
      this.cmbDatabase.Items.AddRange(new object[] {
            "Full Display (Primary)"});
      this.cmbDatabase.Location = new System.Drawing.Point(86, 45);
      this.cmbDatabase.Name = "cmbDatabase";
      this.cmbDatabase.Size = new System.Drawing.Size(345, 21);
      this.cmbDatabase.TabIndex = 42;
      this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
      // 
      // CTRNitroFueledLoadRemoverSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Name = "CTRNitroFueledLoadRemoverSettings";
      this.Padding = new System.Windows.Forms.Padding(7);
      this.Size = new System.Drawing.Size(474, 532);
      ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.requiredMatchesUpDown)).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.devToolsCropY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.devToolsCropX)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.devToolsCroppedPictureBox)).EndInit();
      this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.ComboBox processListComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox previewPictureBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PictureBox croppedPreviewPictureBox;
		private System.Windows.Forms.Label scalingLabel;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label matchDisplayLabel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown requiredMatchesUpDown;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button saveDiagnosticsButton;
		private System.Windows.Forms.Button updatePreviewButton;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckBox enableAutoSplitterChk;
		private System.Windows.Forms.Label autoSplitCategoryLbl;
		private System.Windows.Forms.Label autoSplitNameLbl;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chkAutoSplitterDisableOnSkip;
		private System.Windows.Forms.CheckBox chkRemoveTransitions;
    private System.Windows.Forms.CheckBox chkSaveDetectionLog;
    private System.Windows.Forms.CheckBox chkRemoveFadeIns;
    private System.Windows.Forms.Label lblBlackLevel;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.PictureBox devToolsCroppedPictureBox;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.NumericUpDown devToolsCropY;
    private System.Windows.Forms.NumericUpDown devToolsCropX;
    private System.Windows.Forms.CheckBox devToolsRecord;
    private System.Windows.Forms.Button devToolsDatabaseButton;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox devToolsCaptureImageText;
    private System.Windows.Forms.Button devToolsDataBaseFromCaptureImages;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.ComboBox cmbDatabase;
  }
}
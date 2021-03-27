﻿using CaptureSampleCore;
using CrashNSaneLoadDetector;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{


	public partial class Crash4LoadRemoverSettings : UserControl
	{
		#region Public Fields

		public bool AutoSplitterEnabled = false;

		public bool AutoSplitterDisableOnSkipUntilSplit = false;

		public bool RemoveFadeouts = false;
		public bool RemoveFadeins = false;

		public bool SaveDetectionLog = false;

		public bool RecordImages = false;
    public bool DetailedDetectionLog = false;

		public int AverageBlackLevel = -1;

		public string DetectionLogFolderName = "Crash4LoadRemoverLog";

		//Number of frames to wait for a change from load -> running and vice versa.
		public int AutoSplitterJitterToleranceFrames = 8;

		//If you split manually during "AutoSplitter" mode, I ignore AutoSplitter-splits for 50 frames. (A little less than 2 seconds)
		//This means that if a split would happen during these frames, it is ignored.
		public int AutoSplitterManualSplitDelayFrames = 50;

		#endregion Public Fields

		#region Private Fields

		private AutoSplitData autoSplitData = null;

		private float captureAspectRatioX = 16.0f;

		private float captureAspectRatioY = 9.0f;

		private List<string> captureIDs = null;

		private Size captureSize = new Size(300, 100);

		private float cropOffsetX = 590.0f;

		private float cropOffsetY = 434.0f;

		private bool drawingPreview = false;

		private List<Control> dynamicAutoSplitterControls;

		private float featureVectorResolutionX = 1920.0f;

		private float featureVectorResolutionY = 1080.0f;

		private ImageCaptureInfo imageCaptureInfo;

		private Bitmap lastDiagnosticCapture = null;

		private List<int> lastFeatures = null;

		private Bitmap lastFullCapture = null;

		private Bitmap lastFullCroppedCapture = null;

		private int lastMatchingBins = 0;

		private LiveSplitState liveSplitState = null;

		//private string DiagnosticsFolderName = "CrashNSTDiagnostics/";
		private int numCaptures = 0;

		private int numScreens = 1;

		private Dictionary<string, XmlElement> AllGameAutoSplitSettings;

		private Bitmap previewImage = null;

		//-1 -> full screen, otherwise index process list
		private int processCaptureIndex = -1;

		private Process[] processList;
		private int scalingValue = 100;
		private float scalingValueFloat = 1.0f;
		private string selectedCaptureID = "";
		private Point selectionBottomRight = new Point(0, 0);
		private Rectangle selectionRectanglePreviewBox;
		private Point selectionTopLeft = new Point(0, 0);
    private BasicSampleApplication WGCCaptureSample;
    private bool WGCEnabled = false;

    #endregion Private Fields

    #region Public Constructors

    private string LoadRemoverDataName = "";

		public class Binder : System.Runtime.Serialization.SerializationBinder
		{
			public override Type BindToType(string assemblyName, string typeName)
			{
				Assembly ass = Assembly.Load(assemblyName);
				return ass.GetType(typeName);
			}
		}


		private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			//LoadRemoverDataName = cmbDatabase.SelectedItem.ToString();
			DeserializeAndUpdateDetectorData();
		}

		private void SetComboBoxToStoredDatabase(string database)
		{
			/*for (int item_index = 0; item_index < cmbDatabase.Items.Count; item_index++)
			{
				var item = cmbDatabase.Items[item_index];
				if (item.ToString() == database)
				{
					cmbDatabase.SelectedIndex = item_index;
					return;
				}
			}*/
		}

		private string[] getDatabaseFiles()
		{
			return Directory.GetFiles("Components/", "*.crash4data");
		}

		private void DeserializeAndUpdateDetectorData()
		{
			DetectorData data = DeserializeDetectorData(LoadRemoverDataName);
			captureSize = new Size(data.sizeX, data.sizeY);
      return;
			FeatureDetector.numberOfBins = data.numberOfHistogramBins;
			FeatureDetector.patchSizeX = captureSize.Width / data.numPatchesX;
			FeatureDetector.patchSizeY = captureSize.Height / data.numPatchesY;
			int[][] features_temp = data.features.ToArray();

			int len_x = features_temp.Length;
			int len_y = features_temp[0].Length;

			FeatureDetector.listOfFeatureVectorsEng = new int[len_x, len_y];

			for (int x = 0; x < len_x; x++)
			{
				for (int y = 0; y < len_y; y++)
				{
					FeatureDetector.listOfFeatureVectorsEng[x, y] = features_temp[x][y];
				}
			}

		}

		public Crash4LoadRemoverSettings(LiveSplitState state)
		{
			InitializeComponent();

      // TODO/NOTE: Removed AutoSplitter control. Might come back, might not.
      tabControl1.TabPages.Remove(tabPage2);


      WGCCaptureSample = new BasicSampleApplication();
      WGCCaptureSample.Init();

      if (!WGCCaptureSample.CanUseWGCCapture)
        chkWGCEnabled.Visible = false;
      /*
			string[] database_files = getDatabaseFiles();

			if (database_files.Length == 0)
			{
				MessageBox.Show("Error: Please make sure that at least one .crash4 file exists in the Components directory!", "Crash 4 Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}*/

      /*cmbDatabase.Items.Clear();
			foreach (string database_file in database_files)
			{
				cmbDatabase.Items.Add(database_file);
			}
			cmbDatabase.SelectedIndex = 0;*/
      //RemoveFadeins = chkRemoveFadeIns.Checked;
      DeserializeAndUpdateDetectorData();

			RemoveFadeouts = chkRemoveTransitions.Checked;
			RemoveFadeins = chkRemoveTransitions.Checked;
			SaveDetectionLog = chkSaveDetectionLog.Checked;

			AllGameAutoSplitSettings = new Dictionary<string, XmlElement>();
			dynamicAutoSplitterControls = new List<Control>();
			CreateAutoSplitControls(state);
			liveSplitState = state;
			initImageCaptureInfo();
			//processListComboBox.SelectedIndex = 0;
			lblVersion.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);


			RefreshCaptureWindowList();
			//processListComboBox.SelectedIndex = 0;
			DrawPreview();


		}

		#endregion Public Constructors

		#region Public Methods

		public void StoreCaptureImage(string gameName, string category, Bitmap img = null)
		{
			System.IO.Directory.CreateDirectory(Path.Combine(DetectionLogFolderName, devToolsCaptureImageText.Text));

			string capture_time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");

			for (int offset_x = -4; offset_x <= 4; offset_x += 2)
			{
				for (int offset_y = -4; offset_y <= 4; offset_y += 2)
				{
					imageCaptureInfo.cropOffsetY = cropOffsetY + offset_y;
					imageCaptureInfo.cropOffsetX = cropOffsetX + offset_x;
					CaptureImageFullPreview(ref imageCaptureInfo, true);

					string fileName = Path.Combine(DetectionLogFolderName, devToolsCaptureImageText.Text, "Crash4LoadRemover_Log_" + capture_time + removeInvalidXMLCharacters(gameName) + "_" + removeInvalidXMLCharacters(category) + "_" + offset_x + "_" + offset_y + ".png");


					using (MemoryStream memory = new MemoryStream())
					{
						using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
						{
              Bitmap capture = null;

              if (img != null)
              {
                capture = img;
              }
              else
              {
                capture = CaptureImage(Crash4LoadState.LOAD1);
              }
							
							capture.Save(memory, ImageFormat.Png);
							byte[] bytes = memory.ToArray();
							fs.Write(bytes, 0, bytes.Length);
						}
					}
				}

			}


			imageCaptureInfo.cropOffsetY = cropOffsetY;
			imageCaptureInfo.cropOffsetX = cropOffsetX;
			CaptureImageFullPreview(ref imageCaptureInfo, true);
			devToolsCroppedPictureBox.Image = CaptureImage(Crash4LoadState.LOAD1);


		}

		public void SetBlackLevel(int black_level)
		{
			AverageBlackLevel = black_level;
			lblBlackLevel.Text = "Black-Level: " + AverageBlackLevel;
		}
    int frameNumber = 0;
		public Bitmap CaptureImage(Crash4LoadState state)
		{
			Bitmap b = new Bitmap(1, 1);

      var copy_info = imageCaptureInfo;
      int feature_offset = 0;

      if (state == Crash4LoadState.WAITING_FOR_LOAD1 || state == Crash4LoadState.LOAD1 || state == Crash4LoadState.WAITING_FOR_LOAD2)
      {
        // Switch imageCaptureInfo and use a different one. This is all very hacky and bad, but whatever
        imageCaptureInfo.cropOffsetX = -13;
        imageCaptureInfo.cropOffsetY = -460;
        imageCaptureInfo.captureSizeX = 300;
        imageCaptureInfo.captureSizeY = 40;
        imageCaptureInfo.feature_size_x = 300;
        imageCaptureInfo.feature_size_y = 40;
        imageCaptureInfo.feature_size_x2 = 50;
        imageCaptureInfo.feature_size_y2 = 50;
        imageCaptureInfo.cropOffsetX2 = -783;
        imageCaptureInfo.cropOffsetY2 = 363;
        imageCaptureInfo.cropOffsetX1 = -13;
        imageCaptureInfo.cropOffsetY1 = -460;
        captureSize.Width = imageCaptureInfo.captureSizeX;
        captureSize.Height = imageCaptureInfo.captureSizeY;
      }
      else
      {
        // Switch imageCaptureInfo and use a different one. This is all very hacky and bad, but whatever
        imageCaptureInfo.cropOffsetX = -783;
        imageCaptureInfo.cropOffsetY = 363;
        imageCaptureInfo.captureSizeX = 50;
        imageCaptureInfo.captureSizeY = 50;
        imageCaptureInfo.feature_size_x = 300;
        imageCaptureInfo.feature_size_y = 40;
        imageCaptureInfo.feature_size_x2 = 50;
        imageCaptureInfo.feature_size_y2 = 50;
        imageCaptureInfo.cropOffsetX2 = -783;
        imageCaptureInfo.cropOffsetY2 = 363;
        imageCaptureInfo.cropOffsetX1 = -13;
        imageCaptureInfo.cropOffsetY1 = -460;
        captureSize.Width = imageCaptureInfo.captureSizeX;
        captureSize.Height = imageCaptureInfo.captureSizeY;
        feature_offset = 3;
      }

			//Full screen capture
			if (processCaptureIndex < 0)
			{
				Screen selected_screen = Screen.AllScreens[-processCaptureIndex - 1];
				Rectangle screenRect = selected_screen.Bounds;

				screenRect.Width = (int)(screenRect.Width * scalingValueFloat);
				screenRect.Height = (int)(screenRect.Height * scalingValueFloat);

				Point screenCenter = new Point(screenRect.Width / 2, screenRect.Height / 2);

				//Change size according to selected crop
				screenRect.Width = (int)(imageCaptureInfo.crop_coordinate_right - imageCaptureInfo.crop_coordinate_left);
				screenRect.Height = (int)(imageCaptureInfo.crop_coordinate_bottom - imageCaptureInfo.crop_coordinate_top);

				//Compute crop coordinates and width/ height based on resoution
				ImageCapture.SizeAdjustedCropAndOffset(screenRect.Width, screenRect.Height, ref imageCaptureInfo);

				//Adjust for crop offset
				imageCaptureInfo.center_of_frame_x += imageCaptureInfo.crop_coordinate_left;
				imageCaptureInfo.center_of_frame_y += imageCaptureInfo.crop_coordinate_top;

				//Adjust for selected screen offset
				imageCaptureInfo.center_of_frame_x += selected_screen.Bounds.X;
				imageCaptureInfo.center_of_frame_y += selected_screen.Bounds.Y;

				b = ImageCapture.CaptureFromDisplay(ref imageCaptureInfo);
			}
			else
			{
				IntPtr handle = new IntPtr(0);

				if (processCaptureIndex >= processList.Length)
					return b;

				if (processCaptureIndex != -1)
				{
					handle = processList[processCaptureIndex].MainWindowHandle;
				}
				//Capture from specific process
				processList[processCaptureIndex].Refresh();
				if ((int)handle == 0)
					return b;

        if(!WGCEnabled)
        {
          b = ImageCapture.PrintWindow(handle, ref imageCaptureInfo, useCrop: true);
        }
				else
        {
          WGCCaptureSample.StartCaptureFromHwnd(handle);
          //WGCCaptureSample.SetImageCaptureInfo(ref imageCaptureInfo);
          b = WGCCaptureSample.getNewestBitmap(feature_offset);
          //b.Save("outputs/test" + frameNumber++ + ".png");
        }


			}

      // Restore from copy. This is all very hacky and bad, but whatever
      imageCaptureInfo.cropOffsetX = copy_info.cropOffsetX;
      imageCaptureInfo.cropOffsetY = copy_info.cropOffsetY;
      imageCaptureInfo.captureSizeX = copy_info.captureSizeX;
      imageCaptureInfo.captureSizeY = copy_info.captureSizeY;
      captureSize.Width = imageCaptureInfo.captureSizeX;
      captureSize.Height = imageCaptureInfo.captureSizeY;

      return b;
		}

		public Bitmap CaptureImageFullPreview(ref ImageCaptureInfo imageCaptureInfo, bool useCrop = false)
		{
			Bitmap b = new Bitmap(1, 1);

			//Full screen capture
			if (processCaptureIndex < 0)
			{
				Screen selected_screen = Screen.AllScreens[-processCaptureIndex - 1];
				Rectangle screenRect = selected_screen.Bounds;

				screenRect.Width = (int)(screenRect.Width * scalingValueFloat);
				screenRect.Height = (int)(screenRect.Height * scalingValueFloat);

				Point screenCenter = new Point((int)(screenRect.Width / 2.0f), (int)(screenRect.Height / 2.0f));

				if (useCrop)
				{
					//Change size according to selected crop
					screenRect.Width = (int)(imageCaptureInfo.crop_coordinate_right - imageCaptureInfo.crop_coordinate_left);
					screenRect.Height = (int)(imageCaptureInfo.crop_coordinate_bottom - imageCaptureInfo.crop_coordinate_top);
				}

				//Compute crop coordinates and width/ height based on resoution
				ImageCapture.SizeAdjustedCropAndOffset(screenRect.Width, screenRect.Height, ref imageCaptureInfo);

				imageCaptureInfo.actual_crop_size_x = 2 * imageCaptureInfo.center_of_frame_x;
				imageCaptureInfo.actual_crop_size_y = 2 * imageCaptureInfo.center_of_frame_y;

				if (useCrop)
				{
					//Adjust for crop offset
					imageCaptureInfo.center_of_frame_x += imageCaptureInfo.crop_coordinate_left;
					imageCaptureInfo.center_of_frame_y += imageCaptureInfo.crop_coordinate_top;
				}

				//Adjust for selected screen offset
				imageCaptureInfo.center_of_frame_x += selected_screen.Bounds.X;
				imageCaptureInfo.center_of_frame_y += selected_screen.Bounds.Y;

				imageCaptureInfo.actual_offset_x = 0;
				imageCaptureInfo.actual_offset_y = 0;

				b = ImageCapture.CaptureFromDisplay(ref imageCaptureInfo);

				imageCaptureInfo.actual_offset_x = cropOffsetX;
				imageCaptureInfo.actual_offset_y = cropOffsetY;
			}
			else
			{
				IntPtr handle = new IntPtr(0);

				if (processCaptureIndex >= processList.Length)
					return b;

				if (processCaptureIndex != -1)
				{
					handle = processList[processCaptureIndex].MainWindowHandle;
				}
				//Capture from specific process
				processList[processCaptureIndex].Refresh();
				if ((int)handle == 0)
					return b;

        if (!WGCEnabled)
        {
          b = ImageCapture.PrintWindow(handle, ref imageCaptureInfo, full: true, useCrop: useCrop, scalingValueFloat: scalingValueFloat);
        }
        else
        {
          WGCCaptureSample.StartCaptureFromHwnd(handle);

          //WGCCaptureSample.SetImageCaptureInfo(ref imageCaptureInfo);

          WGCCaptureSample.getPreviewBitmap(useCrop, updateFullPreviewBitmap, updateCroppedPreviewBitmap);




          //This is necessary for the preview windows to compute the correct offset later on...
          //imageCaptureInfo.actual_crop_size_x = 2 * imageCaptureInfo.center_of_frame_x;
          //imageCaptureInfo.actual_crop_size_y = 2 * imageCaptureInfo.center_of_frame_y;
          imageCaptureInfo.actual_crop_size_x = WGCCaptureSample.getLastSizeX();
          imageCaptureInfo.actual_crop_size_y = WGCCaptureSample.getLastSizeY();

          return null;
        }
      }

			return b;
		}

    public void updateFullPreviewBitmap(Bitmap b)
    {
      previewImage = (Bitmap)b.Clone();
      DrawCaptureRectangleBitmap();
    }
    public void updateCroppedPreviewBitmap(Bitmap b)
    {
      var b_clone = (Bitmap)b.Clone();
      croppedPreviewPictureBox.Image = b_clone;
      lastFullCroppedCapture = b_clone;
    }

    public void ChangeAutoSplitSettingsToGameName(string gameName, string category)
		{
			gameName = removeInvalidXMLCharacters(gameName);
			category = removeInvalidXMLCharacters(category);

			//TODO: go through gameSettings to see if the game matches, enter info based on that.
			foreach (var control in dynamicAutoSplitterControls)
			{
				tabPage2.Controls.Remove(control);
			}

			dynamicAutoSplitterControls.Clear();

			//Add current game to gameSettings
			XmlDocument document = new XmlDocument();

			var gameNode = document.CreateElement(autoSplitData.GameName + autoSplitData.Category);

			//var categoryNode = document.CreateElement(autoSplitData.Category);

			foreach (AutoSplitEntry splitEntry in autoSplitData.SplitData)
			{
				gameNode.AppendChild(ToElement(document, splitEntry.SplitName, splitEntry.NumberOfLoads));
			}


			AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = gameNode;

			//otherGameSettings[]

			CreateAutoSplitControls(liveSplitState);

			//Change controls if we find the chosen game
			foreach (var gameSettings in AllGameAutoSplitSettings)
			{
				if (gameSettings.Key == gameName + category)
				{
					var game_element = gameSettings.Value;

					//var splits_element = game_element[autoSplitData.Category];
					Dictionary<string, int> usedSplitNames = new Dictionary<string, int>();
					foreach (XmlElement number_of_loads in game_element)
					{
						var up_down_controls = tabPage2.Controls.Find(number_of_loads.LocalName, true);

						if (usedSplitNames.ContainsKey(number_of_loads.LocalName) == false)
						{
							usedSplitNames[number_of_loads.LocalName] = 0;
						}
						else
						{
							usedSplitNames[number_of_loads.LocalName]++;
						}

						//var up_down = tabPage2.Controls.Find(number_of_loads.LocalName, true).FirstOrDefault() as NumericUpDown;

						NumericUpDown up_down = (NumericUpDown)up_down_controls[usedSplitNames[number_of_loads.LocalName]];

						if (up_down != null)
						{
							up_down.Value = Convert.ToInt32(number_of_loads.InnerText);
						}
					}

				}
			}
		}
		public int GetCumulativeNumberOfLoadsForSplit(string splitName)
		{
			int numberOfLoads = 0;
			splitName = removeInvalidXMLCharacters(splitName);
			foreach (AutoSplitEntry entry in autoSplitData.SplitData)
			{
				numberOfLoads += entry.NumberOfLoads;
				if (entry.SplitName == splitName)
				{
					return numberOfLoads;
				}
			}
			return numberOfLoads;
		}

		public int GetAutoSplitNumberOfLoadsForSplit(string splitName)
		{
			splitName = removeInvalidXMLCharacters(splitName);
			foreach (AutoSplitEntry entry in autoSplitData.SplitData)
			{
				if (entry.SplitName == splitName)
				{
					return entry.NumberOfLoads;
				}
			}

			//This should never happen, but might if the splits are changed without reloading the component...
			return 2;
		}

		public XmlNode GetSettings(XmlDocument document)
		{
			//RefreshCaptureWindowList();
			var settingsNode = document.CreateElement("Settings");

			settingsNode.AppendChild(ToElement(document, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

			//settingsNode.AppendChild(ToElement(document, "RequiredMatches", FeatureDetector.numberOfBinsCorrect));

			if (captureIDs != null)
			{
				if (processListComboBox.SelectedIndex < captureIDs.Count && processListComboBox.SelectedIndex >= 0)
				{
					var selectedCaptureTitle = captureIDs[processListComboBox.SelectedIndex];

					settingsNode.AppendChild(ToElement(document, "SelectedCaptureTitle", selectedCaptureTitle));
				}
			}

			settingsNode.AppendChild(ToElement(document, "ScalingPercent", trackBar1.Value));

			var captureRegionNode = document.CreateElement("CaptureRegion");

			captureRegionNode.AppendChild(ToElement(document, "X", selectionRectanglePreviewBox.X));
			captureRegionNode.AppendChild(ToElement(document, "Y", selectionRectanglePreviewBox.Y));
			captureRegionNode.AppendChild(ToElement(document, "Width", selectionRectanglePreviewBox.Width));
			captureRegionNode.AppendChild(ToElement(document, "Height", selectionRectanglePreviewBox.Height));

			settingsNode.AppendChild(captureRegionNode);

			settingsNode.AppendChild(ToElement(document, "AutoSplitEnabled", enableAutoSplitterChk.Checked));
			settingsNode.AppendChild(ToElement(document, "AutoSplitDisableOnSkipUntilSplit", chkAutoSplitterDisableOnSkip.Checked));
			settingsNode.AppendChild(ToElement(document, "RemoveFadeouts", chkRemoveTransitions.Checked));
			//settingsNode.AppendChild(ToElement(document, "RemoveFadeins", chkRemoveFadeIns.Checked));
			settingsNode.AppendChild(ToElement(document, "SaveDetectionLog", chkSaveDetectionLog.Checked));
      settingsNode.AppendChild(ToElement(document, "DetailedDetectionLog", chkUseDetailedDetectionLog.Checked));
      settingsNode.AppendChild(ToElement(document, "WGCEnabled", chkWGCEnabled.Checked));
      //settingsNode.AppendChild(ToElement(document, "DatabaseFile", cmbDatabase.SelectedItem.ToString()));

      var splitsNode = document.CreateElement("AutoSplitGames");

			//Re-Add all other games/categories to the xml file
			foreach (var gameSettings in AllGameAutoSplitSettings)
			{
				if (gameSettings.Key != autoSplitData.GameName + autoSplitData.Category)
				{
					XmlNode node = document.ImportNode(gameSettings.Value, true);
					splitsNode.AppendChild(node);
				}
			}

			var gameNode = document.CreateElement(autoSplitData.GameName + autoSplitData.Category);

			//var categoryNode = document.CreateElement(autoSplitData.Category);

			foreach (AutoSplitEntry splitEntry in autoSplitData.SplitData)
			{
				gameNode.AppendChild(ToElement(document, splitEntry.SplitName, splitEntry.NumberOfLoads));
			}
			AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = gameNode;
			//gameNode.AppendChild(categoryNode);
			splitsNode.AppendChild(gameNode);
			settingsNode.AppendChild(splitsNode);
			//settingsNode.AppendChild(ToElement(document, "AutoReset", AutoReset.ToString()));
			//settingsNode.AppendChild(ToElement(document, "Category", category.ToString()));
			/*if (checkedListBox1.Items.Count == SplitsByCategory[category].Length)
			{
				for (int i = 0; i < checkedListBox1.Items.Count; i++)
				{
					SplitsByCategory[category][i].enabled = (checkedListBox1.GetItemCheckState(i) == CheckState.Checked);
				}
			}

			foreach (Split[] category in SplitsByCategory)
			{
				foreach (Split split in category)
				{
					settingsNode.AppendChild(ToElement(document, "split_" + split.splitID, split.enabled.ToString()));
				}
			}*/

			return settingsNode;
		}

		public void SetSettings(XmlNode settings)
		{
			var element = (XmlElement)settings;
			if (!element.IsEmpty)
			{
				Version version;
				if (element["Version"] != null)
				{
					version = Version.Parse(element["Version"].InnerText);
				}
				else {
					version = new Version(1, 0, 0);
				}

				/*if (element["RequiredMatches"] != null)
				{
					FeatureDetector.numberOfBinsCorrect = Convert.ToInt32(element["RequiredMatches"].InnerText);
					requiredMatchesUpDown.Value = Convert.ToDecimal(FeatureDetector.numberOfBinsCorrect / (float)FeatureDetector.listOfFeatureVectorsEng.GetLength(1));
				}*/

				if (element["SelectedCaptureTitle"] != null)
				{
					String selectedCaptureTitle = element["SelectedCaptureTitle"].InnerText;
					selectedCaptureID = selectedCaptureTitle;
					UpdateIndexToCaptureID();
					RefreshCaptureWindowList();
				}

				if (element["ScalingPercent"] != null)
				{
					trackBar1.Value = Convert.ToInt32(element["ScalingPercent"].InnerText);
				}


        if (element["CaptureRegion"] != null)
				{
					var element_region = element["CaptureRegion"];
					if (element_region["X"] != null && element_region["Y"] != null && element_region["Width"] != null && element_region["Height"] != null)
					{
						int captureRegionX = Convert.ToInt32(element_region["X"].InnerText);
						int captureRegionY = Convert.ToInt32(element_region["Y"].InnerText);
						int captureRegionWidth = Convert.ToInt32(element_region["Width"].InnerText);
						int captureRegionHeight = Convert.ToInt32(element_region["Height"].InnerText);

            captureRegionX = Math.Max(Math.Min(captureRegionX, Convert.ToInt32(numTopLeftRectX.Maximum)), 0);
            captureRegionY = Math.Max(Math.Min(captureRegionY, Convert.ToInt32(numTopLeftRectY.Maximum)), 0);
            captureRegionWidth = Math.Max(Math.Min(captureRegionWidth, Convert.ToInt32(numBottomRightRectX.Maximum) - captureRegionX), 0);
            captureRegionHeight = Math.Max(Math.Min(captureRegionHeight, Convert.ToInt32(numBottomRightRectY.Maximum) - captureRegionY), 0);

            selectionRectanglePreviewBox = new Rectangle(captureRegionX, captureRegionY, captureRegionWidth, captureRegionHeight);
						selectionTopLeft = new Point(captureRegionX, captureRegionY);
						selectionBottomRight = new Point(captureRegionX + captureRegionWidth, captureRegionY + captureRegionHeight);

						//RefreshCaptureWindowList();
					}
				}

				/*foreach (Split[] category in SplitsByCategory)
				{
					foreach (Split split in category)
					{
						if (element["split_" + split.splitID] != null)
						{
							split.enabled = Convert.ToBoolean(element["split_" + split.splitID].InnerText);
						}
					}
				}*/
				if (element["AutoSplitEnabled"] != null)
				{
					enableAutoSplitterChk.Checked = Convert.ToBoolean(element["AutoSplitEnabled"].InnerText);
				}

				if (element["AutoSplitDisableOnSkipUntilSplit"] != null)
				{
					chkAutoSplitterDisableOnSkip.Checked = Convert.ToBoolean(element["AutoSplitDisableOnSkipUntilSplit"].InnerText);
				}

				if (element["RemoveFadeouts"] != null)
				{
					chkRemoveTransitions.Checked = Convert.ToBoolean(element["RemoveFadeouts"].InnerText);
				}

				//if (element["RemoveFadeins"] != null)
				//{
				//  chkRemoveFadeIns.Checked = Convert.ToBoolean(element["RemoveFadeins"].InnerText);
				//}
				chkRemoveFadeIns.Checked = chkRemoveTransitions.Checked;

				if (element["SaveDetectionLog"] != null)
				{
					chkSaveDetectionLog.Checked = Convert.ToBoolean(element["SaveDetectionLog"].InnerText);
				}


        if (element["DetailedDetectionLog"] != null)
        {
          chkUseDetailedDetectionLog.Checked = Convert.ToBoolean(element["DetailedDetectionLog"].InnerText);
        }

        if (element["WGCEnabled"] != null)
        {
          chkWGCEnabled.Checked = Convert.ToBoolean(element["WGCEnabled"].InnerText);
        }

        if (element["DatabaseFile"] != null)
				{
					SetComboBoxToStoredDatabase(element["DatabaseFile"].InnerText);
				}

				if (element["AutoSplitGames"] != null)
				{
					var auto_split_element = element["AutoSplitGames"];

					foreach (XmlElement game in auto_split_element)
					{
						if (game.LocalName != autoSplitData.GameName)
						{
							AllGameAutoSplitSettings[game.LocalName] = game;
						}
					}

					if (auto_split_element[autoSplitData.GameName + autoSplitData.Category] != null)
					{
						var game_element = auto_split_element[autoSplitData.GameName + autoSplitData.Category];
						AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = game_element;
						//var splits_element = game_element[autoSplitData.Category];
						Dictionary<string, int> usedSplitNames = new Dictionary<string, int>();
						foreach (XmlElement number_of_loads in game_element)
						{
							var up_down_controls = tabPage2.Controls.Find(number_of_loads.LocalName, true);

							//This can happen if the layout was not saved and contains old splits.
							if (up_down_controls == null || up_down_controls.Length == 0)
							{
								continue;
							}

							if (usedSplitNames.ContainsKey(number_of_loads.LocalName) == false)
							{
								usedSplitNames[number_of_loads.LocalName] = 0;
							}
							else
							{
								usedSplitNames[number_of_loads.LocalName]++;
							}

							//var up_down = tabPage2.Controls.Find(number_of_loads.LocalName, true).FirstOrDefault() as NumericUpDown;

							NumericUpDown up_down = (NumericUpDown)up_down_controls[usedSplitNames[number_of_loads.LocalName]];

							if (up_down != null)
							{
								up_down.Value = Convert.ToInt32(number_of_loads.InnerText);
							}
						}
					}
				}

				DrawPreview();

				CaptureImageFullPreview(ref imageCaptureInfo, true);
				devToolsCroppedPictureBox.Image = CaptureImage(Crash4LoadState.WAITING_FOR_LOAD2);
			}
		}

		#endregion Public Methods

		#region Private Methods

		private void AutoSplitUpDown_ValueChanged(object sender, EventArgs e, string splitName)
		{
			foreach (AutoSplitEntry entry in autoSplitData.SplitData)
			{
				if (entry.SplitName == splitName)
				{
					entry.NumberOfLoads = (int)((NumericUpDown)sender).Value;
					return;
				}
			}
		}

		private void checkAutoReset_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
		{
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (processListComboBox.SelectedIndex < numScreens)
			{
				processCaptureIndex = -processListComboBox.SelectedIndex - 1;
			}
			else
			{
				processCaptureIndex = processListComboBox.SelectedIndex - numScreens;
			}

			//selectionTopLeft = new Point(0, 0);
			//selectionBottomRight = new Point(previewPictureBox.Width, previewPictureBox.Height);

			selectionRectanglePreviewBox = new Rectangle(selectionTopLeft.X, selectionTopLeft.Y, selectionBottomRight.X - selectionTopLeft.X, selectionBottomRight.Y - selectionTopLeft.Y);

      WGCCaptureSample.StopCapture();
      //Console.WriteLine("SELECTED ITEM: {0}", processListComboBox.SelectedItem.ToString());
      DrawPreview();
		}

		private void CreateAutoSplitControls(LiveSplitState state)
		{
			autoSplitCategoryLbl.Text = "Category: " + state.Run.CategoryName;
			autoSplitNameLbl.Text = "Game: " + state.Run.GameName;

			int splitOffsetY = 95;
			int splitSpacing = 50;

			int splitCounter = 0;
			autoSplitData = new AutoSplitData(removeInvalidXMLCharacters(state.Run.GameName), removeInvalidXMLCharacters(state.Run.CategoryName));

			foreach (var split in state.Run)
			{
				//Setup controls for changing AutoSplit settings
				var autoSplitPanel = new System.Windows.Forms.Panel();
				var autoSplitLbl = new System.Windows.Forms.Label();
				var autoSplitUpDown = new System.Windows.Forms.NumericUpDown();

				autoSplitUpDown.Value = 2;
				autoSplitPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
				autoSplitPanel.Controls.Add(autoSplitUpDown);
				autoSplitPanel.Controls.Add(autoSplitLbl);
				autoSplitPanel.Location = new System.Drawing.Point(28, splitOffsetY + splitSpacing * splitCounter);
				autoSplitPanel.Size = new System.Drawing.Size(409, 39);

				autoSplitLbl.AutoSize = true;
				autoSplitLbl.Location = new System.Drawing.Point(3, 10);
				autoSplitLbl.Size = new System.Drawing.Size(199, 13);
				autoSplitLbl.TabIndex = 0;
				autoSplitLbl.Text = split.Name;

				autoSplitUpDown.Location = new System.Drawing.Point(367, 8);
				autoSplitUpDown.Size = new System.Drawing.Size(35, 20);
				autoSplitUpDown.TabIndex = 1;

				//Remove all whitespace to name the control, we can then access it in SetSettings.
				autoSplitUpDown.Name = removeInvalidXMLCharacters(split.Name);

				autoSplitUpDown.ValueChanged += (s, e) => AutoSplitUpDown_ValueChanged(autoSplitUpDown, e, removeInvalidXMLCharacters(split.Name));

				tabPage2.Controls.Add(autoSplitPanel);
				//tabPage2.Controls.Add(autoSplitLbl);
				//tabPage2.Controls.Add(autoSplitUpDown);

				autoSplitData.SplitData.Add(new AutoSplitEntry(removeInvalidXMLCharacters(split.Name), 2));
				dynamicAutoSplitterControls.Add(autoSplitPanel);
				splitCounter++;
			}
		}

		private void DrawCaptureRectangleBitmap()
		{
			Bitmap capture_image = (Bitmap)previewImage.Clone();
			//Draw selection rectangle
			using (Graphics g = Graphics.FromImage(capture_image))
			{
				Pen drawing_pen = new Pen(Color.Magenta, 8.0f);
				drawing_pen.Alignment = PenAlignment.Inset;
				g.DrawRectangle(drawing_pen, selectionRectanglePreviewBox);
			}

			previewPictureBox.Image = capture_image;
		}

		private void DrawPreview()
		{
			try
			{


				ImageCaptureInfo copy = imageCaptureInfo;
				copy.captureSizeX = previewPictureBox.Width;
				copy.captureSizeY = previewPictureBox.Height;

				//Show something in the preview
				var capture_full = CaptureImageFullPreview(ref copy);

        if (capture_full != null)
          previewImage = capture_full;

				float crop_size_x = copy.actual_crop_size_x;
				float crop_size_y = copy.actual_crop_size_y;

				lastFullCapture = previewImage;
				//Draw selection rectangle
				DrawCaptureRectangleBitmap();

				//Compute image crop coordinates according to selection rectangle

				//Get raw image size from imageCaptureInfo.actual_crop_size to compute scaling between raw and rectangle coordinates

				//Console.WriteLine("SIZE X: {0}, SIZE Y: {1}", imageCaptureInfo.actual_crop_size_x, imageCaptureInfo.actual_crop_size_y);

				imageCaptureInfo.crop_coordinate_left = selectionRectanglePreviewBox.Left * (crop_size_x / previewPictureBox.Width);
				imageCaptureInfo.crop_coordinate_right = selectionRectanglePreviewBox.Right * (crop_size_x / previewPictureBox.Width);
				imageCaptureInfo.crop_coordinate_top = selectionRectanglePreviewBox.Top * (crop_size_y / previewPictureBox.Height);
				imageCaptureInfo.crop_coordinate_bottom = selectionRectanglePreviewBox.Bottom * (crop_size_y / previewPictureBox.Height);

				copy.crop_coordinate_left = selectionRectanglePreviewBox.Left * (crop_size_x / previewPictureBox.Width);
				copy.crop_coordinate_right = selectionRectanglePreviewBox.Right * (crop_size_x / previewPictureBox.Width);
				copy.crop_coordinate_top = selectionRectanglePreviewBox.Top * (crop_size_y / previewPictureBox.Height);
				copy.crop_coordinate_bottom = selectionRectanglePreviewBox.Bottom * (crop_size_y / previewPictureBox.Height);

        if(WGCEnabled)
          WGCCaptureSample.SetImageCaptureInfo(ref imageCaptureInfo);

        var capture_cropped = CaptureImageFullPreview(ref copy, useCrop: true);
        Bitmap full_cropped_capture = capture_cropped;

        if(capture_cropped != null)
        {
          croppedPreviewPictureBox.Image = full_cropped_capture;
          lastFullCroppedCapture = full_cropped_capture;
        }
				

				copy.captureSizeX = captureSize.Width;
				copy.captureSizeY = captureSize.Height;

				//Show matching bins for preview
				var capture = CaptureImage(Crash4LoadState.WAITING_FOR_LOAD2);
				List<int> dummy;
				List<int> dummy2;
				int black_level = 0;
				var features = FeatureDetector.featuresFromBitmap(capture, out dummy, out black_level, out dummy2);
				int tempMatchingBins = 0;
				var isLoading = FeatureDetector.compareFeatureVector(features.ToArray(), FeatureDetector.listOfFeatureVectorsEng, out tempMatchingBins, -1.0f, false);

				lastFeatures = features;
				lastDiagnosticCapture = capture;
				lastMatchingBins = tempMatchingBins;
				matchDisplayLabel.Text = Math.Round((Convert.ToSingle(tempMatchingBins) / Convert.ToSingle(FeatureDetector.listOfFeatureVectorsEng.GetLength(1))), 4).ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.ToString());
			}
		}

		private void enableAutoSplitterChk_CheckedChanged(object sender, EventArgs e)
		{
			AutoSplitterEnabled = enableAutoSplitterChk.Checked;
		}

		private void initImageCaptureInfo()
		{
			imageCaptureInfo = new ImageCaptureInfo();

			selectionTopLeft = new Point(0, 0);
			selectionBottomRight = new Point(previewPictureBox.Width, previewPictureBox.Height);
			selectionRectanglePreviewBox = new Rectangle(selectionTopLeft.X, selectionTopLeft.Y, selectionBottomRight.X - selectionTopLeft.X, selectionBottomRight.Y - selectionTopLeft.Y);

			//float required_matches = Math.Min(Convert.ToSingle(FeatureDetector.numberOfBinsCorrect / (float)FeatureDetector.listOfFeatureVectorsEng.GetLength(1)), 1.0f);
			//requiredMatchesUpDown.Value = Convert.ToDecimal(required_matches);

			imageCaptureInfo.featureVectorResolutionX = featureVectorResolutionX;
			imageCaptureInfo.featureVectorResolutionY = featureVectorResolutionY;
			imageCaptureInfo.captureSizeX = captureSize.Width;
			imageCaptureInfo.captureSizeY = captureSize.Height;
			imageCaptureInfo.captureAspectRatio = captureAspectRatioX / captureAspectRatioY;
      imageCaptureInfo.preview_full_size_x = previewPictureBox.Width;
      imageCaptureInfo.preview_full_size_y = previewPictureBox.Height;
      imageCaptureInfo.cropped_preview_size_x = croppedPreviewPictureBox.Width;
      imageCaptureInfo.cropped_preview_size_y = croppedPreviewPictureBox.Height;

      imageCaptureInfo.cropOffsetX = cropOffsetX;
      imageCaptureInfo.cropOffsetY = cropOffsetY;
      imageCaptureInfo.feature_size_x = 300;
      imageCaptureInfo.feature_size_y = 40;
      imageCaptureInfo.feature_size_x2 = 50;
      imageCaptureInfo.feature_size_y2 = 50;
      imageCaptureInfo.cropOffsetX2 = -783;
      imageCaptureInfo.cropOffsetY2 = 363;
      imageCaptureInfo.cropOffsetX1 = -13;
      imageCaptureInfo.cropOffsetY1 = -460;

    }

		private void previewPictureBox_MouseClick(object sender, MouseEventArgs e)
		{
		}

		private void previewPictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			SetRectangleFromMouse(e);
			DrawPreview();
		}

		private void previewPictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			SetRectangleFromMouse(e);
			if (drawingPreview == false)
			{
				drawingPreview = true;
				//Draw selection rectangle
				DrawCaptureRectangleBitmap();
				drawingPreview = false;
			}
		}

		private void previewPictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			SetRectangleFromMouse(e);
			DrawPreview();
		}

		private void processListComboBox_DropDown(object sender, EventArgs e)
		{
			RefreshCaptureWindowList();
			//processListComboBox.SelectedIndex = 0;
		}

		private void RefreshCaptureWindowList()
		{
			try
			{
				Process[] processListtmp = Process.GetProcesses();
				List<Process> processes_with_name = new List<Process>();

				if (captureIDs != null)
				{
					if (processListComboBox.SelectedIndex < captureIDs.Count && processListComboBox.SelectedIndex >= 0)
					{
						selectedCaptureID = processListComboBox.SelectedItem.ToString();
					}
				}

				captureIDs = new List<string>();

				processListComboBox.Items.Clear();
				numScreens = 0;
				foreach (var screen in Screen.AllScreens)
				{
					// For each screen, add the screen properties to a list box.
					processListComboBox.Items.Add("Screen: " + screen.DeviceName + ", " + screen.Bounds.ToString());
					captureIDs.Add("Screen: " + screen.DeviceName);
					numScreens++;
				}
				foreach (Process process in processListtmp)
				{
					if (!String.IsNullOrEmpty(process.MainWindowTitle) && DLLImportStuff.IsWindowValidForCapture(process.MainWindowHandle))
					{
						//Console.WriteLine("Process: {0} ID: {1} Window title: {2} HWND PTR {3}", process.ProcessName, process.Id, process.MainWindowTitle, process.MainWindowHandle);
						processListComboBox.Items.Add(process.ProcessName + ": " + process.MainWindowTitle);
						captureIDs.Add(process.ProcessName);
						processes_with_name.Add(process);
					}
				}

				UpdateIndexToCaptureID();

				//processListComboBox.SelectedIndex = 0;
				processList = processes_with_name.ToArray();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.ToString());
			}
		}

		public string removeInvalidXMLCharacters(string in_string)
		{
			if (in_string == null) return null;

			StringBuilder sbOutput = new StringBuilder();
			char ch;

			bool was_other_char = false;

			for (int i = 0; i < in_string.Length; i++)
			{
				ch = in_string[i];

				if ((ch >= 0x0 && ch <= 0x2F) ||
					(ch >= 0x3A && ch <= 0x40) ||
					(ch >= 0x5B && ch <= 0x60) ||
					(ch >= 0x7B)
					)
				{
					continue;
				}

				//Can't start with a number.
				if (was_other_char == false && ch >= '0' && ch <= '9')
				{
					continue;
				}

				/*if ((ch >= 0x0020 && ch <= 0xD7FF) ||
					(ch >= 0xE000 && ch <= 0xFFFD) ||
					ch == 0x0009 ||
					ch == 0x000A ||
					ch == 0x000D)
				{*/
				sbOutput.Append(ch);
				was_other_char = true;
				//}
			}

			if (sbOutput.Length == 0)
			{
				sbOutput.Append("NULL");
			}

			return sbOutput.ToString();
		}

		private void requiredMatchesUpDown_ValueChanged(object sender, EventArgs e)
		{
			//FeatureDetector.numberOfBinsCorrect = (int)(requiredMatchesUpDown.Value * FeatureDetector.listOfFeatureVectorsEng.GetLength(1));
		}

		private void saveDiagnosticsButton_Click(object sender, EventArgs e)
		{
			try
			{
				FolderBrowserDialog fbd = new FolderBrowserDialog();

				var result = fbd.ShowDialog();

				if (result != DialogResult.OK)
				{
					return;
				}

				//System.IO.Directory.CreateDirectory(fbd.SelectedPath);
				numCaptures++;
				lastFullCapture.Save(fbd.SelectedPath + "/" + numCaptures.ToString() + "_FULL_" + lastMatchingBins + ".jpg", ImageFormat.Jpeg);
				lastFullCroppedCapture.Save(fbd.SelectedPath + "/" + numCaptures.ToString() + "_FULL_CROPPED_" + lastMatchingBins + ".jpg", ImageFormat.Jpeg);
				lastDiagnosticCapture.Save(fbd.SelectedPath + "/" + numCaptures.ToString() + "_DIAGNOSTIC_" + lastMatchingBins + ".jpg", ImageFormat.Jpeg);
				saveFeatureVectorToTxt(lastFeatures, numCaptures.ToString() + "_FEATURES_" + lastMatchingBins + ".txt", fbd.SelectedPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.ToString());
			}
		}

		private void saveFeatureVectorToTxt(List<int> featureVector, string filename, string directoryName)
		{
			System.IO.Directory.CreateDirectory(directoryName);
			try
			{
				using (var file = File.CreateText(directoryName + "/" + filename))
				{
					file.Write("{");
					file.Write(string.Join(",", featureVector));
					file.Write("},\n");
				}
			}
			catch
			{
				//yeah, silent catch is bad, I don't care
			}
		}

		private void SetRectangleFromMouse(MouseEventArgs e)
		{
			//Clamp values to pictureBox range
			int x = Math.Min(Math.Max(0, e.Location.X), previewPictureBox.Width);
			int y = Math.Min(Math.Max(0, e.Location.Y), previewPictureBox.Height);

			if (e.Button == MouseButtons.Left
				&& (selectionRectanglePreviewBox.Left + selectionRectanglePreviewBox.Width) - x > 0
				&& (selectionRectanglePreviewBox.Top + selectionRectanglePreviewBox.Height) - y > 0)
			{
				selectionTopLeft = new Point(x, y);
			}
			else if (e.Button == MouseButtons.Right && x - selectionRectanglePreviewBox.Left > 0 && y - selectionRectanglePreviewBox.Top > 0)
			{
				selectionBottomRight = new Point(x, y);
			}


      do_not_trigger_value_changed = true;
      numTopLeftRectY.Value = Math.Max(Math.Min(selectionTopLeft.Y, numTopLeftRectY.Maximum), 0);

			do_not_trigger_value_changed = true;
			numTopLeftRectX.Value = Math.Max(Math.Min(selectionTopLeft.X, numTopLeftRectX.Maximum), 0);

      do_not_trigger_value_changed = true;
			numBottomRightRectY.Value = Math.Max(Math.Min(selectionBottomRight.Y, numBottomRightRectY.Maximum), 0);

			do_not_trigger_value_changed = true;
			numBottomRightRectX.Value = Math.Max(Math.Min(selectionBottomRight.X, numBottomRightRectX.Maximum), 0);



      selectionRectanglePreviewBox = new Rectangle(selectionTopLeft.X, selectionTopLeft.Y, selectionBottomRight.X - selectionTopLeft.X, selectionBottomRight.Y - selectionTopLeft.Y);
		}

		private XmlElement ToElement<T>(XmlDocument document, String name, T value)
		{
			var element = document.CreateElement(name);
			element.InnerText = value.ToString();
			return element;
		}

		private void trackBar1_ValueChanged(object sender, EventArgs e)
		{
			scalingValue = trackBar1.Value;

			if (scalingValue % trackBar1.SmallChange != 0)
			{
				scalingValue = (scalingValue / trackBar1.SmallChange) * trackBar1.SmallChange;

				trackBar1.Value = scalingValue;
			}

			scalingValueFloat = ((float)scalingValue) / 100.0f;

			scalingLabel.Text = "Scaling: " + trackBar1.Value.ToString() + "%";

			DrawPreview();
		}

		private void UpdateIndexToCaptureID()
		{
			//Find matching window, set selected index to index in dropdown items
			int item_index = 0;
			for (item_index = 0; item_index < processListComboBox.Items.Count; item_index++)
			{
				String item = processListComboBox.Items[item_index].ToString();
				if (item.Contains(selectedCaptureID))
				{
					processListComboBox.SelectedIndex = item_index;
					//processListComboBox.Text = processListComboBox.SelectedItem.ToString();

					break;
				}
			}
		}

		private void updatePreviewButton_Click(object sender, EventArgs e)
		{

			DrawPreview();
		}

		#endregion Private Methods

		private void chkAutoSplitterDisableOnSkip_CheckedChanged(object sender, EventArgs e)
		{
			AutoSplitterDisableOnSkipUntilSplit = chkAutoSplitterDisableOnSkip.Checked;
		}

		private void chkRemoveTransitions_CheckedChanged(object sender, EventArgs e)
		{
			RemoveFadeouts = chkRemoveTransitions.Checked;
			RemoveFadeins = chkRemoveTransitions.Checked;
		}

		private void chkSaveDetectionLog_CheckedChanged(object sender, EventArgs e)
		{
			SaveDetectionLog = chkSaveDetectionLog.Checked;
		}

		private void chkRemoveFadeIns_CheckedChanged(object sender, EventArgs e)
		{
			//RemoveFadeins = chkRemoveFadeIns.Checked;
			RemoveFadeins = chkRemoveTransitions.Checked;
		}

		private void devToolsCropX_ValueChanged(object sender, EventArgs e)
		{
			cropOffsetX = Convert.ToSingle(devToolsCropX.Value);
			imageCaptureInfo.cropOffsetX = cropOffsetX;
			CaptureImageFullPreview(ref imageCaptureInfo, true);
			devToolsCroppedPictureBox.Image = CaptureImage(Crash4LoadState.WAITING_FOR_LOAD2);
		}

		private void devToolsCropY_ValueChanged(object sender, EventArgs e)
		{
			cropOffsetY = Convert.ToSingle(devToolsCropY.Value);
			imageCaptureInfo.cropOffsetY = cropOffsetY;
			CaptureImageFullPreview(ref imageCaptureInfo, true);
			devToolsCroppedPictureBox.Image = CaptureImage(Crash4LoadState.WAITING_FOR_LOAD2);
		}

		private void devToolsRecord_CheckedChanged(object sender, EventArgs e)
		{
			RecordImages = devToolsRecord.Checked;
		}

		private bool IsFeatureUnique(DetectorData data, int[] feature)
		{
			int tempMatchingBins;
			return !FeatureDetector.compareFeatureVector(feature, data.features, out tempMatchingBins, -1.0f, false);
		}


		private void ComputeDatabaseFromPath(string path)
		{
			DetectorData data = new DetectorData();
			data.numberOfHistogramBins = Convert.ToInt32(devToolsNumHistBins.Value);
			data.numPatchesX = Convert.ToInt32(devToolsNumPatchX.Value);
			data.numPatchesY = Convert.ToInt32(devToolsNumPatchY.Value);
			data.offsetX = Convert.ToInt32(cropOffsetX);
			data.offsetY = Convert.ToInt32(cropOffsetY);
			data.features = new List<int[]>();

			var files = System.IO.Directory.GetFiles(path);

			float[] downsampling_factors = { 1, 2, 3 };
			float[] brightness_values = { 1.0f, 0.97f, 1.03f };
			float[] contrast_values = { 1.0f, 0.97f, 1.03f };
			InterpolationMode[] interpolation_modes = { InterpolationMode.NearestNeighbor, InterpolationMode.Bicubic };

			int previous_matching_bins = FeatureDetector.numberOfBinsCorrect;
			FeatureDetector.numberOfBinsCorrect = Convert.ToInt32(0.9f * (data.numPatchesX * data.numPatchesY * data.numberOfHistogramBins * 3));

			foreach (string filename in files)
			{

				foreach (float downsampling_factor in downsampling_factors)
				{
					foreach (float brightness in brightness_values)
					{
						foreach (float contrast in contrast_values)
						{
							foreach (InterpolationMode interpolation_mode in interpolation_modes)
							{

								float gamma = 1.0f; // no change in gamma

								float adjustedBrightness = brightness - 1.0f;
								// create matrix that will brighten and contrast the image
								// https://stackoverflow.com/a/15408608
								float[][] ptsArray = {
				  new float[] {contrast, 0, 0, 0, 0}, // scale red
                  new float[] {0, contrast, 0, 0, 0}, // scale green
                  new float[] {0, 0, contrast, 0, 0}, // scale blue
                  new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                  new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

								Bitmap bmp = new Bitmap(filename);

								data.sizeX = bmp.Width;
								data.sizeY = bmp.Height;

								//Make 32 bit ARGB bitmap
								Bitmap clone = new Bitmap(bmp.Width, bmp.Height,
								  System.Drawing.Imaging.PixelFormat.Format32bppArgb);

								//Make 32 bit ARGB bitmap
								Bitmap sample_factor_clone = new Bitmap(Convert.ToInt32(bmp.Width / downsampling_factor), Convert.ToInt32(bmp.Height / downsampling_factor),
								  System.Drawing.Imaging.PixelFormat.Format32bppArgb);

								var attributes = new ImageAttributes();
								attributes.SetWrapMode(WrapMode.TileFlipXY);


								using (Graphics gr = Graphics.FromImage(sample_factor_clone))
								{
									gr.InterpolationMode = interpolation_mode;
									gr.DrawImage(bmp, new Rectangle(0, 0, sample_factor_clone.Width, sample_factor_clone.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
								}

								attributes.ClearColorMatrix();
								attributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
								attributes.SetGamma(gamma, ColorAdjustType.Bitmap);

								using (Graphics gr = Graphics.FromImage(clone))
								{
									gr.InterpolationMode = interpolation_mode;
									gr.DrawImage(sample_factor_clone, new Rectangle(0, 0, clone.Width, clone.Height), 0, 0, sample_factor_clone.Width, sample_factor_clone.Height, GraphicsUnit.Pixel, attributes);
								}

								int black_level = 0;
								List<int> max_per_patch = new List<int>();
								List<int> min_per_patch = new List<int>();
								int[] feature = FeatureDetector.featuresFromBitmap(clone, out max_per_patch, out black_level, out min_per_patch).ToArray();

								if (IsFeatureUnique(data, feature))
								{
									data.features.Add(feature);
								}


								bmp.Dispose();
								clone.Dispose();
								sample_factor_clone.Dispose();
							}
						}
					}

				}


			}

			SerializeDetectorData(data, new DirectoryInfo(path).Name);
			FeatureDetector.numberOfBinsCorrect = previous_matching_bins;
		}

		private void devToolsDatabaseButton_Click(object sender, EventArgs e)
		{
			using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
			{
				DialogResult result = fbd.ShowDialog();

				if (result != DialogResult.OK)
				{
					return;
				}

				ComputeDatabaseFromPath(fbd.SelectedPath);

			}



		}

		void SerializeDetectorData(DetectorData data, string path_suffix = "")
		{
			IFormatter formatter = new BinaryFormatter();
			System.IO.Directory.CreateDirectory(Path.Combine(DetectionLogFolderName, "SerializedData"));
			Stream stream = new FileStream(Path.Combine(DetectionLogFolderName, "SerializedData", "LiveSplit.Crash4LoadRemover_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + "_" + path_suffix + ".crash4data"), FileMode.Create, FileAccess.Write);
			using (BinaryWriter binaryWriter = new BinaryWriter(stream))
			{
				binaryWriter.Write(data.version);
				binaryWriter.Write(data.offsetX);
				binaryWriter.Write(data.offsetY);
				binaryWriter.Write(data.sizeX);
				binaryWriter.Write(data.sizeY);
				binaryWriter.Write(data.numPatchesX);
				binaryWriter.Write(data.numPatchesY);
				binaryWriter.Write(data.numberOfHistogramBins);

				// Write features
				binaryWriter.Write(data.features.Count);

				foreach (var feature in data.features)
				{
					binaryWriter.Write(feature.Length);
					foreach (var feature_entry in feature)
					{
						binaryWriter.Write(feature_entry);
					}
				}

			}

			//formatter.Serialize(stream, data);
			stream.Close();
		}

		DetectorData DeserializeDetectorData(string path)
		{
			//IFormatter formatter = new BinaryFormatter();
			//Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
			//formatter.Binder = new Binder();
			DetectorData data = new DetectorData();
      data.numPatchesX = 1;
      data.numPatchesY = 1;
      data.sizeX = 250;
      data.sizeY = 50;
      

      // NOTE: this is hacked in because we don't need databases here.
      return data;
      /*
			using (BinaryReader binaryReader = new BinaryReader(stream))
			{
				// Read version number
				data.version = binaryReader.ReadInt32();

				if (data.version >= 1)
				{
					// Read most default stuff
					data.offsetX = binaryReader.ReadInt32();
					data.offsetY = binaryReader.ReadInt32();
					data.sizeX = binaryReader.ReadInt32();
					data.sizeY = binaryReader.ReadInt32();
					data.numPatchesX = binaryReader.ReadInt32();
					data.numPatchesY = binaryReader.ReadInt32();
					data.numberOfHistogramBins = binaryReader.ReadInt32();

					// Read features
					int number_of_feature_vectors = binaryReader.ReadInt32();

					data.features = new List<int[]>(number_of_feature_vectors);

					for (int feature_index = 0; feature_index < number_of_feature_vectors; feature_index++)
					{
						int feature_vector_length = binaryReader.ReadInt32();
						data.features.Add(new int[feature_vector_length]);
						for (int feature_entry_index = 0; feature_entry_index < feature_vector_length; feature_entry_index++)
						{
							data.features[feature_index][feature_entry_index] = binaryReader.ReadInt32();
						}

					}

				}


			}

			stream.Close();

			cropOffsetX = data.offsetX;
			cropOffsetY = data.offsetY;
			imageCaptureInfo.cropOffsetX = cropOffsetX;
			imageCaptureInfo.cropOffsetY = cropOffsetY;

			return data;*/
		}

		private void devToolsDataBaseFromCaptureImages_Click(object sender, EventArgs e)
		{
			ComputeDatabaseFromPath(Path.Combine(DetectionLogFolderName, devToolsCaptureImageText.Text));
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{

		}

		bool do_not_trigger_value_changed = false;

		private void numericUpDown4_ValueChanged(object sender, EventArgs e)
		{
			if (do_not_trigger_value_changed == false)
			{
				SetRectangleFromMouse(new MouseEventArgs(MouseButtons.Left, 1, Convert.ToInt32(numTopLeftRectX.Value), Convert.ToInt32(numTopLeftRectY.Value), 0));
				DrawPreview();
			}
			do_not_trigger_value_changed = false;
		}

		private void numericUpDown3_ValueChanged(object sender, EventArgs e)
		{
			if (do_not_trigger_value_changed == false)
			{
				SetRectangleFromMouse(new MouseEventArgs(MouseButtons.Left, 1, Convert.ToInt32(numTopLeftRectX.Value), Convert.ToInt32(numTopLeftRectY.Value), 0));
				DrawPreview();
			}
			do_not_trigger_value_changed = false;
		}

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			if (do_not_trigger_value_changed == false)
			{
				SetRectangleFromMouse(new MouseEventArgs(MouseButtons.Right, 1, Convert.ToInt32(numBottomRightRectX.Value), Convert.ToInt32(numBottomRightRectY.Value), 0));
				DrawPreview();
			}
			do_not_trigger_value_changed = false;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (do_not_trigger_value_changed == false)
			{
				SetRectangleFromMouse(new MouseEventArgs(MouseButtons.Right, 1, Convert.ToInt32(numBottomRightRectX.Value), Convert.ToInt32(numBottomRightRectY.Value), 0));
				DrawPreview();
			}

			do_not_trigger_value_changed = false;
		}

		private void devToolsCapSizeX_ValueChanged(object sender, EventArgs e)
		{
			captureSize.Width = Convert.ToInt32(devToolsCapSizeX.Value);
			imageCaptureInfo.captureSizeX = captureSize.Width;
			//initImageCaptureInfo();
		}

		private void devToolsCapSizeY_ValueChanged(object sender, EventArgs e)
		{
			captureSize.Height = Convert.ToInt32(devToolsCapSizeY.Value);
			imageCaptureInfo.captureSizeY = captureSize.Height;
			//initImageCaptureInfo();
		}

    private void chkUseDetailedDetectionLog_CheckedChanged(object sender, EventArgs e)
    {
      DetailedDetectionLog = chkUseDetailedDetectionLog.Checked;


    }

    private void chkWGCEnabled_CheckedChanged(object sender, EventArgs e)
    {
      WGCEnabled = chkWGCEnabled.Checked;

      // We stop the capture, and start it in the draw preview / capture image methods if necessary.
      WGCCaptureSample.StopCapture();

      DrawPreview();

      //if (WGCEnabled)
      //  WGCCaptureSample.LeakTest();
    }

    private void previewPictureBox_Click(object sender, EventArgs e)
    {

    }
  }

  [Serializable]
	public class DetectorData
	{
		public int version = 1;
		public int offsetX;
		public int offsetY;
		public int sizeX;
		public int sizeY;
		public int numPatchesX;
		public int numPatchesY;
		public int numberOfHistogramBins;

		public List<int[]> features;
	}


	public class AutoSplitData
	{
		#region Public Fields

		public string Category;
		public string GameName;
		public List<AutoSplitEntry> SplitData;

		#endregion Public Fields

		#region Public Constructors

		public AutoSplitData(string gameName, string category)
		{
			SplitData = new List<AutoSplitEntry>();
			GameName = gameName;
			Category = category;
		}

		#endregion Public Constructors
	}

	public class AutoSplitEntry
	{
		#region Public Fields

		public int NumberOfLoads = 2;
		public string SplitName = "";

		#endregion Public Fields

		#region Public Constructors

		public AutoSplitEntry(string splitName, int numberOfLoads)
		{
			SplitName = splitName;
			NumberOfLoads = numberOfLoads;
		}

		#endregion Public Constructors
	}
}
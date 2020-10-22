using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using CrashNSaneLoadDetector;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
//using System.Threading;

namespace LiveSplit.UI.Components
{

  public enum Crash4LoadState
  {
    WAITING_FOR_LOAD1,
    LOAD1,
    WAITING_FOR_LOAD2, // This state is only there for a couple of tolerance frame to check if we can find the swirl in the bottom left
    TRANSITION_TO_LOAD2,
    LOAD2
  }

  class Crash4LoadRemoverComponent : IComponent
  {
    public string ComponentName
    {
      get { return "Crash Bandicoot 4: It's About Time - Load Remover"; }
    }
    public GraphicsCache Cache { get; set; }


    public float PaddingBottom { get { return 0; } }
    public float PaddingTop { get { return 0; } }
    public float PaddingLeft { get { return 0; } }
    public float PaddingRight { get { return 0; } }
    public int frame_count = 0;
    public bool Refresh { get; set; }

    public IDictionary<string, Action> ContextMenuControls { get; protected set; }

    public Crash4LoadRemoverSettings settings { get; set; }

    private bool isLoading = false;
    private bool isTransition = false;
    private int matchingBins = 0;
    private float numSecondsTransitionMax = 10.0f; // A transition can at most be 5 seconds long, otherwise it is not counted

    private TimerModel timer;
    private bool timerStarted = false;
    private bool postLoadTransition = false;
    private bool first_frame_post_load_transition = false;
    private double total_paused_time = 0.0f;
    private string log_file_name = "";
    FileStream log_file_stream = null;
    StreamWriter log_file_writer = null;

    public enum CrashNSTState
    {
      RUNNING,
      LOADING
    }


    private CrashNSTState NSTState = CrashNSTState.RUNNING;
    private Crash4LoadState Crash4State = Crash4LoadState.WAITING_FOR_LOAD1;
    private int runningFrames = 0;
    private int pausedFrames = 0;
    private int pausedFramesSegment = 0;
    private string GameName = "";
    private string GameCategory = "";
    private int NumberOfSplits = 0;
    private List<string> SplitNames;
    private DateTime lastTime;
    private DateTime transitionStart;

    private DateTime segmentTimeStart;
    private LiveSplitState liveSplitState;
    //private Thread captureThread;
    private bool threadRunning = false;
    private double framesSum = 0.0;
    private int framesSumRounded = 0;
    private int framesSinceLastManualSplit = 0;
    private bool LastSplitSkip = false;
    private const float LOAD_PHASE_TOLERANCE_TIME = 0.3f; // waits 0.3s for the next load phase, otherwise discards it and returns to base state.
    private Time load1_phase_start;
    private Time load1_phase_end; // This is used for the tolerance

    //private HighResolutionTimer.HighResolutionTimer highResTimer;
    private List<int> NumberOfLoadsPerSplit;
    private List<FeatureDetector.HSVRange> hsv_ranges_load_1;
    private List<int> gradient_thresholds_load_1;
    private List<FeatureDetector.HSVRange> hsv_ranges_load_2;
    private List<int> gradient_thresholds_load_2;

    private List<FeatureDetector.HSVRange> test_hsv_ranges;
    private List<int> test_gradient_ranges;


    private List<float> achieved_hsv_ranges;
    private List<float> achieved_gradient_thresholds;
    private List<float> average_thresholded_gradients;

    private bool output_state_info = true;
    private bool output_first_load_debug = true;
    private bool output_second_load_debug = true;
    private bool output_to_file = true;

    public Crash4LoadRemoverComponent(LiveSplitState state)
    {

      GameName = state.Run.GameName;
      GameCategory = state.Run.CategoryName;
      NumberOfSplits = state.Run.Count;
      SplitNames = new List<string>();

      foreach (var split in state.Run)
      {
        SplitNames.Add(split.Name);
      }

      liveSplitState = state;
      NumberOfLoadsPerSplit = new List<int>();
      InitNumberOfLoadsFromState();

      settings = new Crash4LoadRemoverSettings(state);

      lastTime = DateTime.Now;
      segmentTimeStart = DateTime.Now;
      timer = new TimerModel { CurrentState = state };
      timer.CurrentState.OnStart += timer_OnStart;
      timer.CurrentState.OnReset += timer_OnReset;
      timer.CurrentState.OnSkipSplit += timer_OnSkipSplit;
      timer.CurrentState.OnSplit += timer_OnSplit;
      timer.CurrentState.OnUndoSplit += timer_OnUndoSplit;
      timer.CurrentState.OnPause += timer_OnPause;
      timer.CurrentState.OnResume += timer_OnResume;
      //highResTimer = new HighResolutionTimer.HighResolutionTimer(16.0f);
      //highResTimer.Elapsed += (s, e) => { CaptureLoads(); };

      hsv_ranges_load_1 = new List<FeatureDetector.HSVRange>();
      hsv_ranges_load_2 = new List<FeatureDetector.HSVRange>();
      gradient_thresholds_load_1 = new List<int>();
      gradient_thresholds_load_2 = new List<int>();

      //isLoading = FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.3f, out achieved_threshold, 5, 55, 50, 101, 75, 101);

      //isLoading &= FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.08f, out achieved_threshold_2, -1, 361, 30, 101, -1, 30);
      //FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.3f, out achieved_threshold_1, 200, 230, 80, 101, 50, 101);

      //FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.3f, out achieved_threshold_2, -1, 360, 95, 101, -1, 15);

      hsv_ranges_load_1.Add(new FeatureDetector.HSVRange(15, 55, 70, 101, 75, 101));
      hsv_ranges_load_1.Add(new FeatureDetector.HSVRange(-1, 361, 30, 101, -1, 30));
      gradient_thresholds_load_1.Add(102);
      gradient_thresholds_load_1.Add(10);

      hsv_ranges_load_2.Add(new FeatureDetector.HSVRange(200, 230, 80, 101, 50, 101));
      hsv_ranges_load_2.Add(new FeatureDetector.HSVRange(-1, 360, 60, 101, -1, 35));

      // TODO: determine optimal gradient thresholds
      gradient_thresholds_load_2.Add(80);
      gradient_thresholds_load_2.Add(10);

      test_hsv_ranges = new List<FeatureDetector.HSVRange>();
      test_gradient_ranges = new List<int>();
      test_hsv_ranges.Add(new FeatureDetector.HSVRange(5, 55, 50, 101, 75, 101));
      test_hsv_ranges.Add(new FeatureDetector.HSVRange(-1, 361, 30, 101, -1, 30));
      test_gradient_ranges.Add(102);
      test_gradient_ranges.Add(10);

      test_hsv_ranges.Add(new FeatureDetector.HSVRange(200, 230, 80, 101, 50, 101));
      test_hsv_ranges.Add(new FeatureDetector.HSVRange(-1, 360, 60, 101, -1, 35));


      achieved_hsv_ranges = new List<float>();
      achieved_gradient_thresholds = new List<float>();
      average_thresholded_gradients = new List<float>();

      for(int i = 0; i < test_hsv_ranges.Count; i++)
      {
        achieved_hsv_ranges.Add(0);
      }

      for (int i = 0; i < test_gradient_ranges.Count; i++)
      {
        achieved_gradient_thresholds.Add(0);
        average_thresholded_gradients.Add(0);
      }
    }

    private void timer_OnResume(object sender, EventArgs e)
    {
      timerStarted = true;
    }

    private void timer_OnPause(object sender, EventArgs e)
    {
      timerStarted = false;
    }

    private void InitNumberOfLoadsFromState()
    {
      NumberOfLoadsPerSplit = new List<int>();
      NumberOfLoadsPerSplit.Clear();

      if (liveSplitState == null)
      {
        return;
      }

      foreach (var split in liveSplitState.Run)
      {
        NumberOfLoadsPerSplit.Add(0);
      }

      //Quicker way to prevent OOB on last split as I'm not sure if the index will go over if the run finishes
      NumberOfLoadsPerSplit.Add(99999);
    }

    private int CumulativeNumberOfLoadsForSplitIndex(int splitIndex)
    {
      int numberOfLoads = 0;

      for (int idx = 0; (idx < NumberOfLoadsPerSplit.Count && idx <= splitIndex); idx++)
      {
        numberOfLoads += NumberOfLoadsPerSplit[idx];
      }
      return numberOfLoads;
    }

    private void CaptureLoads()
    {
      try
      {


        if (timerStarted)
        {
          framesSinceLastManualSplit++;
          //Console.WriteLine("TIME NOW: {0}", DateTime.Now - lastTime);
          //Console.WriteLine("TIME DIFF START: {0}", DateTime.Now - lastTime);
          lastTime = DateTime.Now;

          //Capture image using the settings defined for the component
          Bitmap capture = settings.CaptureImage(Crash4State);
          isLoading = false;

          try
          {
            // CSV Test code.
            /*
            FeatureDetector.compareImageCaptureCrash4(capture, test_hsv_ranges, test_gradient_ranges, achieved_hsv_ranges, achieved_gradient_thresholds, average_thresholded_gradients);

            string csv_out = timer.CurrentState.CurrentTime.RealTime.ToString();

            for (int i = 0; i < achieved_hsv_ranges.Count; i++)
            {
              csv_out += ";" + achieved_hsv_ranges[i];
            }

            for (int i = 0; i < achieved_gradient_thresholds.Count; i++)
            {
              csv_out += ";" + achieved_gradient_thresholds[i];
              csv_out += ";" + average_thresholded_gradients[i];
            }

            Console.WriteLine(csv_out);*/
            // ...


            //DateTime current_time = DateTime.Now;
            //isLoading = FeatureDetector.compareFeatureVector(features.ToArray(), FeatureDetector.listOfFeatureVectorsEng, out tempMatchingBins, -1.0f, false);
            //Console.WriteLine("Timing for detection: {0}", (DateTime.Now - current_time).TotalSeconds);
            
            if(Crash4State == Crash4LoadState.WAITING_FOR_LOAD1 || Crash4State == Crash4LoadState.LOAD1 || Crash4State == Crash4LoadState.WAITING_FOR_LOAD2)
            {
              // Do HSV comparison on the raw image, without any features
              //float achieved_threshold = 0.0f;
              //float achieved_threshold_2 = 0.0f;

              //isLoading = FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.3f, out achieved_threshold, 5, 55, 50, 101, 75, 101);

              //isLoading &= FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.08f, out achieved_threshold_2, -1, 361, 30, 101, -1, 30);

              FeatureDetector.compareImageCaptureCrash4(capture, hsv_ranges_load_1, gradient_thresholds_load_1, achieved_hsv_ranges, achieved_gradient_thresholds, average_thresholded_gradients, 2);

              isLoading = (achieved_hsv_ranges[0] > 0.04) && (achieved_gradient_thresholds[0] > 0.10) && (average_thresholded_gradients[1] > 50);
              
              if(output_first_load_debug && settings.DetailedDetectionLog)
              {
                string csv_out = "first_load;" + timer.CurrentState.CurrentTime.RealTime.ToString();
                csv_out += ";" + achieved_hsv_ranges[0];

                csv_out += ";" + achieved_gradient_thresholds[0];
                csv_out += ";" + average_thresholded_gradients[1];


                Console.WriteLine(csv_out);
              }

              //Console.WriteLine("Loading 1: " + isLoading.ToString() + ", achieved Threshold 1: " + achieved_threshold.ToString() + ", achieved Threshold 2: " + achieved_threshold_2.ToString());

              if (isLoading && Crash4State == Crash4LoadState.WAITING_FOR_LOAD1)
              {
                // Store current time - this is the start of our load!
                Crash4State = Crash4LoadState.LOAD1;
                load1_phase_start = timer.CurrentState.CurrentTime;

                if(output_state_info && settings.DetailedDetectionLog)
                  Console.WriteLine("Transition to LOAD1:");
              }
              else if(isLoading && Crash4State == Crash4LoadState.LOAD1)
              {
                // Everything fine - nothing to do here.
              }
              else if(!isLoading && Crash4State == Crash4LoadState.LOAD1)
              {
                // Transtiion from load phase 1 into load phase 2 - this checks the next couple of frames until it potentially finds the load screen phase 2.
                Crash4State = Crash4LoadState.WAITING_FOR_LOAD2;
                load1_phase_end = timer.CurrentState.CurrentTime;

                if (output_state_info && settings.DetailedDetectionLog)
                  Console.WriteLine("Transition to WAITING_FOR_LOAD2");
              }
              else if(Crash4State == Crash4LoadState.WAITING_FOR_LOAD2)
              {
                // We're waiting for LOAD2 until the tolerance. If LOAD1 detection happens in the mean time, we reset back to LOAD1.

                // Check if the elapsed time is over our tolerance

                if(isLoading)
                {
                  // Store current time - this is the start of our load - this might happen if orange letters are shortly before the real load screen.
                  Crash4State = Crash4LoadState.LOAD1;
                  load1_phase_start = timer.CurrentState.CurrentTime;

                  if (output_state_info && settings.DetailedDetectionLog)
                    Console.WriteLine("load screen detected while waiting for LOAD2: Transition to LOAD1: Loading 1: ");
                }
                else if ((timer.CurrentState.CurrentTime - load1_phase_end).RealTime.Value.TotalSeconds > LOAD_PHASE_TOLERANCE_TIME)
                {
                  // Transition to TRANSITION_TO_LOAD2 state.
                  if (output_state_info && settings.DetailedDetectionLog)
                    Console.WriteLine("TOLERANCE OVER: Transition to TRANSITION_TO_LOAD2");

                  Crash4State = Crash4LoadState.TRANSITION_TO_LOAD2;
                }
              }
            }
            else
            {
              // Do HSV comparison on the raw image, without any features
              /*float achieved_threshold_1 = 0.0f;
              float achieved_threshold_2 = 0.0f;

              FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.3f, out achieved_threshold_1, 200, 230, 80, 101, 50, 101);

              FeatureDetector.compareImageCaptureHSVCrash4(capture, 0.3f, out achieved_threshold_2, -1, 360, 95, 101, -1, 15);*/

              FeatureDetector.compareImageCaptureCrash4(capture, hsv_ranges_load_2, gradient_thresholds_load_2, achieved_hsv_ranges, achieved_gradient_thresholds, average_thresholded_gradients, 0);

              isLoading = (achieved_gradient_thresholds[0] > 0.03) && (average_thresholded_gradients[1] > 40);


              isLoading &= ((achieved_hsv_ranges[0] > 0.40) && (achieved_hsv_ranges[1] > 0.04)) || ((achieved_hsv_ranges[0] > 0.35) && (achieved_hsv_ranges[1] > 0.05)) || ((achieved_hsv_ranges[0] > 0.30) && (achieved_hsv_ranges[1] > 0.06)) || ((achieved_hsv_ranges[0] > 0.25) && (achieved_hsv_ranges[1] > 0.12)) || ((achieved_hsv_ranges[0] > 0.20) && (achieved_hsv_ranges[1] > 0.17));

              if(output_second_load_debug && settings.DetailedDetectionLog)
              {
                string csv_out = "second_load;" + timer.CurrentState.CurrentTime.RealTime.ToString();
                csv_out += ";" + achieved_hsv_ranges[0];
                csv_out += ";" + achieved_hsv_ranges[1];
                csv_out += ";" + achieved_gradient_thresholds[0];
                csv_out += ";" + average_thresholded_gradients[1];
                Console.WriteLine(csv_out);
              }
              

              //if((achieved_threshold_1 > 0.35f && achieved_threshold_2 > 0.10f) || (achieved_threshold_1 > 0.20f && achieved_threshold_2 > 0.15f))
              if (isLoading)
              {
                // Everything fine, nothing to do here, except for transitioning to LOAD2
                // Transition to LOAD2 state.

                if(Crash4State == Crash4LoadState.TRANSITION_TO_LOAD2)
                {
                  if (output_state_info && settings.DetailedDetectionLog)
                    Console.WriteLine("TRANSITION TO LOAD2: Loading 2: ");

                  Crash4State = Crash4LoadState.LOAD2;
                }
                
              }
              else if (Crash4State == Crash4LoadState.LOAD2)
              {
                // We're done and went through a whole load cycle.
                // Compute the duration and update the timer. 
                var load_time = (timer.CurrentState.CurrentTime - load1_phase_start).RealTime.Value;
                timer.CurrentState.LoadingTimes += load_time;
                Crash4State = Crash4LoadState.WAITING_FOR_LOAD1;
                Console.WriteLine(load_time.TotalSeconds + ";" + load1_phase_start.RealTime.ToString() + ";" + timer.CurrentState.CurrentTime.RealTime.ToString());

                if (output_state_info && settings.DetailedDetectionLog)
                  Console.WriteLine("<<<<<<<<<<<<< LOAD DONE! back to to WAITING_FOR_LOAD1: elapsed time: " + load_time.TotalSeconds);
                //Console.WriteLine("LOAD DONE (pt2): Loading 2: " + isLoading.ToString() + ", achieved Threshold (blue): " + achieved_threshold_1.ToString() + ", achieved Threshold (black): " + achieved_threshold_2.ToString());
              }
              else
              {
                // This was a screen that detected the yellow/orange letters, didn't detect them again for the tolerance frame and then *didn't* detect LOAD2.
                // Back to WAITING_FOR_LOAD1.
                if (output_state_info && settings.DetailedDetectionLog)
                  Console.WriteLine("TRANSITION TO WAITING_FOR_LOAD1 (didn't see swirl): Loading 2: ");

                Crash4State = Crash4LoadState.WAITING_FOR_LOAD1;
              }

              //Console.WriteLine("Loading 2: " + isLoading.ToString() + ", achieved Threshold (blue): " + achieved_threshold_1.ToString() + ", achieved Threshold (black): " + achieved_threshold_2.ToString());
            }
          }
          catch (Exception ex)
          {
            isLoading = false;
            Console.WriteLine("Error: " + ex.ToString());
            throw ex;
          }

          /*
          matchingBins = tempMatchingBins;

          timer.CurrentState.IsGameTimePaused = isLoading;

          if (settings.RemoveFadeins || settings.RemoveFadeouts)
          {
            float new_avg_transition_max = 0.0f;
            try
            {
                //isTransition = FeatureDetector.compareFeatureVectorTransition(features.ToArray(), FeatureDetector.listOfFeatureVectorsEng, max_per_patch, min_per_patch, - 1.0f, out new_avg_transition_max, out tempMatchingBins, 0.8f, false);//FeatureDetector.isGameTransition(capture, 30);
            }
            catch (Exception ex)
            {
              isTransition = false;
              Console.WriteLine("Error: " + ex.ToString());
              throw ex;
            }
            //Console.WriteLine("Transition: {0}", isTransition);
            if (wasLoading && isTransition && settings.RemoveFadeins)
            {
              postLoadTransition = true;
              transitionStart = DateTime.Now;
            }

            if (wasTransition == false && isTransition && settings.RemoveFadeouts)
            {
              //This could be a pre-load transition, start timing it
              transitionStart = DateTime.Now;
            }


            //Console.WriteLine("GAMETIMEPAUSETIME: {0}", timer.CurrentState.GameTimePauseTime);

            
            if (wasTransition && isLoading)
            {
              // This was a pre-load transition, subtract the gametime
              TimeSpan delta = (DateTime.Now - transitionStart);

              if(settings.RemoveFadeouts)
              {
                if (delta.TotalSeconds > numSecondsTransitionMax)
                {
                  Console.WriteLine("{2}: Transition longer than {0} seconds, doesn't count! (Took {1} seconds)", numSecondsTransitionMax, delta.TotalSeconds, SplitNames[Math.Max(Math.Min(liveSplitState.CurrentSplitIndex, SplitNames.Count - 1), 0)]);
                }
                else
                {
                  timer.CurrentState.SetGameTime(timer.CurrentState.GameTimePauseTime - delta);

                  total_paused_time += delta.TotalSeconds;
                  Console.WriteLine("PRE-LOAD TRANSITION {2} seconds: {0}, totalPausedTime: {1}", delta.TotalSeconds, total_paused_time, SplitNames[Math.Max(Math.Min(liveSplitState.CurrentSplitIndex, SplitNames.Count - 1), 0)]);
                }

              }
              
            }

            if(settings.RemoveFadeins)
            {
              if (postLoadTransition && isTransition == false)
              {
                TimeSpan delta = (DateTime.Now - transitionStart);

                total_paused_time += delta.TotalSeconds;
                Console.WriteLine("POST-LOAD TRANSITION {2} seconds: {0}, totalPausedTime: {1}", delta.TotalSeconds, total_paused_time, SplitNames[Math.Max(Math.Min(liveSplitState.CurrentSplitIndex, SplitNames.Count - 1), 0)]);
              }

             
              if (postLoadTransition == true && isTransition)
              {
                // We are transitioning after a load screen, this stops the timer, and actually increases the load time
                timer.CurrentState.IsGameTimePaused = true;


              }
              else
              {
                postLoadTransition = false;
              }

            }

          }



          if (isLoading && !wasLoading)
          {
            segmentTimeStart = DateTime.Now;
          }

          if (isLoading)
          {
            pausedFramesSegment++;
          }

          if (wasLoading && !isLoading)
          {
            TimeSpan delta = (DateTime.Now - segmentTimeStart);
            framesSum += delta.TotalSeconds * 60.0f;
            int framesRounded = Convert.ToInt32(delta.TotalSeconds * 60.0f);
            framesSumRounded += framesRounded;
            total_paused_time += delta.TotalSeconds;

            Console.WriteLine("SEGMENT FRAMES {7}: {0}, fromTime (@60fps) {1}, timeDelta {2}, totalFrames {3}, fromTime(int) {4}, totalFrames(int) {5}, totalPausedTime(double) {6}",
              pausedFramesSegment, delta.TotalSeconds,
              delta.TotalSeconds * 60.0f, framesSum, framesRounded, framesSumRounded, total_paused_time, SplitNames[Math.Max(Math.Min(liveSplitState.CurrentSplitIndex, SplitNames.Count - 1), 0)]);
            pausedFramesSegment = 0;
          }


          if (settings.AutoSplitterEnabled && !(settings.AutoSplitterDisableOnSkipUntilSplit && LastSplitSkip))
          {
            //This is just so that if the detection is not correct by a single frame, it still only splits if a few successive frames are loading
            if (isLoading && NSTState == CrashNSTState.RUNNING)
            {
              pausedFrames++;
              runningFrames = 0;
            }
            else if (!isLoading && NSTState == CrashNSTState.LOADING)
            {
              runningFrames++;
              pausedFrames = 0;
            }

            if (NSTState == CrashNSTState.RUNNING && pausedFrames >= settings.AutoSplitterJitterToleranceFrames)
            {
              runningFrames = 0;
              pausedFrames = 0;
              //We enter pause.
              NSTState = CrashNSTState.LOADING;
              if (framesSinceLastManualSplit >= settings.AutoSplitterManualSplitDelayFrames)
              {
                NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex]++;

                if (CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex) >= settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
                {

                  timer.Split();


                }
              }

            }
            else if (NSTState == CrashNSTState.LOADING && runningFrames >= settings.AutoSplitterJitterToleranceFrames)
            {
              runningFrames = 0;
              pausedFrames = 0;
              //We enter runnning.
              NSTState = CrashNSTState.RUNNING;
            }
          }


          //Console.WriteLine("TIME TAKEN FOR DETECTION: {0}", DateTime.Now - lastTime);*/
        }
      }
      catch (Exception ex)
      {
        isTransition = false;
        isLoading = false;
        Console.WriteLine("Error: " + ex.ToString());
      }
    }

    private void timer_OnUndoSplit(object sender, EventArgs e)
    {
      //skippedPauses -= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex + 1].Name);
      runningFrames = 0;
      pausedFrames = 0;

      //If we undo a split that already has met the required number of loads, we probably want the number to reset.
      if (NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] >= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
      {
        NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] = 0;
      }

      //Otherwise - we're fine. If it is a split that was skipped earlier, we still keep track of how we're standing.


    }

    private void timer_OnSplit(object sender, EventArgs e)
    {
      runningFrames = 0;
      pausedFrames = 0;
      framesSinceLastManualSplit = 0;
      //If we split, we add all remaining loads to the last split.
      //This means that the autosplitter now starts at 0 loads on the next split.
      //This is just necessary for manual splits, as automatic splits will always have a difference of 0.
      var loadsRequiredTotal = settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex - 1].Name);
      var loadsCurrentTotal = CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex - 1);
      NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] += loadsRequiredTotal - loadsCurrentTotal;

      LastSplitSkip = false;
    }

    private void timer_OnSkipSplit(object sender, EventArgs e)
    {

      runningFrames = 0;
      pausedFrames = 0;

      //We don't need to do anything here - we just keep track of loads per split now.
      LastSplitSkip = true;

      /*if(settings.AutoSplitterDisableOnSkipUntilSplit)
			{
				NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] = 0;
			}*/
    }

    private void timer_OnReset(object sender, TimerPhase value)
    {
      timerStarted = false;
      runningFrames = 0;
      pausedFrames = 0;
      framesSinceLastManualSplit = 0;
      threadRunning = false;
      LastSplitSkip = false;
      //highResTimer.Stop(joinThread:false);
      InitNumberOfLoadsFromState();

      first_frame_post_load_transition = false;
      total_paused_time = 0.0f;


      if (log_file_writer != null)
      {
        if (log_file_writer.BaseStream != null)
        {
          log_file_writer.Flush();
          log_file_writer.Close();
          log_file_writer.Dispose();
        }
        log_file_writer = null;
      }

    }

    void timer_OnStart(object sender, EventArgs e)
    {
      InitNumberOfLoadsFromState();
      timer.InitializeGameTime();
      runningFrames = 0;
      framesSinceLastManualSplit = 0;
      pausedFrames = 0;
      timerStarted = true;
      threadRunning = true;
      first_frame_post_load_transition = false;
      total_paused_time = 0.0f;

      ReloadLogFile();
      //StartCaptureThread();
      //highResTimer.Start();
    }

    void StartCaptureThread()
    {
      //captureThread = new Thread(() =>
      //{
      //	System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
      //	while (threadRunning)
      //	{
      //watch.Restart();
      //		CaptureLoads();
      //TODO: test rounding of framecounts in output, more importantly:
      //TEST FINAL TIME TO SEE IF IT IS ACCURATE WITH THIS,
      //THEN ADD SLEEPS FOR PERFORMANCE
      //THEN ADJUST FOR BETTER PERFORMANCE

      /*Thread.Sleep(Math.Max((int)(captureDelay - watch.Elapsed.TotalMilliseconds - 1), 0));
      while(captureDelay - watch.Elapsed.TotalMilliseconds >= 0)
      {
        ;
      }*/
      //	}
      //});
      //captureThread.Start();*/
    }

    private void ReloadLogFile()
    {
      if (settings.SaveDetectionLog == false)
        return;


      System.IO.Directory.CreateDirectory(settings.DetectionLogFolderName);

      string fileName = Path.Combine(settings.DetectionLogFolderName, "Crash4LoadRemover_Log_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_") + settings.removeInvalidXMLCharacters(GameName) + "_" + settings.removeInvalidXMLCharacters(GameCategory) + ".txt");

      if (log_file_writer != null)
      {
        if (log_file_writer.BaseStream != null)
        {
          log_file_writer.Flush();
          log_file_writer.Close();
          log_file_writer.Dispose();
        }
        log_file_writer = null;
      }


      log_file_stream = new FileStream(fileName, FileMode.Create);
      log_file_writer = new StreamWriter(log_file_stream);
      log_file_writer.AutoFlush = true;

      if(output_to_file)
      {
        Console.SetOut(log_file_writer);
        Console.SetError(log_file_writer);
      }
    }

    private bool SplitsAreDifferent(LiveSplitState newState)
    {
      //If GameName / Category is different
      if (GameName != newState.Run.GameName || GameCategory != newState.Run.CategoryName)
      {
        GameName = newState.Run.GameName;
        GameCategory = newState.Run.CategoryName;
        return true;
      }

      //If number of splits is different
      if (newState.Run.Count != liveSplitState.Run.Count)
      {
        NumberOfSplits = newState.Run.Count;
        return true;
      }

      //Check if any split name is different.
      for (int splitIdx = 0; splitIdx < newState.Run.Count; splitIdx++)
      {
        if (newState.Run[splitIdx].Name != SplitNames[splitIdx])
        {

          SplitNames = new List<string>();

          foreach (var split in newState.Run)
          {
            SplitNames.Add(split.Name);
          }

          return true;
        }

      }



      return false;
    }
    public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
    {
      frame_count++;

      if (SplitsAreDifferent(state))
      {
        settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);

        ReloadLogFile();
      }

      if (settings.RecordImages && (frame_count % 3) == 0)
      {
        settings.StoreCaptureImage(GameName, GameCategory);
      }

      liveSplitState = state;
      /*
			liveSplitState = state;
			if (GameName != state.Run.GameName || GameCategory != state.Run.CategoryName)
			{
				//Reload settings for different game or category
				GameName = state.Run.GameName;
				GameCategory = state.Run.CategoryName;

				settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);
			}
			*/



      CaptureLoads();




    }

    public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
    {

    }

    public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
    {

    }

    public float VerticalHeight
    {
      get { return 0; }
    }

    public float MinimumWidth
    {
      get { return 0; }
    }

    public float HorizontalWidth
    {
      get { return 0; }
    }

    public float MinimumHeight
    {
      get { return 0; }
    }

    public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
    {
      return settings.GetSettings(document);
    }

    public System.Windows.Forms.Control GetSettingsControl(UI.LayoutMode mode)
    {
      return settings;
    }

    public void SetSettings(System.Xml.XmlNode settings)
    {
      this.settings.SetSettings(settings);
    }

    public void RenameComparison(string oldName, string newName)
    {
    }

    public void Dispose()
    {
      timer.CurrentState.OnStart -= timer_OnStart;

      if (log_file_writer != null)
      {
        if (log_file_writer.BaseStream != null)
        {
          log_file_writer.Flush();
          log_file_writer.Close();
          log_file_writer.Dispose();
        }
        log_file_writer = null;
      }

    }
  }
}

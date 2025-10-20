using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JavacLMD.Utils.Timers._DEMO
{
    public class TimerExample : MonoBehaviour
    {
        public float CountdownDuration = 10;
        public float IntervalTick = 2;
        public float DurationTick = 10;
        
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private TextMeshProUGUI intervalText;
        [SerializeField] private TextMeshProUGUI durationIntervalText;
        [SerializeField] private TextMeshProUGUI stopwatchText;

        [SerializeField] private Button stopwatchStartButton, stopwatchStopButton, stopwatchResetButton;
        
        
        private CountdownTimer _countdown;
        private IntervalTimer _interval;
        private DurationIntervalTimer _durationInterval;
        private StopwatchTimer _stopwatch;

        private void Start()
        {
            TimerManager.Instance.OnSystemRegister();

            SetupCountdownTimer();
            SetupIntervalTimer();
            SetupDurationIntervalTimer();
            SetupStopwatchTimer();
        }

        private void Update()
        {
            TimerManager.Instance.OnSystemUpdate(Time.deltaTime);
        }

        #region Timer Setup Methods

        private void SetupCountdownTimer()
        {
            _countdown = new CountdownTimer(CountdownDuration);

            _countdown.OnStart += () => Debug.Log("Countdown started");
            _countdown.OnCountdownTick += (remaining, duration) =>
            {
                string msg = $"Countdown: {remaining:F2}s / {duration:F2}s";
                if (countdownText) countdownText.text = msg;
                Debug.Log(msg);
            };
            _countdown.OnComplete += () =>
            {
                string msg = "Countdown: Completed!";
                if (countdownText) countdownText.text = msg;
                Debug.Log(msg);
            };

            _countdown.Start();
        }

        private void SetupIntervalTimer()
        {
            _interval = new IntervalTimer(IntervalTick);

            _interval.OnIntervalElapsed += () =>
            {
                string msg = $"Interval tick at {_interval.ElapsedTime:F2}s";
                if (intervalText) intervalText.text = msg;
                Debug.Log(msg);
            };

            _interval.Start();
        }

        private void SetupDurationIntervalTimer()
        {
            _durationInterval = new DurationIntervalTimer(IntervalTick, DurationTick);

            _durationInterval.OnIntervalElapsed += () =>
            {
                string msg = $"Duration Interval tick at {_durationInterval.ElapsedTime:F2}s";
                if (durationIntervalText) durationIntervalText.text = msg;
                Debug.Log(msg);
            };

            _durationInterval.OnComplete += () =>
            {
                string msg = "Duration Interval: Completed!";
                if (durationIntervalText) durationIntervalText.text = msg;
                Debug.Log(msg);
            };

            _durationInterval.Start();
        }

        private void SetupStopwatchTimer()
        {
            _stopwatch = new StopwatchTimer();
            _stopwatch.OnStopwatchTick += (elapsed) =>
            {
                string msg = $"Stopwatch Elapsed tick at {elapsed:F2}s";
                if (stopwatchText) stopwatchText.text = msg;
                Debug.Log(msg);
            };

            if (stopwatchStartButton)
            {
                stopwatchStartButton.onClick.AddListener(() =>
                {
                    _stopwatch.Start();
                    Debug.Log("Stopwatch Started");
                });
            }
            
            if (stopwatchStopButton)
            {
                stopwatchStopButton.onClick.AddListener(() =>
                {
                    _stopwatch.Stop();
                    Debug.Log("Stopwatch Stopped");
                });
            }
            
            if (stopwatchResetButton)
            {
                stopwatchResetButton.onClick.AddListener(() =>
                {
                    _stopwatch.ResetStopwatch();
                    Debug.Log("Stopwatch Reset");
                });
            }
            
            if (!stopwatchStartButton)
                _stopwatch.Start();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System.Windows.Forms.Integration;

namespace VlcMemoryLeaksDemo
{
    class VlcViewer
    {
        #region Fields

        private readonly WindowsFormsHost _formHost;

        private LibVLC _libVlc;

        private LibVLCSharp.Shared.MediaPlayer _mediaPlayer;

        private VideoView _videoView;

        private static bool _libVlcCoreLoaded;

        #endregion

        #region Properties and Indexers

        public UIElement Display => _formHost;

        #endregion

        #region Constructors

        public VlcViewer()
        {
            InitializeVlc();
            SetMedium();
            _formHost = new WindowsFormsHost {Child = _videoView};
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Dispose()
        {
            _videoView?.Dispose();
            if (_mediaPlayer != null)
            {
                _mediaPlayer.EndReached -= VlcOnEndReached;
                _mediaPlayer.EncounteredError -= OnEncounteredError;
                _mediaPlayer.Dispose();
            }
            _libVlc?.Dispose();

            if (_formHost == null)
                return;
            _formHost.Child = null;
            _formHost.Dispose();
        }

        public void Play()
        {
            _mediaPlayer.EncounteredError += OnEncounteredError;
            _mediaPlayer.EndReached += VlcOnEndReached;

            if (!_mediaPlayer.Play())
                OnMediaFailed();
        }

        public void SetMedium()
        {
            // TODO: Add some video URI here.
            _mediaPlayer.Media = new Media(_libVlc, new Uri(@""));
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
            }
        }

        #endregion

        #region Private Methods

        private void OnEncounteredError(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ => OnMediaFailed());
        }

        private void VlcOnEndReached(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ => OnMediaEnded());
        }

        /// <summary>
        /// Inicializuje VLC
        /// </summary>
        private void InitializeVlc()
        {
            if (!_libVlcCoreLoaded)
            {
                // Načtení nativní LibVlc - stačí jednou po celý běh aplikace
                Core.Initialize();
                _libVlcCoreLoaded = true;
            }
            _libVlc = new LibVLC();
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVlc);
            _videoView = new VideoView { MediaPlayer = _mediaPlayer };
        }

        /// <summary>
        /// Called when [media failed].
        /// </summary>
        protected virtual void OnMediaFailed()
        {
            // Do some stuff
        }

        /// <summary>
        /// Called when [media ended].
        /// </summary>
        protected virtual void OnMediaEnded()
        {
            // Do some stuff
        }

        #endregion
    }
}

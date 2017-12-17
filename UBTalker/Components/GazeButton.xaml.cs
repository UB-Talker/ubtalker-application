using System.Windows;
using System.Windows.Controls;
using EyeXFramework.Wpf;
using System.Windows.Input;
using System;

namespace UBTalker.Components
{

    /// <summary>
    /// Interaction logic for GazeButton.xaml
    /// </summary>
    public partial class GazeButton : UserControl
    {
        public static readonly int DEFAULT_FOCUS_DELAY = 350;
        public static readonly int DEFAULT_SELECTION_DELAY = 2000;
        public static readonly int DEFAULT_FONT_SIZE = 48;

        public static readonly DependencyProperty FocusDelayProperty = DependencyProperty.Register("GazeFocusDelay", typeof(int), typeof(GazeButton), new FrameworkPropertyMetadata(DEFAULT_FOCUS_DELAY));
        public static readonly DependencyProperty SelectionDelayProperty = DependencyProperty.Register("GazeSelectionDelay", typeof(int), typeof(GazeButton), new FrameworkPropertyMetadata(DEFAULT_SELECTION_DELAY));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(GazeButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty LinkProperty = DependencyProperty.Register("Link", typeof(string), typeof(GazeButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("InternalFontSize", typeof(int), typeof(GazeButton), new FrameworkPropertyMetadata(DEFAULT_FONT_SIZE));

        public static readonly RoutedEvent GazeFocusEvent = EventManager.RegisterRoutedEvent("GazeFocus", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GazeButton));
        public static readonly RoutedEvent GazeSelectionEvent = EventManager.RegisterRoutedEvent("GazeSelect", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GazeButton));

        /// <summary>
        /// How long in milliseconds it takes for this control to become focused
        /// </summary>
        public int GazeFocusDelay
        {
            get { return (int)GetValue(FocusDelayProperty); }
            set { SetValue(FocusDelayProperty, value); }
        }

        /// <summary>
        /// How long in milliseconds it takes for this control to become selected
        /// This includes the GazeFocusDelay
        /// </summary>
        public int GazeSelectionDelay
        {
            get { return (int)GetValue(SelectionDelayProperty); }
            set { SetValue(SelectionDelayProperty, value); }
        }

        /// <summary>
        /// Text on this control
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Additional metadata used to determine what the button is meant for
        /// </summary>
        public string Link
        {
            get { return (string)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }

        /// <summary>
        /// Font size
        /// </summary>
        public int InternalFontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// On focus
        /// </summary>
        public event RoutedEventHandler GazeFocus
        {
            add { AddHandler(GazeFocusEvent, value); }
            remove { RemoveHandler(GazeFocusEvent, value); }
        }

        /// <summary>
        /// On select
        /// </summary>
        public event RoutedEventHandler GazeSelect
        {
            add { AddHandler(GazeSelectionEvent, value); }
            remove { RemoveHandler(GazeSelectionEvent, value); }
        }

        public GazeButton()
        {
            InitializeComponent();
        }

        private void OnHasGazeChanged_Focus(object sender, RoutedEventArgs e)
        {
            var source = e.Source as Grid;
            if (source != null && source.GetHasGaze())
            {
                RaiseEvent(new RoutedEventArgs(GazeButton.GazeFocusEvent));
            }
        }

        private void OnHasGazeChanged_Select(object sender, RoutedEventArgs e)
        {
            var source = e.Source as Label;
            if (source != null && source.GetHasGaze())
            {
                RaiseEvent(new RoutedEventArgs(GazeButton.GazeSelectionEvent));
            }
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            // Only raised if this project is built in Debug mode
            // Used for debugging without an Eye Gaze device
            // When built in Release mode, this will no longer propagate
            RaiseEvent(new RoutedEventArgs(GazeButton.GazeSelectionEvent));
#endif
        }
    }
}
using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Challenge.WPF.Controls
{
    /// <summary>
    /// Our awesome custom control for selecting items!
    /// Loosely based around the MahApps FlipView control
    /// </summary>
    [TemplatePart(Name = "PART_Presenter", Type = typeof(TransitioningContentControl))]
    [TemplatePart(Name = "PART_ForwardButton", Type = typeof(Button))]
    public class CircleSelectView : Selector
    {
        private const string PART_Presenter = "PART_Presenter";
        private const string PART_ForwardButton = "PART_ForwardButton";
        private const string TwoItemArc = "TwoItemArc";
        private const string ThreeItemArc = "ThreeItemArc";
        private const string FourItemArc = "FourItemArc";
        private const string FiveItemArc = "FiveItemArc";
        private const string SixItemArc = "SixItemArc";

        private TransitioningContentControl _presenter = null;
        private Button _forwardButton = null;

        private Grid _twoItemArcGrid;
        private Grid _threeItemArcGrid;
        private Grid _fourItemArcGrid;
        private Grid _fiveItemArcGrid;
        private Grid _sixItemArcGrid;

        private Storyboard HideControlStoryboard = null;
        private Storyboard ShowControlStoryboard = null;

        private EventHandler HideControlStoryboard_CompletedHandler = null;

        /// <summary>
        /// To counteract the double Loaded event issue.
        /// </summary>
        private bool _loaded;

        private bool _controlsVisibilityOverride;

        static CircleSelectView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleSelectView), new FrameworkPropertyMetadata(typeof(CircleSelectView)));
        }

        public CircleSelectView()
        {
            this.Unloaded += FlipView_Unloaded;
            this.Loaded += FlipView_Loaded;
            this.MouseLeftButtonDown += FlipView_MouseLeftButtonDown;
            this.SelectionChanged += CircleSelectView_SelectionChanged;
        }

        void CircleSelectView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO:
            // Fire the animation for everything.
            // Preferably from the ViewModel, raise an event.
            // This is something that ReactiveUI is better at than Caliburn.Micro!
        }

        void FlipView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
        }

        ~CircleSelectView()
        {
            Dispatcher.Invoke(new EmptyDelegate(() =>
            {
                this.Loaded -= FlipView_Loaded;
            }));
        }

        private delegate void EmptyDelegate();

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is FlipViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new FlipViewItem() { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element != item)
                element.SetCurrentValue(DataContextProperty, item); //dont want to set the datacontext to itself. taken from MetroTabControl.cs

            base.PrepareContainerForItemOverride(element, item);
        }

        void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DetectControlButtonsStatus();
        }

        private void DetectControlButtonsStatus()
        {
            if (_controlsVisibilityOverride) return;

            if (_forwardButton == null
                || _twoItemArcGrid == null
                || _threeItemArcGrid == null
                || _fourItemArcGrid == null
                || _fiveItemArcGrid == null
                || _sixItemArcGrid == null) return;

            _forwardButton.Visibility = Visibility.Visible;

            // Allows us to 'switch' the view if the number of items changes.
            ResetVisibility(_twoItemArcGrid);
            ResetVisibility(_threeItemArcGrid);
            ResetVisibility(_fourItemArcGrid);
            ResetVisibility(_fiveItemArcGrid);
            ResetVisibility(_sixItemArcGrid);

            switch (Items.Count)
            {
                case 1:
                case 0:
                    _forwardButton.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    _twoItemArcGrid.Visibility = Visibility.Visible;
                    break;
                case 3:
                    _threeItemArcGrid.Visibility = Visibility.Visible;
                    break;
                case 4:
                    _fourItemArcGrid.Visibility = Visibility.Visible;
                    break;
                case 5:
                    _fiveItemArcGrid.Visibility = Visibility.Visible;
                    break;
                case 6:
                    _sixItemArcGrid.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ResetVisibility(UIElement arc)
        {
            if (arc.Visibility != Visibility.Collapsed)
            {

                arc.Visibility = Visibility.Collapsed;
            }
        }

        void FlipView_Loaded(object sender, RoutedEventArgs e)
        {
            /* Loaded event fires twice if its a child of a TabControl.
             * Once because the TabControl seems to initiali(z|s)e everything.
             * And a second time when the Tab (housing the FlipView) is switched to. */

            if (_forwardButton == null) //OnApplyTemplate hasn't been called yet.
                ApplyTemplate();

            if (_loaded) return; //Counteracts the double 'Loaded' event issue.

            _forwardButton.Click += forwardButton_Click;

            this.SelectionChanged += FlipView_SelectionChanged;
            this.PreviewKeyDown += FlipView_PreviewKeyDown;

            SelectedIndex = 0;

            DetectControlButtonsStatus();

            _loaded = true;
        }

        void FlipView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= FlipView_Unloaded;
            this.MouseLeftButtonDown -= FlipView_MouseLeftButtonDown;
            this.SelectionChanged -= FlipView_SelectionChanged;

            this.PreviewKeyDown -= FlipView_PreviewKeyDown;
            _forwardButton.Click -= forwardButton_Click;

            if (HideControlStoryboard_CompletedHandler != null)
                HideControlStoryboard.Completed -= HideControlStoryboard_CompletedHandler;

            _loaded = false;
        }

        void FlipView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    GoForward();
                    e.Handled = true;
                    break;
                case Key.Right:
                    GoForward();
                    e.Handled = true;
                    break;
            }

            if (e.Handled)
                this.Focus();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ShowControlStoryboard = ((Storyboard)this.Template.Resources["ShowControlStoryboard"]).Clone();
            HideControlStoryboard = ((Storyboard)this.Template.Resources["HideControlStoryboard"]).Clone();

            _presenter = GetTemplateChild(PART_Presenter) as TransitioningContentControl;
            _forwardButton = GetTemplateChild(PART_ForwardButton) as Button;

            _twoItemArcGrid = GetTemplateChild(TwoItemArc) as Grid;
            _threeItemArcGrid = GetTemplateChild(ThreeItemArc) as Grid;
            _fourItemArcGrid = GetTemplateChild(FourItemArc) as Grid;
            _fiveItemArcGrid = GetTemplateChild(FiveItemArc) as Grid;
            _sixItemArcGrid = GetTemplateChild(SixItemArc) as Grid;
        }

        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            SelectedIndex = 0;
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            DetectControlButtonsStatus();
        }

        void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            GoForward();
        }

        /// <summary>
        /// Changes the current to the next item.
        /// </summary>
        public void GoForward()
        {
            if (SelectedIndex < Items.Count - 1)
            {
                _presenter.Transition = LeftTransition;
                SelectedIndex++;
            }
            else // Loop it!
            {
                _presenter.Transition = LeftTransition;
                SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Brings the control buttons (next/previous) into view.
        /// </summary>
        public void ShowControlButtons()
        {
            _controlsVisibilityOverride = false;

            ExecuteWhenLoaded(this, () =>
                {
                    _forwardButton.Visibility = Visibility.Visible;
                });
        }

        /// <summary>
        /// Removes the control buttons (next/previous) from view.
        /// </summary>
        public void HideControlButtons()
        {
            _controlsVisibilityOverride = true;
            ExecuteWhenLoaded(this, () =>
                {
                    _forwardButton.Visibility = Visibility.Hidden;
                });
        }

        public static readonly DependencyProperty UpTransitionProperty = DependencyProperty.Register("UpTransition", typeof(TransitionType), typeof(CircleSelectView), new PropertyMetadata(TransitionType.Up));
        public static readonly DependencyProperty DownTransitionProperty = DependencyProperty.Register("DownTransition", typeof(TransitionType), typeof(CircleSelectView), new PropertyMetadata(TransitionType.Down));
        public static readonly DependencyProperty LeftTransitionProperty = DependencyProperty.Register("LeftTransition", typeof(TransitionType), typeof(CircleSelectView), new PropertyMetadata(TransitionType.LeftReplace));
        public static readonly DependencyProperty RightTransitionProperty = DependencyProperty.Register("RightTransition", typeof(TransitionType), typeof(CircleSelectView), new PropertyMetadata(TransitionType.RightReplace));

        public TransitionType UpTransition
        {
            get { return (TransitionType)GetValue(UpTransitionProperty); }
            set { SetValue(UpTransitionProperty, value); }
        }
        public TransitionType DownTransition
        {
            get { return (TransitionType)GetValue(DownTransitionProperty); }
            set { SetValue(DownTransitionProperty, value); }
        }
        public TransitionType LeftTransition
        {
            get { return (TransitionType)GetValue(LeftTransitionProperty); }
            set { SetValue(LeftTransitionProperty, value); }
        }
        public TransitionType RightTransition
        {
            get { return (TransitionType)GetValue(RightTransitionProperty); }
            set { SetValue(RightTransitionProperty, value); }
        }

        private static void ExecuteWhenLoaded(CircleSelectView flipview, Action body)
        {
            if (flipview.IsLoaded)
                System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(new EmptyDelegate(() => body()));
            else
            {
                RoutedEventHandler handler = null;
                handler = (o, a) =>
                {
                    flipview.Loaded -= handler;
                    System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(new EmptyDelegate(() => body()));
                };

                flipview.Loaded += handler;
            }
        }
    }

    public class FlipViewItem : ContentControl
    {
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Api.TL;
using Unigram.Converters;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Unigram.Controls
{
    public class PeriodTimer : Control
    {
        private ProgressBarRingSlice Indicator;

        private Storyboard _angleStoryboard;

        public PeriodTimer()
        {
            DefaultStyleKey = typeof(PeriodTimer);
        }

        protected override void OnApplyTemplate()
        {
            Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            Indicator = (ProgressBarRingSlice)GetTemplateChild("Indicator");
            Indicator.EndAngle = 359;

            _angleStoryboard = new Storyboard();
            var angleAnimation = new DoubleAnimation();
            angleAnimation.Duration = TimeSpan.FromSeconds(0.25);
            angleAnimation.EnableDependentAnimation = true;

            Storyboard.SetTarget(angleAnimation, Indicator);
            Storyboard.SetTargetProperty(angleAnimation, "StartAngle");

            _angleStoryboard.Children.Add(angleAnimation);
            _angleStoryboard.Completed += OnAngleStoryboardCompleted;

            OnValueChanged(Value);
        }

        private void OnAngleStoryboardCompleted(object sender, object e)
        {
            if (Value == null)
            {
                Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        #region Value

        public TLMessage Value
        {
            get { return (TLMessage)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(TLMessage), typeof(PeriodTimer), new PropertyMetadata(null, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PeriodTimer)d).OnValueChanged(e.NewValue as TLMessage);
        }

        #endregion

        private void OnValueChanged(TLMessage newValue)
        {
            if (Indicator == null)
            {
                return;
            }

            if (newValue == null)
            {
                Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                Visibility = Visibility.Visible;
            }

            var geoLiveMedia = newValue.Media as TLMessageMediaGeoLive;
            if (geoLiveMedia == null)
            {
                return;
            }

            var value = BindConvert.Current.DateTime(newValue.Date + geoLiveMedia.Period);
            var difference = value - DateTime.Now;

            _angleStoryboard.SkipToFill();

            var angleAnimation = (DoubleAnimation)_angleStoryboard.Children[0];
            angleAnimation.From = 359 - (difference.TotalSeconds / geoLiveMedia.Period * 359);
            angleAnimation.To = 359;
            angleAnimation.Duration = difference;
            _angleStoryboard.Begin();

            //double value;
            ////if (oldValue > 0.0 && oldValue < 1.0 && newValue == 0.0)
            ////{
            ////    value = 359.0;
            ////}
            ////else
            //{
            //    value = newValue * 359.0;
            //    if (value < 0.0)
            //    {
            //        value = 0.0;
            //    }
            //    else if (value > 359.0)
            //    {
            //        value = 359.0;
            //    }
            //}
            //if (value != Indicator.StartAngle)
            //{
            //    if (value > 0.0 && value < 359.0)
            //    {
            //        Visibility = Windows.UI.Xaml.Visibility.Visible;
            //    }

            //    if (value > Indicator.StartAngle)
            //    {
            //        _angleStoryboard.SkipToFill();

            //        var angleAnimation = (DoubleAnimation)_angleStoryboard.Children[0];
            //        angleAnimation.To = value;
            //        angleAnimation.Duration = DueTime - DateTime.Now;
            //        _angleStoryboard.Begin();
            //    }
            //}
        }
    }
}
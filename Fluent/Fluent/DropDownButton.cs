﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Fluent
{
    /// <summary>
    /// Represents drop down button
    /// </summary>
    [ContentProperty("PopupContent")]
    public class DropDownButton: RibbonControl
    {
        #region Fields

        /// <summary>
        /// Popup
        /// </summary>
        private RibbonPopup popup;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets button large icon
        /// </summary>
        public ImageSource LargeIcon
        {
            get { return (ImageSource)GetValue(LargeIconProperty); }
            set { SetValue(LargeIconProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for SmallIcon.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty LargeIconProperty =
            DependencyProperty.Register("LargeIcon", typeof(ImageSource), typeof(DropDownButton), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets whether button has triangle
        /// </summary>
        public bool HasTriangle
        {
            get { return (bool) GetValue(HasTriangleProperty); }
            set { SetValue(HasTriangleProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for HasTriangle. 
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty HasTriangleProperty =
            DependencyProperty.Register(
                "HasTriangle", typeof(bool), typeof(DropDownButton), new UIPropertyMetadata(true));

        /// <summary>
        /// Gets or sets whether popup is opened
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(DropDownButton), new UIPropertyMetadata(false,OnIsOpenChanged));

        /// <summary>
        /// Gets or sets popup content
        /// </summary>
        public object PopupContent
        {
            get { return (object)GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for PopupContent.  
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PopupContentProperty =
            DependencyProperty.Register("PopupContent", typeof(object), typeof(DropDownButton), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets an enumerator for logical child elements of this element.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(PopupContent);
                return list.GetEnumerator();
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Static constructor
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810")]
        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton),
                                                     new FrameworkPropertyMetadata(typeof(DropDownButton)));                       


        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DropDownButton()
        {
            AddHandler(RibbonControl.ClickEvent, new RoutedEventHandler(OnClick));
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Invoked when an unhandled System.Windows.UIElement.PreviewMouseLeftButtonDown routed event 
        /// reaches an element in its route that is derived from this class. Implement this method to add 
        /// class handling for this event.
        /// </summary>
        /// <param name="e">The System.Windows.Input.MouseButtonEventArgs that contains the event data. 
        /// The event data reports that the left mouse button was pressed.</param>
        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((popup != null)&&(!popup.IsOpen))
            {
                popup.IsOpen = !popup.IsOpen;
                if (IsOpen) Mouse.Capture(popup, CaptureMode.Element);
                e.Handled = true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes 
        /// call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (popup != null) RemoveLogicalChild(popup);
            popup = GetTemplateChild("PART_Popup") as RibbonPopup;
            if(popup!=null)
            {
                if (popup.Parent != null) (popup.Parent as Panel).Children.Remove(popup);
                AddLogicalChild(popup);
                Binding binding = new Binding("IsOpen");
                binding.Mode = BindingMode.TwoWay;
                binding.Source = this;
                popup.SetBinding(Popup.IsOpenProperty, binding);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles IsOpen property changes
        /// </summary>
        /// <param name="d">Object</param>
        /// <param name="e">The event data</param>
        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropDownButton ribbon = (DropDownButton)d;

            if (ribbon.IsOpen)
            {                
                ribbon.IsHitTestVisible = false;
            }
            else
            {
                ribbon.IsHitTestVisible = true;
            }
        }

        #endregion
    }


}
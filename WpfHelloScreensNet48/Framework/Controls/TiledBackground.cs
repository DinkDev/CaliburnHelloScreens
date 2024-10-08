﻿namespace Caliburn.Micro.HelloScreens.Framework.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// This class was borrowed from the SL4 JetPack Theme.
    /// </summary>
    public class TiledBackground : UserControl
    {
        public static readonly DependencyProperty SourceUriProperty = DependencyProperty.Register(
            nameof(SourceUri),
            typeof(Uri),
            typeof(TiledBackground),
            new PropertyMetadata(null, OnSourceUriChanged)
        );

        private BitmapImage _bitmap;
        private int _lastHeight;
        private int _lastWidth;
        private WriteableBitmap _sourceBitmap;
        private readonly Image _tiledImage = new Image();

        public TiledBackground()
        {
            _tiledImage.Stretch = Stretch.None;
            Content = _tiledImage;
            SizeChanged += TiledBackgroundSizeChanged;
        }

        /// <summary>
        /// A description of the property.
        /// </summary>
        public Uri SourceUri
        {
            get => (Uri)GetValue(SourceUriProperty);
            set => SetValue(SourceUriProperty, value);
        }

        void TiledBackgroundSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateTiledImage();
        }

        void UpdateTiledImage()
        {
            if (_sourceBitmap != null)
            {
                var width = (int)Math.Ceiling(ActualWidth);
                var height = (int)Math.Ceiling(ActualHeight);

                // only regenerate the image if the width/height has grown
                if (width >= _lastWidth || height >= _lastHeight)
                {
                    _lastWidth = width;
                    _lastHeight = height;

                    var final = new WriteableBitmap(_bitmap);

                    //for (var x = 0; x < final.PixelWidth; x++)
                    //{
                    //    for (var y = 0; y < final.PixelHeight; y++)
                    //    {
                    //        var tiledX = (x % _sourceBitmap.PixelWidth);
                    //        var tiledY = (y % _sourceBitmap.PixelHeight);
                    //        var writeRec = new Int32Rect(x, y, final.PixelWidth, final.PixelHeight);
                    //
                    //        //final.Pixels[y * final.PixelWidth + x] = sourceBitmap.Pixels[tiledY * sourceBitmap.PixelWidth + tiledX];
                    //    }
                    //}

                    _tiledImage.Source = final;
                }
            }
        }

        static void OnSourceUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TiledBackground)d).OnSourceUriChanged(e);
        }

        protected virtual void OnSourceUriChanged(DependencyPropertyChangedEventArgs e)
        {
            _bitmap = new BitmapImage(e.NewValue as Uri ?? throw new InvalidOperationException())
                { CreateOptions = BitmapCreateOptions.None };
            _bitmap.Changed += BitmapImageOpened;
        }

        void BitmapImageOpened(object sender, EventArgs e)
        {
            _sourceBitmap = new WriteableBitmap(_bitmap);
            UpdateTiledImage();
        }
    }
}
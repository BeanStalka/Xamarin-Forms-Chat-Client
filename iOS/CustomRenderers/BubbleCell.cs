﻿using System;
using System.Drawing;
using Foundation;
using UIKit;
using CoreGraphics;

namespace XamarinChat.iOS
{
	public class BubbleCell : UITableViewCell
	{
		public static NSString KeyLeft = new NSString("BubbleElementLeft");
		public static NSString KeyRight = new NSString("BubbleElementRight");
		public static UIImage bleft, bright, left, right;
		public static UIFont font = UIFont.SystemFontOfSize(14);
		UIView view;
		UIView imageView;
		UILabel label;
		bool isLeft;

		static BubbleCell()
		{
			bright = UIImage.FromFile("green.png");
			bleft = UIImage.FromFile("grey.png");

			// buggy, see https://bugzilla.xamarin.com/show_bug.cgi?id=6177
			//left = bleft.CreateResizableImage (new UIEdgeInsets (10, 16, 18, 26));
			//right = bright.CreateResizableImage (new UIEdgeInsets (11, 11, 17, 18));
			left = bleft.StretchableImage(26, 16);
			right = bright.StretchableImage(11, 11);
		}

		public BubbleCell(bool isLeft)
			: base(UITableViewCellStyle.Default, isLeft ? KeyLeft : KeyRight)
		{
			var rect = new RectangleF(0, 0, 1, 1);
			this.isLeft = isLeft;
			view = new UIView(rect);
			imageView = new UIImageView(isLeft ? left : right);
			view.AddSubview(imageView);
			label = new UILabel(rect)
			{
				LineBreakMode = UILineBreakMode.WordWrap,
				Lines = 0,
				Font = font,
				BackgroundColor = UIColor.Clear
			};
			view.AddSubview(label);
			ContentView.Add(view);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			var frame = ContentView.Frame;
			var size = GetSizeForText(this, label.Text) + BubblePadding;
			imageView.Frame = new CGRect (isLeft ? 10 : frame.Width - size.Width - 10, frame.Y, size.Width,size.Height);
//			imageView.Frame = new RectangleF(new PointF(isLeft ? 10 : frame.Width - size.Width - 10, frame.Y), size);
			view.SetNeedsDisplay();
			frame = imageView.Frame;
			var noBubbleSize = size - BubblePadding;
			label.Frame = new CGRect (frame.X + (isLeft ? 12 : 8), frame.Y + 6, noBubbleSize.Width,noBubbleSize.Height);
//			label.Frame = new RectangleF(new PointF(frame.X + (isLeft ? 12 : 8), frame.Y + 6), size - BubblePadding);
		}

		static internal SizeF BubblePadding = new SizeF(22, 16);

		static internal SizeF GetSizeForText(UIView tv, string text)
		{
			//return tv.StringSize(text, font, new SizeF(tv.Bounds.Width * .7f - 10 - 22, 99999));

			NSString s = new NSString (text);
			var size = s.StringSize(font, new CGSize(tv.Bounds.Width * .7f - 10 - 22, 99999));
			return new SizeF ((float)size.Width, (float)size.Height);
		}

		public void Update(string text)
		{
			label.Text = text;
			SetNeedsLayout();
		}

		public float GetHeight(UIView tv)
		{
			return GetSizeForText(tv, label.Text).Height + BubblePadding.Height;
		}
	}
}


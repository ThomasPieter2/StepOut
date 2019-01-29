using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StepOut.Droid;
using StepOut;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(CustomSliderRenderer), typeof(SliderRendererAndroid))]
namespace StepOut.Droid
{
    public class SliderRendererAndroid : SliderRenderer
    {
        public SliderRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                string colorSlider = "#3C4459";
                Control.ProgressDrawable.SetColorFilter(Xamarin.Forms.Color.FromHex(colorSlider).ToAndroid(), PorterDuff.Mode.SrcIn);

                // Set Progress bar Thumb color
                Control.Thumb.SetColorFilter(
                    Xamarin.Forms.Color.FromHex("#3C4459").ToAndroid(),
                    PorterDuff.Mode.SrcIn);

                //if (!string.IsNullOrEmpty(Element.ThumbImage))
                //    Control.SetThumb(defaultthumb);
                //else
                //    Control.SetThumb(Context.GetDrawable("circle.png"));


                Control.ProgressDrawable.SetColorFilter(
                    new PorterDuffColorFilter(
                    Xamarin.Forms.Color.FromHex("#3C4459").ToAndroid(),
                    PorterDuff.Mode.SrcIn));

                //Set Background Progress bar color
                Control.ProgressBackgroundTintList
                       = ColorStateList.ValueOf(
                        Xamarin.Forms.Color.FromHex("#BEC2CC").ToAndroid());
                Control.ProgressBackgroundTintMode
                       = PorterDuff.Mode.SrcIn;
            }
        }
    }
}
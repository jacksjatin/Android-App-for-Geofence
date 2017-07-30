using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace GeoFencngDemo
{
    [Activity(Label = "VehicleDetailsActivity")]
    public class VehicleDetailsActivity : Activity
    {

        private VehicleDetls vehicle;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VehicleDetails);
            var imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            imageView.SetImageResource(Resource.Drawable.car);

            vehicle = JsonConvert.DeserializeObject<VehicleDetls>(Intent.GetStringExtra("VehicleDetails"));

            FindViewById<TextView>(Resource.Id.textView1).Text = vehicle.selMake;

            FindViewById<TextView>(Resource.Id.textView2).Text = vehicle.selModel;
        }
    }
}
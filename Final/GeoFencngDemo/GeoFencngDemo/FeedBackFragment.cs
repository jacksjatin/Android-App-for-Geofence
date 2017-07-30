
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GeoFencngDemo
{
	public class FeedBackFragment : Fragment
	{
		EditText onNoText, whatWentWrongServiceText;
		TextView onNoLabel, whatWentWrongServiceLabel;
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.FeedBackLayout, container, false);
			onNoLabel =  view.FindViewById<TextView>(Resource.Id.whatWentWrongLabel);
			onNoText = view.FindViewById<EditText>(Resource.Id.whatWentWrongText);
			whatWentWrongServiceLabel = view.FindViewById<TextView>(Resource.Id.whatWentWrongServiceLabel);
			whatWentWrongServiceText = view.FindViewById<EditText>(Resource.Id.whatWentWrongServiceText);


			onNoLabel.Visibility = Android.Views.ViewStates.Gone;
			onNoText.Visibility = Android.Views.ViewStates.Gone;

			whatWentWrongServiceLabel.Visibility = Android.Views.ViewStates.Gone;
			whatWentWrongServiceText.Visibility = Android.Views.ViewStates.Gone;

			RadioButton radio_wishPos = view.FindViewById<RadioButton>(Resource.Id.radio_wishPos);
			RadioButton radio_wishNeg = view.FindViewById<RadioButton>(Resource.Id.radio_wishNeg);

			RadioButton radio_serviceYes = view.FindViewById<RadioButton>(Resource.Id.radio_serviceYes);
			RadioButton radio_serviceNo = view.FindViewById<RadioButton>(Resource.Id.radio_serviceNo);


			radio_wishPos.Click += RadioButtonWish;
			radio_wishNeg.Click += RadioButtonWish;

			radio_serviceYes.Click += RadioButtonService;
			radio_serviceNo.Click += RadioButtonService;
			return view;
        
        
        }
		private void RadioButtonWish(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
				if (rb.Text == "No")
			{
				onNoLabel.Visibility = Android.Views.ViewStates.Visible;
				onNoText.Visibility = Android.Views.ViewStates.Visible;
			}

			if (rb.Text == "Yes")
			{
				onNoLabel.Visibility = Android.Views.ViewStates.Gone;
				onNoText.Visibility = Android.Views.ViewStates.Gone;
			}

		}
		private void RadioButtonService(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
					if (rb.Text == "No")
			{
				whatWentWrongServiceLabel.Visibility = Android.Views.ViewStates.Visible;
				whatWentWrongServiceText.Visibility = Android.Views.ViewStates.Visible;
			}

			if (rb.Text == "Yes")
			{
				whatWentWrongServiceLabel.Visibility = Android.Views.ViewStates.Gone;
				whatWentWrongServiceText.Visibility = Android.Views.ViewStates.Gone;
			}

		}

	}
}


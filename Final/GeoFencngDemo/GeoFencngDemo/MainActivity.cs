using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget; 
using Android.Support.V7.App;
using Android.Views;
using Android.App; 
using System;
using Android.Content;
using Android.Runtime;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using System.Collections.Generic;
using Xamarin.Facebook.Login.Widget;
using Android.Preferences;
using Xamarin.Facebook.Login;
using Android.Graphics;
using System.Net;
using SupportFragment = Android.Support.V4.App.Fragment;


namespace GeoFencngDemo
{
	[Activity(Label = "Dealers Inn", Icon = "@drawable/icon", Theme = "@style/MyTheme" ,MainLauncher = true)]
	public class MainActivity : AppCompatActivity
    {
        ISharedPreferences prefs;
        private FeedBackFragment feedBackFragment;
        private DealerSearchFragment dealerSearchFragment;
        private Fragment mCurrentFragment = new Fragment();
        private TextView TxtFirstName;


		DrawerLayout drawerLayout;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			//Creating shared preference and storing the user name in the shared preference
			 prefs = Application.Context.GetSharedPreferences("User_Profile", FileCreationMode.Private);
			var UserName = prefs.GetString("Logged_In_UserName" ,null);
	  

            //to-do : if user's password is changed then how to authenticate
           
            if (UserName == null)
			{
				var intent = new Intent(this, typeof(LoginActivity));
				intent.PutExtra("UserSharedPrefenece", "User_Profile");
				StartActivity(intent);
			}
			
        
			base.OnCreate (savedInstanceState);   
			SetContentView (Resource.Layout.Main);
			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout); 

			// Init toolbar
			var toolbar = FindViewById<SupportToolbar>(Resource.Id.app_bar);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetTitle (Resource.String.app_name);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);

			// Attach item selected handler to navigation view
			var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
			navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

			// Create ActionBarDrawerToggle button and add it to the toolbar
			var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
			drawerLayout.SetDrawerListener(drawerToggle);
			drawerToggle.SyncState();

            // available fragments
            feedBackFragment = new FeedBackFragment();
            dealerSearchFragment = new DealerSearchFragment();


			//load default home screen
			var ft= FragmentManager.BeginTransaction ();
			ft.AddToBackStack (null);
			ft.Add (Resource.Id.HomeFrameLayout,dealerSearchFragment);
            ft.Add(Resource.Id.HomeFrameLayout, feedBackFragment);
            ft.Hide(feedBackFragment);
            mCurrentFragment = dealerSearchFragment;

			ft.Commit ();

			//string Name = "Hi," + UserName;
			//TxtFirstName = FindViewById<TextView>(Resource.Id.topLine);
			//TxtFirstName.Text = Name;

			//........start Geofence activity right after  user app is logied in........
		
           /* Boolean userVal= prefs.GetBoolean("Is_First_TimeLogin",false );
            if(!userVal)
            {
				var intent = new Intent(this, typeof(MainActivityGeoFence));
				StartActivity(intent); 
            }
            */  


            //send explicit message
           // SendNotification("hello madhu", "owl", "http://help.adobe.com/en_US/as3/mobile/images/or_object_surface_bitmap_inmemory.png" ,"lastword");

			//initilize new fragments 
			

		}
		//define custom title text
		protected override void OnResume ()
		{ 
			SupportActionBar.SetTitle (Resource.String.app_name);
			base.OnResume ();
		}
		


        //define action for navigation menu selection
		void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
		{
			switch (e.MenuItem.ItemId)
			{
			case (Resource.Id.nav_home):
					ShowFragment(dealerSearchFragment);
					break;
			case (Resource.Id.nav_messages): 
					Toast.MakeText(this, "Message selected!", ToastLength.Short).Show(); 
				break;
			case (Resource.Id.nav_friends):
				// React on 'Friends' selection
				break;
				case (Resource.Id.nav_Feedback):
                    {
                        ShowFragment(feedBackFragment);
                        break;
                    }

			}
			// Close drawer
			drawerLayout.CloseDrawers();
		}

		private void ShowFragment(Fragment fragment)
		{

			if (fragment.IsVisible)
			{
				return;
			}

			var trans = FragmentManager.BeginTransaction();

			fragment.View.BringToFront();
			mCurrentFragment.View.BringToFront();

			trans.Hide(mCurrentFragment);
			trans.Show(fragment);

			trans.AddToBackStack(null);
			//mStackFragments.Push(mCurrentFragment);
			trans.Commit();

			mCurrentFragment = fragment;

		}


		//notification call
		public void SendNotification(string notificationDetails , string messgage ,string url ,string title)
		{
			var notificationIntent = new Intent(ApplicationContext, typeof(MainActivity));

			var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
			stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
			stackBuilder.AddNextIntent(notificationIntent);

			var notificationPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

			var builder = new NotificationCompat.Builder(this);

			var imageBitmap = GetImageBitmapFromUrl(url);
			builder.SetSmallIcon(Resource.Drawable.Icon)
			.SetLargeIcon(imageBitmap)
			.SetColor(Color.Red)
			.SetContentTitle(notificationDetails)
			.SetContentText(title)
			.SetContentIntent(notificationPendingIntent)
			.SetStyle(new NotificationCompat.BigPictureStyle()
			.BigPicture(imageBitmap)
			.SetSummaryText(messgage)).Build();

			builder.SetAutoCancel(true);

			var mNotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
			mNotificationManager.Notify(0, builder.Build());


		}

		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}

		//add custom icon to toolbar
		public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.action_menu,menu);
			if (menu != null) { 
				menu.FindItem (Resource.Id.action_refresh).SetVisible (true); 
				menu.FindItem (Resource.Id.action_attach).SetVisible (false);
			}
			return base.OnCreateOptionsMenu (menu);
		}

		//define action for tolbar icon press
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				//this.Activity.Finish();
				return true; 
			case Resource.Id.action_attach:
				//FnAttachImage();
				return true;
             case  Resource.Id.action_refresh:
                    {
                        var intent = new Intent(this, typeof(LoginActivity));
                        ISharedPreferences perefs = Application.Context.GetSharedPreferences("User_Profile", FileCreationMode.Private);
						ISharedPreferencesEditor editor = perefs.Edit();
                        editor.Remove("Logged_In_UserName");
						editor.Apply();
                        intent.PutExtra("isLogOut", "true");
                        StartActivity(intent);
                        return true;
                    }
			default:
				return base.OnOptionsItemSelected(item);
			}
		}
		//to avoid direct app exit on backpreesed and to show fragment from stack
		public override void OnBackPressed ()
		{
			if(FragmentManager.BackStackEntryCount!= 0) {
				FragmentManager.PopBackStack ();// fragmentManager.popBackStack();
			} else {
				base.OnBackPressed ();
			}  
		}

        //redirect to the feedback form once message is received




	}
}





using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Java.Security;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Login;
using System.Collections.Generic;
using Android.Graphics;
using Java.Net;
using Android.Provider;
using Xamarin.Facebook.Share.Model;
using System.IO;
using Xamarin.Facebook.Share.Widget;
using Xamarin.Facebook.Share;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using Android.Webkit;

namespace GeoFencngDemo
{
	[Activity(Label = "Dealers Inn" ,Icon = "@drawable/logo")]
	public class LoginActivity : Activity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
	{
		private ICallbackManager mCallBackManager;
		private MyProfileTracker mProfileTracker;
        private FacebookResult result;
		private ProfilePictureView mProfilePic;
	    string userInfoService = "http://172.31.109.75/nodesrvc";

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			FacebookSdk.SdkInitialize(this.ApplicationContext);
            String mUserProfileName = Intent.GetStringExtra("isLogOut") ?? null;
            if(mUserProfileName!=null)
				LoginManager.Instance.LogOut();
                


			mProfileTracker = new MyProfileTracker();
			mProfileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
			mProfileTracker.StartTracking();

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.loginPage);
			WebView webLoadingIcon = FindViewById<WebView>(Resource.Id.welcomeImage);
			// expects to find the 'loading_icon_small.gif' file in the 'root' of the assets folder, compiled as AndroidAsset.
			webLoadingIcon.LoadUrl(string.Format("http://www.animatedimages.org/data/media/67/animated-car-image-0049.gif"));
			// this makes it transparent so you can load it over a background
			webLoadingIcon.SetBackgroundColor(new Color(0, 0, 0, 0));
			webLoadingIcon.SetLayerType(LayerType.Software, null);

			Button faceBookButton = FindViewById<Button>(Resource.Id.button);

			LoginButton button = FindViewById<LoginButton>(Resource.Id.login_button);

			button.SetReadPermissions(new List<string> { "public_profile", "user_friends", "email" });

			mCallBackManager = CallbackManagerFactory.Create();

			button.RegisterCallback(mCallBackManager, this);

		}
		
		public void OnCompleted(Org.Json.JSONObject json, GraphResponse response)
		{
			string data = json.ToString();
		    result = JsonConvert.DeserializeObject<FacebookResult>(data);
		}

		void client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
		{
			throw new NotImplementedException();
		}

		void mProfileTracker_mOnProfileChanged(object sender, OnProfileChangedEventArgs e)
		{
			
            if (e.mProfile != null)
			{
				try
				{ 
				    string UserName = e.mProfile.FirstName;
                  /*
                    //service call to store the user email id
                    if (result.email != null)
                    {
                        postLoginInfo(e.mProfile.FirstName, e.mProfile.LastName ,result.email);
                    }
                    */
                    //	mProfilePic.ProfileId = e.mProfile.Id;
					string mUserProfileName = Intent.GetStringExtra("UserSharedPrefenece") ?? null;
					ISharedPreferences prefs = Application.Context.GetSharedPreferences(mUserProfileName, FileCreationMode.Private);
					ISharedPreferencesEditor editor = prefs.Edit();
					editor.PutString("Logged_In_UserName", UserName);
					editor.Apply();
					var intent = new Intent(this, typeof(MainActivity));
					intent.PutExtra("User_Name", UserName);
					StartActivity(intent);

				}

				catch (Exception ex)
				{
					//Handle error
				}
			}

			else
			{
				//the user must have logged out
				
			}
		}

        public void getMail()
        {

            try
            {
                GraphRequest request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);
                Bundle parameters = new Bundle();
                parameters.PutString("fields", "id,name,email");
                request.Parameters = parameters;
                request.ExecuteAsync();
			}
			catch (Exception ex)
			{
				//Handle error
			}
         
		}

     /* public void postLoginInfo(string ufirstName ,string uLastName , string uEmailId ,string uContact)
		{
		    	var request = (HttpWebRequest)WebRequest.Create(userInfoService +"/addProfile");
				request.ContentType = "application/json";
				request.Method = "POST";

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					string json = new JavaScriptSerializer().Serialize(new
					{
						firstName = ufirstName,
						lastName = uLastName,
						emailId = uEmailId,
						contact = uContact
					});

					streamWriter.Write(json);
				}

				var response = (HttpWebResponse)request.GetResponse();
				using (var streamReader = new StreamReader(response.GetResponseStream()))
				{
					var result = streamReader.ReadToEnd();
				}

            
        }
        */

		public void OnCancel()
		{
			//throw new NotImplementedException();
		}

		public void OnError(FacebookException error)
		{
			//throw new NotImplementedException();
		}

		public void OnSuccess(Java.Lang.Object result)
		{
			LoginResult loginResult = result as LoginResult;
			Console.WriteLine(AccessToken.CurrentAccessToken.UserId);
			

		}


		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
		}

		protected override void OnDestroy()
		{
			mProfileTracker.StopTracking();
			base.OnDestroy();
		}
	}

	public class MyProfileTracker : ProfileTracker
	{
		public event EventHandler<OnProfileChangedEventArgs> mOnProfileChanged;

		protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
		{
			if (mOnProfileChanged != null)
			{
				mOnProfileChanged.Invoke(this, new OnProfileChangedEventArgs(newProfile));
			}
		}
	}

	public class OnProfileChangedEventArgs : EventArgs
	{
		public Profile mProfile;

		public OnProfileChangedEventArgs(Profile profile) { mProfile = profile; }
	}

}

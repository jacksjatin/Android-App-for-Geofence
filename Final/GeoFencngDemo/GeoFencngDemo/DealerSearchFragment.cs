using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Content;


namespace GeoFencngDemo{

    [Activity(Label = "ListViewApp", Icon = "@drawable/icon")]
    public class DealerSearchFragment :Fragment, SearchView.IOnQueryTextListener
    {

       
        ListView listView;
        List<TableItem> lstitems;
        SearchView search;
        public VehicleDetls vobj;
        public string selmake;
        public string selmodel;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.DealerSearch, container, false);

			listView = view.FindViewById<ListView>(Resource.Id.listView1);
			//items = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };

			GetItemsList();
			listView.Adapter = new HomeScreenAdapter(this.Activity, lstitems);
			listView.ItemClick += OnListItemClick;  // to be defined

			search = view.FindViewById<SearchView>(Resource.Id.searchView1);
			search.SetQueryHint("Type model name ");
			search.SetIconifiedByDefault(false);
			search.SetOnQueryTextListener(this);
			search.ClearFocus();
            return view;

		}

        public bool OnQueryTextSubmit(string query)
        {

            string[] qrylst = query.Split(' ');
           
           
            search.ClearFocus();
            return true;
        }

        public bool SetOnQueryTextListener(string query)
        {
            return false;
        }

		public List<TableItem> GetItemsList()
		{
			lstitems = new List<TableItem>();
			var cnt = "[{\"_id\":\"597045ae79f3d67e570298a7\",\"DealerName\":\"Lexus Of Belveue\",\"Address\":\"Hitech City\",\"dlrlatitued\":\"17.441039\",\"dlrlongitued\":\"78.377249\",\"inventorys\":[{\"vehicle_make\":\"Audi\",\"vehicle_model\":\"A6\"},{\"vehicle_make\":\"Audi\",\"vehicle_model\":\"A7\"},{\"vehicle_make\":\"Audi\",\"vehicle_model\":\"A5\"},{\"vehicle_make\":\"Volkswagen\",\"vehicle_model\":\"Jetta Sedan\"}]},{\"_id\":\"596bb5a279f3d67e5702986c\",\"DealerName\":\"Gmps of Rodgerland\",\"Address\":\"TCS\",\"dlrlatitued\":\"17.444198\",\"dlrlongitued\":\"78.377860\",\"inventorys\":[{\"vehicle_make\":\"Audi\",\"vehicle_model\":\"A6\"},{\"vehicle_make\":\"Audi\",\"vehicle_model\":\"A5\"},{\"vehicle_make\":\"Volkswagen\",\"vehicle_model\":\"Jetta Sedan\"}]}]";
			try
			{
				lstitems = JsonConvert.DeserializeObject<List<TableItem>>(cnt);

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			//selmake = make;
			//selmodel = model;
			return lstitems;
		}

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;

            VehicleDetls vd = new VehicleDetls();

            TableItem t = lstitems[e.Position];

            foreach (var item in t.inventorys)
            {
                if(item.vehicle_model==selmodel  && item.vehicle_make== selmake)
                {
                    vd.selMake = item.vehicle_make;
                    vd.selModel = item.vehicle_model;

                }

            }
            

            Android.Widget.Toast.MakeText(this.Activity, t.DealerName, Android.Widget.ToastLength.Short).Show();
            var intent = new Intent(this.Activity, typeof(VehicleDetailsActivity));
            intent.PutExtra("VehicleDetails", JsonConvert.SerializeObject(vd));
            StartActivity(intent);

        }

        public bool OnQueryTextChange(string newText)
        {
            //throw new NotImplementedException();
            return true;
        }

       
    }

   
    
}


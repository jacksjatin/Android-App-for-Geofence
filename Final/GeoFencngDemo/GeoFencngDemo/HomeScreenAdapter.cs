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

namespace GeoFencngDemo
{
    public class HomeScreenAdapter : BaseAdapter<TableItem>
    {
        List<TableItem> items;
        Activity context;
        public HomeScreenAdapter(Activity context, List<TableItem> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override TableItem this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.DealerName;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Address;
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.Icon);
            return view;
        }
    }

    public class TableItem
    {
       public string DealerName { get; set; }
       public string Address { get; set; }

        public string dlrlatitued { get; set; }
        public string dlrlongitued { get; set; }
        public List<inventorys> inventorys { get; set; }
    }

    public class VehicleDetls
    {
        public string selModel { get; set; }
        public string selMake { get; set; }
        //public string cost { get; set; }
        //public string description { get; set; }

    }


    public class inventorys
    {
        public string vehicle_make { get; set; }
        public string vehicle_model { get; set; }
    }
    //public class HomeScreenAdapter : BaseAdapter<string>
    //{
    //    string[] items;
    //    Activity context;
    //    public HomeScreenAdapter(Activity context, string[]  items)
    //        : base()
    //    {
    //        this.context = context;
    //        this.items = items;
    //    }
    //    public override long GetItemId(int position)
    //    {
    //        return position;
    //    }
    //    public override string this[int position]
    //    {
    //        get { return items[position]; }
    //    }
    //    public override int Count
    //    {
    //        get { return items.Length; }
    //    }
    //    public override View GetView(int position, View convertView, ViewGroup parent)
    //    {
    //        var item = items[position];
    //        View view = convertView;
    //        if (view == null) // no view to re-use, create new
    //            view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
    //        view.FindViewById<TextView>(Resource.Id.Text1).Text = items[position];
    //        view.FindViewById<TextView>(Resource.Id.Text2).Text = items[position];
    //        view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.Icon);
    //        return view;
    //    }
    //}
}
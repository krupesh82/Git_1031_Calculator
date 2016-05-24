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

namespace _1031_Calculator
{
    public class SavedPropertiesExpandableDataAdapter : BaseExpandableListAdapter
    {
        ImageView delBtn;
        readonly Activity Context;
        public SavedPropertiesExpandableDataAdapter(Activity newContext, List<Property> newList) : base()
        {
            Context = newContext;
            DataList = newList;
        }

        protected List<Property> DataList { get; set; }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            if (DataList == null || DataList.Count == 0)
            {
                var textView = Context.FindViewById<TextView>(Resource.Id.NoPropSaved);
                textView.Visibility = ViewStates.Visible;

                var elv = Context.FindViewById<ExpandableListView>(Resource.Id.elvSavedProperties);
                elv.Visibility = ViewStates.Invisible;
            }
            View header = convertView;
            if (header == null)
            {
                header = Context.LayoutInflater.Inflate(Resource.Layout.SavedPropertiesListGroup, null);
            }
            header.FindViewById<TextView>(Resource.Id.PropertyDataHeader).Text = DataList[groupPosition].Name;

            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = Context.LayoutInflater.Inflate(Resource.Layout.SavedPropertiesDataListItem, null);
            }

            row.FindViewById<TextView>(Resource.Id.liPropertyName).Text = DataList[groupPosition].Name;
            row.FindViewById<TextView>(Resource.Id.liState).Text = DataList[groupPosition].State;
            row.FindViewById<TextView>(Resource.Id.liMaritalStatus).Text = DataList[groupPosition].MaritalStatus;
            row.FindViewById<TextView>(Resource.Id.liIncomeGroup).Text = DataList[groupPosition].Income;
            row.FindViewById<TextView>(Resource.Id.liPurchasePrice).Text = "Purchase Price: $" + DataList[groupPosition].PurchasePrice.ToString();
            row.FindViewById<TextView>(Resource.Id.liCapitalImprovements).Text = "Capital Improvements: $" + DataList[groupPosition].CapitalImprovements.ToString();
            row.FindViewById<TextView>(Resource.Id.liSalePrice).Text = "Sale Price: $" + DataList[groupPosition].SalePrice.ToString();
            row.FindViewById<TextView>(Resource.Id.liTaxRate).Text = "Tax Rate: " + DataList[groupPosition].TaxRate.ToString() +"%";
            row.FindViewById<TextView>(Resource.Id.liSavings).Text = "1031 Savings: $" + DataList[groupPosition].Savings.ToString();

            delBtn = row.FindViewById<ImageView>(Resource.Id.deleteProperty);
            delBtn.SetTag(Resource.Id.deleteProperty , DataList[groupPosition].Name);
            delBtn.Click -= DelBtn_Click;
            delBtn.Click += DelBtn_Click;
            return row;
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
            alert.SetMessage("Do you want to remove this property?");
            alert.SetCancelable(false);
            alert.SetPositiveButton("Yes", delegate
            {
                string name = ((ImageView)sender).GetTag(Resource.Id.deleteProperty).ToString();

                try
                {
                    string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "//";
                    string dbFile = "Properties.db";
                    var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), dbPath + dbFile);
                    int count = conn.ExecuteScalar<int>("DELETE FROM Property where name='" + name + "'");
                    Toast.MakeText(this.Context, "Property " + name + " is deleted.", ToastLength.Short).Show();

                    DataList.Remove(DataList.Find(d => d.Name == name));
                    alert.Dispose();
                    NotifyDataSetChanged();

                    if (DataList == null || DataList.Count == 0)
                    {
                        var textView = Context.FindViewById<TextView>(Resource.Id.NoPropSaved);
                        textView.Visibility = ViewStates.Visible;

                        var elv = Context.FindViewById<ExpandableListView>(Resource.Id.elvSavedProperties);
                        elv.Visibility = ViewStates.Invisible;
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this.Context, "Could not delete the property. Please try again.", ToastLength.Short).Show();
                }
            });

            alert.SetNegativeButton("No", delegate
            {
                alert.Dispose();
            });

            alert.Create().Show();
        }

        public override int GetChildrenCount(int groupPosition)
        {
            if (DataList.Count >= groupPosition)
                return 1;
            else
                return 0;
        }

        public override int GroupCount
        {
            get
            {
                return DataList.Count;
            }
        }

       

        #region implemented abstract members of BaseExpandableListAdapter

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return false;
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        #endregion
    }
}
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
    public class AboutExpandableDataAdapter : BaseExpandableListAdapter
    {

        readonly Activity Context;
        public AboutExpandableDataAdapter(Activity newContext, List<AboutData> newList) : base()
        {
            Context = newContext;
            DataList = newList;
        }

        protected List<AboutData> DataList { get; set; }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = Context.LayoutInflater.Inflate(Resource.Layout.AboutListGroup, null);
            }
            header.FindViewById<TextView>(Resource.Id.AboutDataHeader).Text = DataList[groupPosition].Question;

            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = Context.LayoutInflater.Inflate(Resource.Layout.AboutDataListItem, null);
            }
            string newId = "", newValue = "";
            GetChildViewHelper(groupPosition, childPosition, out newValue);
            row.FindViewById<TextView>(Resource.Id.AboutDataValue).Text = newValue;

            return row;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            if (DataList.Count >= groupPosition && !string.IsNullOrEmpty(DataList[groupPosition].Answer))
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

        private void GetChildViewHelper(int groupPosition, int childPosition, out string Value)
        {
            if (DataList.Count < groupPosition || string.IsNullOrEmpty(DataList[groupPosition].Answer)) { Value = ""; }
            else
            {
                Value = DataList[groupPosition].Answer;
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
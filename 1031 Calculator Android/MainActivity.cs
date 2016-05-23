using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text;
using System.Linq;
using SQLite;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System.IO;
using System.Collections.Generic;
using static Android.Widget.ExpandableListView;
using Android.Text.Style;

namespace _1031_Calculator
{
    [Activity(Label = "1031 Exchange Calculator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            AddTab("1031 Calculator", new Calculator1031());
            AddTab("Saved Properties", new SavedProperties());
            AddTab("About 1031", new About1031());
            AddTab("Disclaimer", new Disclaimer());            
        }
        
        void AddTab(string tabText, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);

            // must set event handler before adding tab
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e) {
                e.FragmentTransaction.Remove(view);
            };

            this.ActionBar.AddTab(tab);
        }
    }

    class Calculator1031 : Fragment
    {
        private View view;
        double percentageTax = 0.00;
        double percentageComplete = 0.00;
        string maritalStatus;
        double savings = 0.00;
        string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        double prevPP = 0;
        double prevCI = 0;
        double prevSP = 0;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.Calculator1031, container, false);
            this.Activity.SetTitle(Resource.String.Calculator1031);

            SetPercentageComplete();
            SetPercentageTax();

            var pc = view.FindViewById<TextView>(Resource.Id.percentagecomplete);
            Android.Graphics.Rect outRect = new Android.Graphics.Rect();
            pc.GetDrawingRect(outRect);

            var seekPurchasePrice = view.FindViewById<SeekBar>(Resource.Id.seekPurchasePrice);
            seekPurchasePrice.ProgressChanged += SeekPurchasePrice_ProgressChanged;

            var seekCapImprovement = view.FindViewById<SeekBar>(Resource.Id.seekCapitalImprovements);
            seekCapImprovement.ProgressChanged += SeekCapImprovement_ProgressChanged;

            var seekSalePrice = view.FindViewById<SeekBar>(Resource.Id.seekSalePrice);
            seekSalePrice.ProgressChanged += SeekSalePrice_ProgressChanged;

            var ipPurchasePrice = view.FindViewById<EditText>(Resource.Id.ipPurchasePrice);
            ipPurchasePrice.TextChanged += IpPurchasePrice_TextChanged;

            var ipCapImprovement = view.FindViewById<EditText>(Resource.Id.ipCapitalImprovements);
            ipCapImprovement.TextChanged += IpCapImprovement_TextChanged;

            var ipSalePrice = view.FindViewById<EditText>(Resource.Id.ipSalePrice);
            ipSalePrice.TextChanged += IpSalePrice_TextChanged;

            SpinnerStates_Fill();
            SpinnerMaritalStatus_Fill();

            var btnCalculate = view.FindViewById<Button>(Resource.Id.calculate);
            btnCalculate.Click += BtnCalculate_Click;

            var btnSave = view.FindViewById<Button>(Resource.Id.saveresults);
            btnSave.Click += BtnSave_Click;

            return view;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            CreateDB();
            Property property = GetPropertyObject();
            if (property == null)
            {
                Toast.MakeText(view.Context, "Please fill all the fields.", ToastLength.Short).Show();
                return;
            }

            AlertDialog alertDialog = (new AlertDialog.Builder(view.Context)).Create();
            var viewAD = LayoutInflater.From(view.Context).Inflate(Resource.Layout.SaveDialog, null);
            alertDialog.SetView(viewAD);
            alertDialog.SetTitle("Enter Name");
            alertDialog.SetCanceledOnTouchOutside(false);
            var btnCancel = viewAD.FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Click += delegate {
                alertDialog.Dismiss();
                Toast.MakeText(viewAD.Context, "Saving Canceled!", ToastLength.Short).Show();
            };
            var name = "";
            var btnSave = viewAD.FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Click += delegate
            {
                var nameField = viewAD.FindViewById<EditText>(Resource.Id.saveName);
                name = nameField.Text;

                if (IsDuplicateName(name))
                {
                    Toast.MakeText(viewAD.Context, "Property with name '" + name + "' already exist. Please use different name.", ToastLength.Short).Show();
                }
                else
                {
                    property.Name = name;

                    CalculateTaxRate();
                    property.TaxRate = percentageTax;

                    CalculateSavings();
                    property.Savings = savings;

                    if (SaveToDB(property))
                    {
                        alertDialog.Dismiss();
                        Toast.MakeText(viewAD.Context, name + " saved successfully.", ToastLength.Short).Show();
                    }
                    else
                    {
                        alertDialog.Dismiss();
                        Toast.MakeText(viewAD.Context, "Sorry! Could not save the property. Please try again.", ToastLength.Short).Show();
                    }
                }
            }; 

            alertDialog.Show();
        }

        private Property GetPropertyObject()
        {
            Property property = new Property();
            if (percentageComplete < 100.00) return null;

            var pp = view.FindViewById<EditText>(Resource.Id.ipPurchasePrice);
            property.PurchasePrice = Convert.ToDouble(pp.Text);

            var cp = view.FindViewById<EditText>(Resource.Id.ipCapitalImprovements);
            property.CapitalImprovements = Convert.ToDouble(cp.Text);

            var sp = view.FindViewById<EditText>(Resource.Id.ipSalePrice);
            property.SalePrice = Convert.ToDouble(sp.Text);

            var state = view.FindViewById<Spinner>(Resource.Id.spinnerStates);
            property.State = state.SelectedItem.ToString();

            var ms = view.FindViewById<Spinner>(Resource.Id.spinnerMaritalStatus);
            property.MaritalStatus = ms.SelectedItem.ToString();

            var income = view.FindViewById<Spinner>(Resource.Id.spinnerIncome);
            property.Income = income.SelectedItem.ToString();

            return property;
        }

        private void CreateDB()
        {
            string dbFile = "Properties.db";
            if (!File.Exists(dbPath + dbFile))
            {
                Stream instream = view.Context.Assets.Open(dbFile);
                if (!Directory.Exists(dbPath))
                    Directory.CreateDirectory(dbPath);

                Stream outstream = new FileStream(dbPath + dbFile, FileMode.OpenOrCreate);
                //transfer bytes from the inputfile to the outputfile
                byte[] buffer = new byte[1024];
                int len;
                while ((len = instream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outstream.Write(buffer, 0, len);
                }

                //Close the streams
                outstream.Flush();
                outstream.Close();
                instream.Close();
            }
        }

        private bool IsDuplicateName(string name)
        {
            string dbFile = "Properties.db";
            try
            {
                var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), dbPath + dbFile);
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM Property where name = '" + name + "'");
                if (count > 0) return true;
                else return false;
            }
            catch (Exception ex) { return true; }
        }
        private bool SaveToDB(Property property)
        {
            string dbFile = "Properties.db";
            try
            {
                    var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), dbPath + dbFile);
                    conn.Insert(property);
                    return true;                
            }
            catch (Exception ex) {
                return false;
            }
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            CalculateSavings();
            var percComplete = view.FindViewById<TextView>(Resource.Id.percentagecomplete);
            string percText = Html.FromHtml("Your 1031 savings: <br>$" + savings.ToString()).ToString();
            percComplete.SetText(percText.ToCharArray(), 0, percText.Length);
        }

        private void CalculateSavings()
        {
            if (percentageComplete == 100.00)
            {
                double pp = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.ipPurchasePrice).Text);
                double ci = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.ipCapitalImprovements).Text);
                double sp = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.ipSalePrice).Text);

                if (sp < pp + ci)
                {
                    savings = 0.00;
                }
                else
                {
                    double gain = sp - (pp + ci);
                    savings = gain * percentageTax / 100;
                }
            }
            else
            {
                savings = 0.0;
            }
        }

        private void SpinnerStates_Fill()
        {
            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerStates);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerStates_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    view.Context, Resource.Array.state_name, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void spinnerStates_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            string taxRate = Resources.GetStringArray(Resource.Array.state_tax)[e.Position];
            SetPercentageComplete();
            SetPercentageTax();
        }

        private void SpinnerMaritalStatus_Fill()
        {
            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerMaritalStatus);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerMaritalStatus_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    view.Context, Resource.Array.marital_status, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void spinnerMaritalStatus_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            maritalStatus = spinner.GetItemAtPosition(e.Position).ToString();
            SetPercentageComplete();
            SpinnerIncome_Fill();
        }

        private void SpinnerIncome_Fill()
        {
            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerIncome);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerIncome_ItemSelected);
            ArrayAdapter adapter = null;
            
            if(maritalStatus.ToUpper() == "SINGLE")
                adapter = ArrayAdapter.CreateFromResource(
                    view.Context, Resource.Array.single_income, Android.Resource.Layout.SimpleSpinnerItem);
            else if(maritalStatus.ToUpper() == "MARRIED")
                adapter = ArrayAdapter.CreateFromResource(
                    view.Context, Resource.Array.married_income, Android.Resource.Layout.SimpleSpinnerItem);

            if (adapter != null)
            {
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinner.Adapter = adapter;
            }
        }

        private void spinnerIncome_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            string taxRate = Resources.GetStringArray(Resource.Array.state_tax)[e.Position];
            SetPercentageComplete();
            SetPercentageTax();
        }

        private void IpSalePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            var seekSalePrice = view.FindViewById<SeekBar>(Resource.Id.seekSalePrice);
            int progress = 0;
            if(!string.IsNullOrEmpty(e.Text.ToString()))
            {
                Double sp = 0;
                Double.TryParse(e.Text.ToString(), out sp);
                if (sp > seekSalePrice.Max)
                {
                    sp = prevSP;
                    ((EditText)sender).Text = sp.ToString();
                }
                progress = (int)sp;
                prevSP = sp;
            }

            seekSalePrice.Progress = progress;
            SetPercentageComplete();
        }

        private void IpCapImprovement_TextChanged(object sender, TextChangedEventArgs e)
        {
            var seekCapitalImprovements = view.FindViewById<SeekBar>(Resource.Id.seekCapitalImprovements);
            int progress = 0;
            if (!string.IsNullOrEmpty(e.Text.ToString()))
            {
                Double ci = 0;
                Double.TryParse(e.Text.ToString(), out ci);
                if (ci > seekCapitalImprovements.Max)
                {
                    ci = prevCI;
                    ((EditText)sender).Text = ci.ToString();
                }
                progress = (int)ci;
                prevCI = ci;
            }

            seekCapitalImprovements.Progress = progress;
            SetPercentageComplete();
        }

        private void IpPurchasePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            var seekPurchasePrice = view.FindViewById<SeekBar>(Resource.Id.seekPurchasePrice);
            int progress = 0;
            if (!string.IsNullOrEmpty(e.Text.ToString()))
            {
                Double pp = 0;
                Double.TryParse(e.Text.ToString(), out pp);
                if (pp > seekPurchasePrice.Max)
                {
                    pp = prevPP;
                    ((EditText)sender).Text = pp.ToString();
                }
                progress = (int)pp;
                prevPP = pp;
            }
            seekPurchasePrice.Progress = progress;
            SetPercentageComplete();
        }

        private void SeekSalePrice_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.FromUser)
            {
                var textView = view.FindViewById<EditText>(Resource.Id.ipSalePrice);
                textView.Text = e.Progress.ToString();
            }
        }

        private void SeekCapImprovement_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.FromUser)
            {
                var textView = view.FindViewById<EditText>(Resource.Id.ipCapitalImprovements);
                textView.Text = e.Progress.ToString();
            }
        }

        private void SeekPurchasePrice_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.FromUser)
            {
                var textView = view.FindViewById<EditText>(Resource.Id.ipPurchasePrice);
                textView.Text = e.Progress.ToString();
            }
        }

        private void SetPercentageComplete()
        {
            int completionStep = 0;

            var textView = view.FindViewById<EditText>(Resource.Id.ipPurchasePrice);
            if (!string.IsNullOrEmpty(textView.Text)) completionStep++;

            textView = view.FindViewById<EditText>(Resource.Id.ipCapitalImprovements);
            if (!string.IsNullOrEmpty(textView.Text)) completionStep++;

            textView = view.FindViewById<EditText>(Resource.Id.ipSalePrice);
            if (!string.IsNullOrEmpty(textView.Text)) completionStep++;

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerStates);
            if (spinner.SelectedItem != null) completionStep++;

            spinner = view.FindViewById<Spinner>(Resource.Id.spinnerMaritalStatus);
            if (spinner.SelectedItem != null) completionStep++;

            spinner = view.FindViewById<Spinner>(Resource.Id.spinnerIncome);
            if (spinner.SelectedItem != null) completionStep++;

            percentageComplete = completionStep * 100 / 6;
            var percComplete = view.FindViewById<TextView>(Resource.Id.percentagecomplete);
            string percText = percentageComplete.ToString() + "%<br>Complete";
            SpannableString ss1 = new SpannableString(Html.FromHtml(percText).ToString());
            ss1.SetSpan(new RelativeSizeSpan(2f), 0, percText.IndexOf('%') + 1,SpanTypes.ExclusiveExclusive); // set size
            
            percComplete.SetText(ss1, TextView.BufferType.Spannable);

            var button = view.FindViewById<Button>(Resource.Id.calculate);
            if (percentageComplete == 100.00)
                button.Enabled = true;
            else
                button.Enabled = false;
        }

        private void CalculateTaxRate()
        {
            string stateTax = "0.0";
            string federalTax = "0.0";
            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerStates);
            if (spinner.SelectedItem != null)
            {
                stateTax = Resources.GetStringArray(Resource.Array.state_tax)[spinner.SelectedItemPosition];
            }

            spinner = view.FindViewById<Spinner>(Resource.Id.spinnerMaritalStatus);
            if (spinner.SelectedItem != null)
            {
                spinner = view.FindViewById<Spinner>(Resource.Id.spinnerIncome);

                if (spinner.SelectedItem != null)
                {
                    if (maritalStatus.ToUpper() == "SINGLE")
                    {
                        federalTax = Resources.GetStringArray(Resource.Array.single_income_tax_rate)[spinner.SelectedItemPosition];
                    }
                    else if (maritalStatus.ToUpper() == "MARRIED")
                    {
                        federalTax = Resources.GetStringArray(Resource.Array.married_income_tax_rate)[spinner.SelectedItemPosition];
                    }
                }
            }

            percentageTax = Convert.ToDouble(stateTax) + Convert.ToDouble(federalTax);
        }

        private void SetPercentageTax()
        {
            CalculateTaxRate();
            
            var percTax = view.FindViewById<TextView>(Resource.Id.percentagetax);
            string percText = percentageTax.ToString() + "%<br>Tax Rate";
            SpannableString ss1 = new SpannableString(Html.FromHtml(percText).ToString());
            ss1.SetSpan(new RelativeSizeSpan(2f), 0, percText.IndexOf('%') + 1, SpanTypes.ExclusiveExclusive); // set size

            percTax.SetText(ss1, TextView.BufferType.Spannable);
        }
    }
    class SavedProperties : Fragment
    {
        string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        int lastExpandedGroup = -1;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SavedProperties, container, false);
            this.Activity.SetTitle(Resource.String.SavedProperties);

            int count = GetCount();

            List<Property> properties = GetSavedProperties();

            if (count == 0 || properties == null || properties.Count == 0)
            {
                var textView = view.FindViewById<TextView>(Resource.Id.NoPropSaved);
                textView.Visibility = ViewStates.Visible;

                var elv = view.FindViewById<ExpandableListView>(Resource.Id.elvSavedProperties);
                elv.Visibility = ViewStates.Invisible;
            }
            else
            {
                var textView = view.FindViewById<TextView>(Resource.Id.NoPropSaved);
                textView.Visibility = ViewStates.Gone;

                var elv = view.FindViewById<ExpandableListView>(Resource.Id.elvSavedProperties);
                elv.Visibility = ViewStates.Visible;
                var expandListener = new SavedPropertiesExpandableDataAdapter(this.Activity, properties);
                elv.SetAdapter(expandListener);
                elv.GroupExpand += Elv_GroupExpand;
            }            

            return view;
        }

        private void Elv_GroupExpand(object sender, GroupExpandEventArgs e)
        {
            var elv = (ExpandableListView)sender;
            if (lastExpandedGroup != -1
                    && e.GroupPosition != lastExpandedGroup)
            {
                elv.CollapseGroup(lastExpandedGroup);
            }
            lastExpandedGroup = e.GroupPosition;
        }
        
        private int GetCount()
        {
            string dbFile = "Properties.db";
            try
            {
                var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), dbPath + dbFile);
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM Property");
                return count;
            }
            catch (Exception ex) { return -1; }
        }

        private List<Property> GetSavedProperties()
        {
            string dbFile = "Properties.db";
            try
            {
                var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), dbPath + dbFile);
                List<Property> properties = conn.Table<Property>().ToList<Property>();
                return properties;

            }
            catch (Exception ex) { return null; }
        }
    }
    class About1031 : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.About1031, container, false);
            this.Activity.SetTitle(Resource.String.About1031);
            var listView = view.FindViewById<ExpandableListView>(Resource.Id.expandableListview);
            listView.SetAdapter(new AboutExpandableDataAdapter(this.Activity, AboutData.GetFaqs()));
            return view;
        }
    }
    class Disclaimer : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.Activity.SetTitle(Resource.String.Disclaimer);
            var view = inflater.Inflate(Resource.Layout.Disclaimer, container, false);
            return view;
        }
    }

    public class Property
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public double PurchasePrice { get; set; }

        public double CapitalImprovements { get; set; }

        public double SalePrice { get; set; }

        public string State { get; set; }

        public string MaritalStatus { get; set; }

        public string Income { get; set; }

        public double TaxRate { get; set; }

        public double Savings { get; set; }        
    }
}


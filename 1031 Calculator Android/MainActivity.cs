using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text;
using System.Linq;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.Calculator1031, container, false);
            this.Activity.SetTitle(Resource.String.Calculator1031);

            SetPercentageComplete();
            SetPercentageTax();

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

            return view;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if(percentageComplete == 100.00)
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
                    savings = gain * percentageTax/100;
                }

                var percComplete = view.FindViewById<TextView>(Resource.Id.percentagecomplete);
                string percText = Html.FromHtml("Your 1031 savings: <br>$" + savings.ToString()).ToString();
                percComplete.SetText(percText.ToCharArray(), 0, percText.Length);
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
                progress = Convert.ToInt32(e.Text.ToString());

            seekSalePrice.Progress = progress;
            SetPercentageComplete();
        }

        private void IpCapImprovement_TextChanged(object sender, TextChangedEventArgs e)
        {
            var seekCapitalImprovements = view.FindViewById<SeekBar>(Resource.Id.seekCapitalImprovements);
            int progress = 0;
            if (!string.IsNullOrEmpty(e.Text.ToString()))
                progress = Convert.ToInt32(e.Text.ToString());

            seekCapitalImprovements.Progress = progress;
            SetPercentageComplete();
        }

        private void IpPurchasePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            var seekPurchasePrice = view.FindViewById<SeekBar>(Resource.Id.seekPurchasePrice);
            int progress = 0;
            if (!string.IsNullOrEmpty(e.Text.ToString()))
                progress = Convert.ToInt32(e.Text.ToString());
            
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
            string percText = Html.FromHtml(percentageComplete.ToString() + "%<br>Complete").ToString();
            percComplete.SetText(percText.ToCharArray(), 0, percText.Length);

            var button = view.FindViewById<Button>(Resource.Id.calculate);
            if (percentageComplete == 100.00)
                button.Enabled = true;
            else
                button.Enabled = false;
        }

        private void SetPercentageTax()
        {
            string stateTax ="0.0";
            string federalTax="0.0";
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
                    else if(maritalStatus.ToUpper() == "MARRIED")
                    {
                        federalTax = Resources.GetStringArray(Resource.Array.married_income_tax_rate)[spinner.SelectedItemPosition];
                    }
                }                
            }

            percentageTax = Convert.ToDouble(stateTax) + Convert.ToDouble(federalTax);
            
            var percTax = view.FindViewById<TextView>(Resource.Id.percentagetax);
            string percText = Html.FromHtml(percentageTax.ToString() + "%<br>Tax Rate").ToString();
            percTax.SetText(percText.ToCharArray(), 0, percText.Length);
        }
    }
    class SavedProperties : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SavedProperties, container, false);
            this.Activity.SetTitle(Resource.String.SavedProperties);
            return view;
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
            listView.SetAdapter(new ExpandableDataAdapter(this.Activity, AboutData.GetFaqs()));
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
}


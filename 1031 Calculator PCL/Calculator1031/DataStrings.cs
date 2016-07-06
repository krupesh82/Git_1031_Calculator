using System;
using System.Collections.Generic;

namespace Calculator1031
{
	public class DataString
	{
		public string Text { get; set; }
		public string Value{ get; set; }
	}

	public class DataStrings
	{
		private static List<DataString> _dataStates;
		private static List<DataString> _dataMaritalStatus;
		private static List<DataString> _dataSingleIncomeGroup;
		private static List<DataString> _dataMarriedIncomeGroup;

		public static List<DataString> GetStates()
		{
			if (_dataStates == null || _dataStates.Count == 0) {
				_dataStates = new List<DataString> ();

				_dataStates.Add (new DataString () {
					Text = "Alabama",
					Value = "5"
				});

				_dataStates.Add (new DataString () {
					Text = "Alaska",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "Arizona",
					Value = "4.5"
				});

				_dataStates.Add (new DataString () {
					Text = "Arkansas",
					Value = "7"
				});

				_dataStates.Add (new DataString () {
					Text = "California",
					Value = "13.3"
				});

				_dataStates.Add (new DataString () {
					Text = "Colorado",
					Value = "4.6"
				});

				_dataStates.Add (new DataString () {
					Text = "Connecticut",
					Value = "6.7"
				});

				_dataStates.Add (new DataString () {
					Text = "Delaware",
					Value = "6.6"
				});

				_dataStates.Add (new DataString () {
					Text = "Florida",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "Georgia",
					Value = "6"
				});

				_dataStates.Add (new DataString () {
					Text = "Hawaii",
					Value = "7.3"
				});

				_dataStates.Add (new DataString () {
					Text = "Idaho",
					Value = "7.4"
				});

				_dataStates.Add (new DataString () {
					Text = "Illinois",
					Value = "3.8"
				});

				_dataStates.Add (new DataString () {
					Text = "Indiana",
					Value = "3.3"
				});

				_dataStates.Add (new DataString () {
					Text = "Iowa",
					Value = "9"
				});

				_dataStates.Add (new DataString () {
					Text = "Kansas",
					Value = "4.8"
				});

				_dataStates.Add (new DataString () {
					Text = "Kentucky",
					Value = "6"
				});

				_dataStates.Add (new DataString () {
					Text = "Louisiana",
					Value = "6"
				});

				_dataStates.Add (new DataString () {
					Text = "Maine",
					Value = "8"
				});

				_dataStates.Add (new DataString () {
					Text = "Maryland",
					Value = "5.8"
				});

				_dataStates.Add (new DataString () {
					Text = "Massachusetts",
					Value = "5.2"
				});

				_dataStates.Add (new DataString () {
					Text = "Michigan",
					Value = "4.3"
				});

				_dataStates.Add (new DataString () {
					Text = "Minnesota",
					Value = "9.9"
				});

				_dataStates.Add (new DataString () {
					Text = "Mississippi",
					Value = "5"
				});

				_dataStates.Add (new DataString () {
					Text = "Missouri",
					Value = "6"
				});

				_dataStates.Add (new DataString () {
					Text = "Montana",
					Value = "6.9"
				});

				_dataStates.Add (new DataString () {
					Text = "Nebraska",
					Value = "6.8"
				});

				_dataStates.Add (new DataString () {
					Text = "Nevada",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "New Hampshire",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "New Jersey",
					Value = "9"
				});

				_dataStates.Add (new DataString () {
					Text = "New Mexico",
					Value = "4.9"
				});

				_dataStates.Add (new DataString () {
					Text = "New York",
					Value = "8.8"
				});

				_dataStates.Add (new DataString () {
					Text = "North Carolina",
					Value = "5.8"
				});

				_dataStates.Add (new DataString () {
					Text = "North Dakota",
					Value = "3.2"
				});

				_dataStates.Add (new DataString () {
					Text = "Ohio",
					Value = "5.3"
				});

				_dataStates.Add (new DataString () {
					Text = "Oklahoma",
					Value = "5.3"
				});

				_dataStates.Add (new DataString () {
					Text = "Oregon",
					Value = "9.9"
				});

				_dataStates.Add (new DataString () {
					Text = "Pennsylvania",
					Value = "3.1"
				});

				_dataStates.Add (new DataString () {
					Text = "Rhode Island",
					Value = "7"
				});

				_dataStates.Add (new DataString () {
					Text = "South Carolina",
					Value = "7"
				});

				_dataStates.Add (new DataString () {
					Text = "South Dakota",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "Tennessee",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "Texas",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "Utah",
					Value = "5"
				});

				_dataStates.Add (new DataString () {
					Text = "Vermont",
					Value = "9"
				});

				_dataStates.Add (new DataString () {
					Text = "Virginia",
					Value = "5.8"
				});

				_dataStates.Add (new DataString () {
					Text = "Washington",
					Value = "0"
				});

				_dataStates.Add (new DataString () {
					Text = "West Virginia",
					Value = "6.5"
				});

				_dataStates.Add (new DataString () {
					Text = "Wisconsin",
					Value = "7.7"
				});

				_dataStates.Add (new DataString () {
					Text = "Wyoming",
					Value = "0"
				});
			}
			return _dataStates;
		}

		public static List<DataString> GetMaritalStatus()
		{
			if (_dataMaritalStatus == null || _dataMaritalStatus.Count == 0) {
				_dataMaritalStatus = new List<DataString> ();
				_dataMaritalStatus.Add (new DataString () {
					Text = "Single",
					Value = "0"
				});
				_dataMaritalStatus.Add (new DataString () {
					Text = "Married",
					Value = "1"
				});
			}
			return _dataMaritalStatus;
		}

		public static List<DataString> GetDataIncomeGroup(string maritalStatus)
		{
			if (maritalStatus.ToUpper () == "SINGLE") {
				if (_dataSingleIncomeGroup == null || _dataSingleIncomeGroup.Count == 0) {
					_dataSingleIncomeGroup = new List<DataString> ();
					_dataSingleIncomeGroup.Add (new DataString () {
						Text = "0-36,250",
						Value = "0"
					});
					_dataSingleIncomeGroup.Add (new DataString () {
						Text = "36,250-200,000",
						Value = "18.8"
					});
					_dataSingleIncomeGroup.Add (new DataString () {
						Text = "200,000-400,000",
						Value = "18.8"
					});
					_dataSingleIncomeGroup.Add (new DataString () {
						Text = "400,000 and higher",
						Value = "23.8"
					});
				}
				return _dataSingleIncomeGroup;
			} else if (maritalStatus.ToUpper () == "MARRIED") {
				if (_dataMarriedIncomeGroup == null || _dataMarriedIncomeGroup.Count == 0) {
					_dataMarriedIncomeGroup = new List<DataString> ();
					_dataMarriedIncomeGroup.Add (new DataString () {
						Text = "0-72,500",
						Value = "0"
					});
					_dataMarriedIncomeGroup.Add (new DataString () {
						Text = "72,500-250,000",
						Value = "18.8"
					});
					_dataMarriedIncomeGroup.Add (new DataString () {
						Text = "250,000-450,000",
						Value = "18.8"
					});
					_dataMarriedIncomeGroup.Add (new DataString () {
						Text = "450,000 and higher",
						Value = "23.8"
					});
				}
				return _dataMarriedIncomeGroup;
			}

			return new List<DataString> ();
		}
	}
}


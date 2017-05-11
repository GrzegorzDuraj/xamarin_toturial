using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using System.Collections.Generic;

namespace toturial_call_after_translate
{
    [Activity(Label = "PHoneWord_Label", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly List<string> phoneNumbers = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
			
            Button callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            Button translateButton = FindViewById<Button>(Resource.Id.TranlateButton);
            Button callButton = FindViewById<Button>(Resource.Id.CallButton);

            if (callHistoryButton == null)
            {
                throw new Exception();
            }
            callHistoryButton.Enabled = true;
            Console.WriteLine("test");
            

            //disable Call button
            callButton.Enabled = false;

            string translateNumber = string.Empty;

            translateButton.Click += (object sender, EventArgs e) =>
            {
                // Translate User's alphanumeric phone number to numeric 
                translateNumber = Core.PhoneWordTranslator.ToNumber(phoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(translateNumber))
                {

                    Console.WriteLine("Translate Button");
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                    callHistoryButton.Enabled = true;
                }
                else
                {

                    Console.WriteLine("Translate Button");
                    callButton.Text = "Call " + translateNumber;
                    callButton.Enabled = true;
                    callHistoryButton.Enabled = true;
                }
            };


            callButton.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + translateNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
               {
                   //add dialed number to list of called numbers
                   phoneNumbers.Add(translateNumber);
                   callHistoryButton.Enabled = true;


                    Console.WriteLine("Translate Button");

                   //Create intent to dial phone
                   var callIntent = new Intent(Intent.ActionDial);

                   callIntent.SetData(Android.Net.Uri.Parse("tel:" + translateNumber));
                   StartActivity(callIntent);
               });
                callDialog.SetNegativeButton("Cancel", delegate { });

                //show the alert dialgo to the user and wait for response. 
                callDialog.Show();

            };

            callHistoryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CallHistoryActivity));
                intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };
        }
    }
}


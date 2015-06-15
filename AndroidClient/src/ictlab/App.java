package ictlab;

import ictlab.dataSources.TestConnectivityManager;
import ictlab.receivers.MonitorContextService;
import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;

/**
 * 
 */

/**
 * @author Zero
 *
 */
public class App extends Activity {
	TestConnectivityManager testConnectivityManager;

	/**
	 * setup field variables
	 */
	public App() {
		/*
		 * TODO Controller de flow van gebruikers interactie Creates a new
		 * Intent to start the RSSPullService IntentService. Passes a URI in the
		 * Intent's "data" field.
		 */

		testConnectivityManager = new TestConnectivityManager();

	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		/*
		 * Creates a new Intent to start the RSSPullService IntentService.
		 * Passes a URI in the Intent's "data" field.
		 */
		Intent mServiceIntent;
		mServiceIntent = new Intent(getApplicationContext(), MonitorContextService.class);
		// mServiceIntent.setData(Uri.parse("www.google.com"));
		// startService(mServiceIntent);

		super.onCreate(savedInstanceState);
	}
}
package ictlab;

import ictlab.dataSources.TestConnectivityManager;
import android.app.Activity;
import android.content.Intent;
import android.net.Uri;

/**
 * 
 */

/**
 * @author Zero
 *
 */
public class App extends Activity {

	/**
	 * 
	 */
	public App() {
		/*TODO Controller de flow van gebruikers interactie
		 * Creates a new Intent to start the RSSPullService
		 * IntentService. Passes a URI in the
		 * Intent's "data" field.
		 */
		// mServiceIntent = new Intent(getActivity(), RSSPullService.class);
		// mServiceIntent.setData(Uri.parse(dataUrl));	}
	
		TestConnectivityManager testConnectivityManager = new TestConnectivityManager() ; 
		
		/*
		 * Creates a new Intent to start the RSSPullService
		 * IntentService. Passes a URI in the
		 * Intent's "data" field.
		 */
		Intent mServiceIntent ;  
		//mServiceIntent = new Intent(this.getApplicationContext(), TestService.class);
		//mServiceIntent.setData(Uri.parse("test"));
		//startService(mServiceIntent); 
}
}
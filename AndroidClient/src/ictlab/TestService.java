package ictlab;

import android.app.IntentService;
import android.content.Intent;

/*
 @author Zero 

 *  TODO maak de manifest file goed 
 *  
 *  
 *    <application
 android:icon="@drawable/icon"
 android:label="@string/app_name">
 ...
 <!--
 Because android:exported is set to "false",
 the service is only available to this app.
 -->
 <service
 android:name=".TestService"
 android:exported="false"/>
 ...
 <application/>


 */
public class TestService extends IntentService {

	public TestService(String name) {
		super(name);
		// TODO Auto-generated constructor stub
	}

	@Override
	protected void onHandleIntent(Intent intent) {
		
				
		
		
		// TODO Auto-generated method stub
		
		  Intent workIntent;
		// Gets data from the incoming Intent
  //      String dataString = workIntent.getDataString();
        // Do work here, based on the contents of dataString

     // Starts the IntentService
        //getActivity().startService(mServiceIntent);
        
        /*
         * Creates a new Intent containing a Uri object
         * BROADCAST_ACTION is a custom Intent action
         */

        /* Intent localIntent =
                new Intent(Constants.BROADCAST_ACTION)
                // Puts the status into the Intent
                .putExtra(Constants.EXTENDED_DATA_STATUS, status);
        // Broadcasts the Intent to receivers in this app.
        LocalBroadcastManager.getInstance(this).sendBroadcast(localIntent);
*/ 
	}

}

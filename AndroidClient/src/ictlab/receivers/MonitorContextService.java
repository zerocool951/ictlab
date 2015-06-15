package ictlab.receivers;

import android.app.IntentService;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

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
public class MonitorContextService extends IntentService {

	private static String logname = "TestService" ; 
	
	public MonitorContextService() {
		super(logname);
	}

	public MonitorContextService(String name) {
		super(logname);
	}

	@Override
	protected void onHandleIntent(Intent intent) {

		// TODO Auto-generated method stub
		Log.v("tony", logname+" is aangeroepen");
		
		   
		
		Thread monitorLoop = new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
			while(true){
				
		
				//call the context checker 
				
				try {
					Thread.sleep(1000);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					Log.e("Tony" , "Kon niet de thread niet slapen " );
					e.printStackTrace();
				}
			}}
		}) ; 
		
		monitorLoop.run(); 
		
		
		Intent workIntent;
		// Gets data from the incoming Intent
		// String dataString = workIntent.getDataString();
		// Do work here, based on the contents of dataString

		// Starts the IntentService
		// getActivity().startService(mServiceIntent);

		/*
		 * Creates a new Intent containing a Uri object BROADCAST_ACTION is a
		 * custom Intent action
		 */

		/*
		 * Intent localIntent = new Intent(Constants.BROADCAST_ACTION) // Puts
		 * the status into the Intent .putExtra(Constants.EXTENDED_DATA_STATUS,
		 * status); // Broadcasts the Intent to receivers in this app.
		 * LocalBroadcastManager.getInstance(this).sendBroadcast(localIntent);
		 */
	}

}

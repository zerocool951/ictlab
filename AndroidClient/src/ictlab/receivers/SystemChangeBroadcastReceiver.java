package ictlab.receivers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 * 
 */

/**
 * @author Zero
 *
 */
public class SystemChangeBroadcastReceiver extends BroadcastReceiver {

	// Prevents instantiation
	// private DownloadStateReceiver() {
	// }

	/**
	 * 
	 */
	public SystemChangeBroadcastReceiver() {
		// TODO Auto-generated constructor stub
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see android.content.BroadcastReceiver#onReceive(android.content.Context,
	 * android.content.Intent)
	 */
	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub
		Intent i = new Intent(context, MonitorContextService.class);
		i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		// context.startActivity(i);
		context.startService(i);

	}

}

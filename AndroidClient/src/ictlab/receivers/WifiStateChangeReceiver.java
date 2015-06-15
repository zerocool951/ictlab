package ictlab.receivers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class WifiStateChangeReceiver extends BroadcastReceiver {

	public WifiStateChangeReceiver() {
		// TODO Auto-generated constructor stub
	}

	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub

		Intent mServiceIntent;
		mServiceIntent = new Intent(context, MonitorContextService.class);
		// mServiceIntent.setData(Uri.parse("www.google.com"));
		 context.startService(mServiceIntent);

		
	}

}

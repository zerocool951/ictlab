package ictlab.dataSources;

import java.util.List;

import android.net.ConnectivityManager;
import android.net.DhcpInfo;
import android.net.IpPrefix;
import android.net.LinkAddress;
import android.net.LinkProperties;
import android.net.Network;
import android.net.NetworkCapabilities;
import android.net.NetworkInfo;
import android.net.NetworkRequest;
import android.net.Proxy;
import android.net.PskKeyManager;
import android.net.RouteInfo;
import android.net.SSLSessionCache;
import android.net.TrafficStats;
import android.net.VpnService;
import android.app.Activity;
import android.content.Context;

/**
 * 
 */

/**
 * @author Zero
 *
 *Hier zit de controle op de systeem datapoints om te valideren 
 *of er relevate wijzgingen zijn gemaakt.  
 *
 *
 */
public class TestConnectivityManager {

	public void TestConnectivityManager() {

	}

	/*
	 * check if there is need for a vpn connection  from the systemcontext
	 *  if so it will broadcast a intent 
	 */	
	public void analyzeContext() {
		
	}
	
	public void TestConnectivityManager(Context context) {
		// TODO Auto-generated constructor stub
		ConnectivityManager connectivityManager = (ConnectivityManager) context
				.getSystemService(Context.CONNECTIVITY_SERVICE);
		
		// TODO verzameling van systeem data object , do er wat mee
		VpnService vpnService = new VpnService();

		SSLSessionCache sslSessionCache = new SSLSessionCache(context);
		PskKeyManager pskKeyManager;
		Proxy proxy = new Proxy();
		IpPrefix ipPrefix;
		Network network;
		NetworkCapabilities networkCapabilities = connectivityManager
				.getNetworkCapabilities(ConnectivityManager
						.getProcessDefaultNetwork());
		NetworkInfo networkInfo;
		NetworkRequest networkRequest;

		LinkProperties linkProperties = connectivityManager
				.getLinkProperties(ConnectivityManager
						.getProcessDefaultNetwork());
		List<LinkAddress> linkAddress = linkProperties.getLinkAddresses();

		DhcpInfo dhcpInfo;
		RouteInfo routeInfo;
		TrafficStats trafficStats;

		// MAC adress Humanreadeble

		linkAddress.size();

		linkProperties.getDnsServers();
		/*
		 * vpnService.createConfigurationContext(overrideConfiguration) ;
		 * builder.addAllowedApplication(packageName);
		 * builder.addDisallowedApplication(packageName); builder.allowBypass()
		 * ;
		 */

		networkCapabilities
				.hasCapability(NetworkCapabilities.NET_CAPABILITY_NOT_VPN);
		networkCapabilities
				.hasCapability(NetworkCapabilities.NET_CAPABILITY_INTERNET);
		networkCapabilities
				.hasCapability(NetworkCapabilities.TRANSPORT_CELLULAR);
		networkCapabilities.hasCapability(NetworkCapabilities.TRANSPORT_WIFI);
		networkCapabilities.hasCapability(NetworkCapabilities.TRANSPORT_VPN);

	}

}

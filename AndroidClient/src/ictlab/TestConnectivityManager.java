package ictlab;

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

/**
 * 
 */

/**
 * @author Zero
 *
 */
public class TestConnectivityManager  {
	
	
	
	//TODO verzameling van systeem data object , do er wat mee
	VpnService vpnService; 
	VpnService.Builder  builder ; 
	
	SSLSessionCache sslSessionCache ; 
	PskKeyManager pskKeyManager;
	Proxy proxy ; 
	IpPrefix ipPrefix ; 
	Network network ;
	NetworkCapabilities networkCapabilities ;
	NetworkInfo networkInfo ;
	NetworkRequest networkRequest;
	LinkAddress linkAddress;
	LinkProperties linkProperties;
	DhcpInfo dhcpInfo;
	RouteInfo RouteInfo ; 
	TrafficStats trafficStats ; 
	
	

	/**
	 * 
	 */
	public void TestConnectivityManager() {
		// TODO Auto-generated constructor stub
	
	// MAC adress Humanreadeble
	linkAddress.getAddress().toString() ;
	linkProperties.getDnsServers() ; 
/*	
	vpnService.createConfigurationContext(overrideConfiguration) ; 
	builder.addAllowedApplication(packageName);
	builder.addDisallowedApplication(packageName);
	builder.allowBypass() ; 
*/	
	
	
	
	network.getSocketFactory();
	
	
	
	
	networkCapabilities.hasCapability(networkCapabilities.NET_CAPABILITY_NOT_VPN);
	networkCapabilities.hasCapability(networkCapabilities.NET_CAPABILITY_INTERNET);
	networkCapabilities.hasCapability(networkCapabilities.TRANSPORT_CELLULAR);
	networkCapabilities.hasCapability(networkCapabilities.TRANSPORT_WIFI);
	networkCapabilities.hasCapability(networkCapabilities.TRANSPORT_VPN);
	

	
	
	
	}

}

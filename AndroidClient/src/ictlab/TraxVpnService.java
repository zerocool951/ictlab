package ictlab;

import android.content.Intent;
import android.net.VpnService;
import android.os.ParcelFileDescriptor;
import android.util.Log;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.net.InetSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;

/**
 * Created by HK on 9-6-2015.
 */
public class TraxVpnService extends VpnService{
    private String serverAddress = "185.77.129.237";
    private int serverPort = 3306;
    private String serverDns = "8.8.8.8";
    private String serverRoute = "0.0.0.0";

    private Thread mThread;
    private ParcelFileDescriptor mInterface;
    Builder builder = new Builder();

    public int runVpn(Intent intent, int flags, int beginId) throws Exception{
        mThread = new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    InetSocketAddress server = new InetSocketAddress(serverAddress, serverPort);
                    //Configuring the builder interface
                   mInterface = builder.setSession("TraxVpn")
                           .addAddress("192.168.0.1", 45)
                           .addDnsServer(serverDns)
                           .addRoute(serverRoute, 0).establish();
                    //packets queued in input stream
                    FileInputStream in = new FileInputStream(mInterface.getFileDescriptor());
                    FileOutputStream out = new FileOutputStream(mInterface.getFileDescriptor());

                    DatagramChannel tunnel = DatagramChannel.open();
                    //connect to server
                    tunnel.connect(server);
                    protect(tunnel.socket());

                    ByteBuffer packet = ByteBuffer.allocate(65535);
                    int timer = 0;

                    //pass the packets
                    while(true){
                        boolean idle = true;

                        int length = in.read(packet.array());
                        if(length > 0){
                            if(packet.get(0) != 0){
                                out.write(packet.array(), 0, length);
                            }
                            packet.clear();

                            idle = false;

                            if(timer > 0){
                                timer = 0;
                            }
                        }

                        if(idle){
                            Thread.sleep(100);

                            timer += (timer > 0) ? 100 : -100;

                            if(timer < -15000){
                                packet.put((byte) 0).limit(1);
                                for(int i = 0; i < 3; ++i){
                                    packet.position(0);
                                    tunnel.write(packet);
                                }
                                packet.clear();
                                timer = 1;
                            }

                            if(timer > 20000){
                                throw new IllegalArgumentException("Timed out");
                            }
                        }
                    }


                } catch (Exception e){
                    Log.i ("error: ", e.toString());
                } finally{
                    try{
                        if(mInterface != null){
                            mInterface.close();
                            mInterface = null;
                        }
                    } catch(Exception e){
                        Log.i ("error: ", e.toString());
                    }
                }
            }
        }, "TraxVpn");
        mThread.start();
        return START_STICKY;
    }

    public void onDestroy(){
        if(mThread != null){
            mThread.interrupt();
        }

        super.onDestroy();
    }
}

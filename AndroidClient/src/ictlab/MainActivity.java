package ictlab;

import android.app.Activity;
import android.content.Intent;
//import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.net.VpnService;

import nl.AndroidClient.ictlab.R;


public class MainActivity extends Activity implements View.OnClickListener{
    //Button vpn_start_button = (Button) findViewById(R.id.vpn_action_button);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        findViewById(R.id.vpn_action_button).setOnClickListener(this);
    }

    public void onClick(View v){
        Intent intent = VpnService.prepare(getApplicationContext());

        if(intent != null){
            startActivityForResult(intent, 0);
        }else{
            onActivityResult(0, RESULT_OK, null);
        }
    }

    protected void onActivityResult(int requestCode, int resultCode, Intent data){
        if(resultCode == RESULT_OK){
            Intent intent = new Intent(this, TraxVpnService.class);
            startService(intent);
        }
    }
}

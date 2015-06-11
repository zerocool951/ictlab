package ictlab;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.net.VpnService;
import android.widget.TextView;

import ictlab.contextRules.ContextRuleManager;
import nl.AndroidClient.ictlab.R;


public class MainActivity extends Activity implements View.OnClickListener{
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        findViewById(R.id.vpn_action_button).setOnClickListener(this);
        findViewById(R.id.bt_startload).setOnClickListener(this);
        findViewById(R.id.bt_display_rules).setOnClickListener(this);
    }

    public void onClick(View v) {
        switch (v.getId()) {
            //Button vpn click
            case R.id.vpn_action_button:
                Intent intent = VpnService.prepare(getApplicationContext());

                if (intent != null) {
                    startActivityForResult(intent, 0);
                } else {
                    onActivityResult(0, RESULT_OK, null);
                }
                break;

            //Button startload click
            case R.id.bt_startload:
                ContextRuleManager.StartGetRules(this);
                break;

            case R.id.bt_display_rules:
                TextView tv = (TextView)findViewById(R.id.tv_ruledisplay);
                if(ContextRuleManager.status == ContextRuleManager.GetRuleStatus.SUCCESS){
                    tv.setText("Found stuff!");
                }else{
                    tv.setText(ContextRuleManager.status.toString());
                }
                break;
            default:
                break;
        }
    }

    protected void onActivityResult(int requestCode, int resultCode, Intent data){
        if(resultCode == RESULT_OK){
            Intent intent = new Intent(this, TraxVpnService.class);
            startService(intent);
        }
    }
}

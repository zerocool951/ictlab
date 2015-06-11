package ictlab.contextRules;

import android.content.Context;
import android.os.AsyncTask;
import android.text.TextUtils;
import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;

import ictlab.contextRules.model.ApplicationRule;
import ictlab.contextRules.model.Rule;
import nl.AndroidClient.ictlab.R;

/**
 * Created by GWigWam on 10-6-2015.
 */
public class ContextRuleManager {

    public static GetRuleStatus status = GetRuleStatus.NOTSTARTED;
    public static Exception thrownException;

    private static ArrayList<Rule> allRules;

    public static void StartGetRules(Context context) {
        Log.d("ContextSec_RuleMgr", "Start get rules");
        String address = context.getResources().getString(R.string.rules_webapi_address) + "/api/Rule";
        new NonBlockingHttpGetText().execute(address);
    }

    private static void ProcessJSon(String jsonText) {
        Log.d("ContextSec_RuleMgr", "Start processing json");

        if(allRules == null){
            allRules = new ArrayList<Rule>();
        }else{
            allRules.clear();
        }

        try {
            JSONArray jArray = new JSONArray(jsonText);

            for(int i = 0; i < jArray.length(); i++){
                JSONObject cur = jArray.getJSONObject(i);

                JSONArray templates = cur.getJSONArray("RuleTypeNames");
                Boolean containsBasic = false;
                String templateName = null;
                for(int j = 0; j < templates.length(); j++){
                    String curStr = templates.getString(j);
                    if(!TextUtils.isEmpty(curStr)){
                        if(curStr.equals("Basic")){
                            containsBasic = true;
                        }else{
                            templateName = curStr;
                        }
                    }
                }
                templateName = (templateName != null) ? templateName : "Basic";

                if(containsBasic){
                    Rule newRule = null;
                    try {
                        switch (templateName) {
                            case "Basic":
                                newRule = new Rule();
                                newRule.fillFromJSon(cur);
                                break;
                            case "Application":
                                newRule = new ApplicationRule();
                                newRule.fillFromJSon(cur);
                                break;
                            default:
                                break;
                        }
                        if(newRule != null) {
                            allRules.add(newRule);
                        }
                    }catch (InvalidRuleJSonException irje){
                        Log.d("ContextSec_RuleMgr", String.format("Failed to serialize JSon obj from server.\nMissing: %s\n%s", irje.GetMissingJSon(), cur.toString()));
                    }
                }else{
                    Log.d("ContextSec_RuleMgr", String.format("Failed to serialize JSon obj from server. It does not have the 'Basic' template.\n%s", cur.toString()));
                }
            }
            status = GetRuleStatus.SUCCESS;
        } catch (JSONException e) {
            thrownException = e;
            status = GetRuleStatus.ERROR;
        }
    }

    private static class NonBlockingHttpGetText extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
            Log.d("ContextSec_RuleMgr", "Do in background started...");
            status = GetRuleStatus.STARTED;

            try {
                String text = null;
                text = HttpHelper.getHttpRequestText(params[0]);
                Log.d("ContextSec_RuleMgr", "Gotten Http text");
                return text;
            } catch (IOException e) {
                Log.e("ContextSec_RuleMgr", "Get from webapi failed: " + e);
                thrownException = e;
                status = GetRuleStatus.ERROR;
            }
            return null;
        }

        @Override
        protected void onPostExecute(String s) {
            super.onPostExecute(s);
            if (s != null) {
                ProcessJSon(s);
            }else{
                Log.e("ContextSec_RuleMgr", "Execute try get json text failed, end of operations.");
            }
        }
    }

    public enum GetRuleStatus {
        NOTSTARTED, STARTED, ERROR, SUCCESS
    }
}

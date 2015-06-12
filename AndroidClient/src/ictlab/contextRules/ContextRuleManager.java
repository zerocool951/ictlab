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

import ictlab.contextRules.model.*;
import nl.AndroidClient.ictlab.R;

/**
 * Created by GWigWam on 10-6-2015.
 * Class used to handle communicating with rule webAPI
 */
public class ContextRuleManager {
    private static final String urlSuffix = "/api/Rule";

    public static RuleManagerStatus status = RuleManagerStatus.NOTSTARTED;
    public static Exception thrownException;

    private static ArrayList<Rule> allRules;

    /** Will try to load rules async (non blocking) to check progress see status property. If this ends successfully use tryGetAllRules() to get the retrieved rules.*/
    public static void startGetRules(Context context) {
        Log.d("ContextSec_RuleMgr", "Start get rules");
        String address = context.getResources().getString(R.string.rules_webapi_address) + urlSuffix;
        status = RuleManagerStatus.STARTED;
        thrownException = null;
        new NonBlockingHttpGetText().execute(address);
    }

    /** Will return gotten rules if transaction was successful, otherwise it will trow FailedGetRulesException. startGetRules() BEFORE using this method*/
    public static Rule[] tryGetAllRules() throws FailedGetRulesException{
        if(status == RuleManagerStatus.SUCCESS){
            return allRules.toArray(new Rule[allRules.size()]);
        }else{
            throw new FailedGetRulesException(status, "Rules have not been retrieved.", thrownException);
        }
    }

    private static void processJSon(String jsonText) {
        Log.d("ContextSec_RuleMgr", "Start processing json");

        if(allRules == null){
            allRules = new ArrayList<>();
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

                if(containsBasic && templateName != null){
                    Rule newRule = null;
                    try {
                        switch (templateName) {
                            case "Between":
                                newRule = new BetweenRule();
                                newRule.fillFromJSon(cur);
                                break;
                            case "Application":
                                newRule = new ApplicationRule();
                                newRule.fillFromJSon(cur);
                                break;
                            default:
                                Log.d("ContextSec_RuleMgr", String.format("Failed to serialize JSon obj from server.\nUnknown template: %s\n%s", templateName, cur.toString()));
                                break;
                        }
                        if(newRule != null) {
                            allRules.add(newRule);
                        }
                    }catch (InvalidRuleJSonException irje){
                        Log.d("ContextSec_RuleMgr", String.format("Failed to serialize JSon obj from server.\nMissing: %s\n%s", irje.GetMissingJSon(), cur.toString()));
                    }
                }else{
                    Log.d("ContextSec_RuleMgr", String.format("Failed to serialize JSon obj from server. Does not have required templates (Basic + 1Other)\n%s", cur.toString()));
                }
            }
            status = RuleManagerStatus.SUCCESS;
            Log.d("ContextSec_RuleMgr", String.format("End of processing json, found %d rules.", allRules.size()));
        } catch (JSONException e) {
            thrownException = e;
            status = RuleManagerStatus.ERROR;
        }
    }

    private static class NonBlockingHttpGetText extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
            Log.d("ContextSec_RuleMgr", "Do in background started...");

            try {
                String text = null;
                text = HttpHelper.getHttpRequestText(params[0]);
                Log.d("ContextSec_RuleMgr", "Gotten Http text");
                return text;
            } catch (IOException e) {
                Log.e("ContextSec_RuleMgr", "Get from webapi failed: " + e);
                thrownException = e;
                status = RuleManagerStatus.ERROR;
            }
            return null;
        }

        @Override
        protected void onPostExecute(String s) {
            super.onPostExecute(s);
            if (s != null) {
                processJSon(s);
            }else{
                Log.e("ContextSec_RuleMgr", "Execute try get json text failed, end of operations.");
            }
        }
    }
}
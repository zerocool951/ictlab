package ictlab.contextRules.model;

import org.json.JSONException;
import org.json.JSONObject;

import ictlab.contextRules.InvalidRuleJSonException;

/**
 * Created by GWigWam on 11-6-2015.
 */
public class ApplicationRule extends Rule {

    private String ApplicationName;

    @Override
    public void fillFromJSon(JSONObject jObject) throws InvalidRuleJSonException {
        super.fillFromJSon(jObject);

        try {
            JSONObject properties = jObject.getJSONObject("Properties");
            ApplicationName = properties.getString("Application");
        } catch (JSONException e) {
            throw new InvalidRuleJSonException("Id / Name");
        }
    }

    public String getApplicationName() {
        return ApplicationName;
    }

    public void setApplicationName(String applicationName) {
        ApplicationName = applicationName;
    }
}
package ictlab.contextRules.model;

import org.json.JSONException;
import org.json.JSONObject;

import ictlab.contextRules.InvalidRuleJSonException;

/**
 * Created by GWigWam on 11-6-2015.
 * Rule for a specific application
 */
public class ApplicationRule extends Rule {

    private String applicationName;

    @Override
    public void fillFromJSon(JSONObject jObject) throws InvalidRuleJSonException {
        super.fillFromJSon(jObject);

        try {
            JSONObject properties = jObject.getJSONObject("Properties");
            applicationName = properties.getString("Application");
        } catch (JSONException e) {
            throw new InvalidRuleJSonException("Application");
        }
    }

    @Override
    public String toString() {
        return super.toString() +  "\nApplicationRule{" +
                "applicationName='" + applicationName + '\'' +
                '}';
    }

    public String getApplicationName() {
        return applicationName;
    }

    public void setApplicationName(String applicationName) {
        this.applicationName = applicationName;
    }
}
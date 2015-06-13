package ictlab.contextRules;

import org.json.*;

/**
 * Created by GWigWam on 11-6-2015.
 * Rule for a specific application
 */
public class ApplicationRule extends Rule {

    private String applicationName;

    @Override
    void fillFromJSON(JSONObject jObject) throws InvalidRuleJSONException {
        super.fillFromJSON(jObject);

        try {
            JSONObject properties = jObject.getJSONObject("Properties");
            applicationName = properties.getString("Application");
        } catch (JSONException e) {
            throw new InvalidRuleJSONException("Application");
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
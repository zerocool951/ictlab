package ictlab.contextRules.model;

import org.json.JSONException;
import org.json.JSONObject;

import ictlab.contextRules.InvalidRuleJSonException;

/**
 * Created by GWigWam on 11-6-2015.
 */
public class Rule {
    private int Id;
    private String Name;

    public void fillFromJSon(JSONObject jObject) throws InvalidRuleJSonException {
        try {
            JSONObject properties = jObject.getJSONObject("Properties");
            Id = properties.getInt("Id");
            Name = properties.getString("Name");
        } catch (JSONException e) {
            throw new InvalidRuleJSonException("Id / Name");
        }
    }

    public String getName() {
        return Name;
    }

    public void setName(String name) {
        Name = name;
    }

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        Id = id;
    }
}

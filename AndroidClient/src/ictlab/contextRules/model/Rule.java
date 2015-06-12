package ictlab.contextRules.model;

import org.json.JSONException;
import org.json.JSONObject;

import ictlab.contextRules.InvalidRuleJSonException;

/**
 * Created by GWigWam on 11-6-2015.
 * Abstract rule contains the basic requirements every rule should have
 */
public abstract class Rule {
    private int id;
    private String name;

    /** Insert JSon gotten from server, if the JSon has the correct properties the object will be filled, otherwise InvalidRuleJSonException is thrown*/
    public void fillFromJSon(JSONObject jObject) throws InvalidRuleJSonException {
        try {
            JSONObject properties = jObject.getJSONObject("Properties");
            id = properties.getInt("Id");
            name = properties.getString("Name");
        } catch (JSONException e) {
            throw new InvalidRuleJSonException("Id / Name");
        }
    }

    @Override
    public String toString() {
        return "Rule{" +
                "id=" + id +
                ", name='" + name + '\'' +
                '}';
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }
}

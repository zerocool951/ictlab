package ictlab.contextRules;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Created by GWigWam on 11-6-2015.
 * Abstract rule contains the basic requirements every rule should have
 */
public abstract class Rule {
    private int id;
    private String name;

    /** Insert JSON gotten from server, if the JSON has the correct properties the object will be filled, otherwise InvalidRuleJSONException is thrown*/
    void fillFromJSON(JSONObject jObject) throws InvalidRuleJSONException {
        try {
            JSONObject properties = jObject.getJSONObject("Properties");
            id = properties.getInt("Id");
            name = properties.getString("Name");
        } catch (JSONException e) {
            throw new InvalidRuleJSONException("Id / Name");
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

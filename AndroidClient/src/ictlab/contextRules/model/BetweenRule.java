package ictlab.contextRules.model;

import org.json.JSONException;
import org.json.JSONObject;

import ictlab.contextRules.InvalidRuleJSonException;

/**
 * Created by GWigWam on 12-6-2015.
 * Rule for timespan
 */
public class BetweenRule extends Rule{
    private int from;
    private int until;

    @Override
    public void fillFromJSon(JSONObject jObject) throws InvalidRuleJSonException {
        super.fillFromJSon(jObject);

        try {
            JSONObject properties = jObject.getJSONObject("Properties");
            from = properties.getInt("From");
            until = properties.getInt("Until");
        } catch (JSONException e) {
            throw new InvalidRuleJSonException("From / Until");
        }
    }

    @Override
    public String toString() {
        return super.toString() + "\nBetweenRule{" +
                "from=" + from +
                ", until=" + until +
                '}';
    }

    public int getFrom() {
        return from;
    }

    public void setFrom(int from) {
        this.from = from;
    }

    public int getUntil() {
        return until;
    }

    public void setUntil(int until) {
        this.until = until;
    }
}

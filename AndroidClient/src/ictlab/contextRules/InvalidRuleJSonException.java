package ictlab.contextRules;

/**
 * Created by GWigWam on 11-6-2015.
 * JSON returned by server cannot be converted into java class from contextRules.model
 */
class InvalidRuleJSONException extends Exception {
    private final String requiredJSON;

    InvalidRuleJSONException(String requiredJSON){
        this.requiredJSON = requiredJSON;
    }

    /** The required JSON properties to create a valid rule object*/
    public String GetMissingJSON(){
        return requiredJSON;
    }
}
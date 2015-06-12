package ictlab.contextRules;

/**
 * Created by GWigWam on 11-6-2015.
 * JSon returned by server cannot be converted into java class from contextRules.model
 */
public class InvalidRuleJSonException extends Exception {
    private String requiredJSon;

    public InvalidRuleJSonException(String requiredJSon){
        this.requiredJSon = requiredJSon;
    }

    /** The required JSon properties to create a valid rule object*/
    public String GetMissingJSon(){
        return requiredJSon;
    }
}
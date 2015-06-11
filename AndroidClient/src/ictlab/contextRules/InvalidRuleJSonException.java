package ictlab.contextRules;

/**
 * Created by GWigWam on 11-6-2015.
 */
public class InvalidRuleJSonException extends Exception {
    private String MissingJSon;

    public InvalidRuleJSonException(String missingJSon){
        MissingJSon = missingJSon;
    }

    public String GetMissingJSon(){
        return MissingJSon;
    }
}
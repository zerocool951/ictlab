package ictlab.contextRules;

/**
 * Created by GWigWam on 11-6-2015.
 * Is thrown when rule collection is requested, but it cannot be returned.
 * It is possible the ContextRuleManager is still working, or there was an error
 * if there was an error check Exception.getCause()
 */
public class FailedGetRulesException extends Exception {
    private final RuleManagerStatus managerStatus;

    FailedGetRulesException(RuleManagerStatus curStatus, String detailMessage, Throwable throwable) {
        super(detailMessage, throwable);
        managerStatus = curStatus;
    }

    @Override
    public String toString() {
        return String.format("FailedGetRulesException: %s \n{\nmanagerStatus: %s\n}\nInner Exception: %s", managerStatus, this.getMessage(), this.getCause());
    }

    /** Status of the ContextRuleManger when trying to retrieve all rules*/
    public RuleManagerStatus getManagerStatus() {
        return managerStatus;
    }
}
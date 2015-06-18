package ictlab.contextRules;

import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.X509Certificate;
import java.util.Scanner;

import javax.net.ssl.*;
/**
 * Created by GWigWam on 10-6-2015.
 * Ignore SSL error code from: http://www.rgagnon.com/javadetails/java-fix-certificate-problem-in-HTTPS.html
 * Gets plain text from an url
 */
class HttpHelper {
    /** Returns plain text String gotten from input url. If url is not reachable/readable throws IOException */
    static String getHttpRequestText(String url, Boolean allowSelfSigendCertificates) throws IOException, NoSuchAlgorithmException, KeyManagementException {
        HttpURLConnection urlConnection = null;
        try {
            if(allowSelfSigendCertificates) {
                //  fix for javax.net.ssl.SSLHandshakeException / sun.security.validator.ValidatorException:
                //      unable to find valid certification path to requested target
                TrustManager[] trustAllCerts = new TrustManager[]{
                        new X509TrustManager() {
                            public java.security.cert.X509Certificate[] getAcceptedIssuers() {
                                return null;
                            }

                            public void checkClientTrusted(X509Certificate[] certs, String authType) {
                            }

                            public void checkServerTrusted(X509Certificate[] certs, String authType) {
                            }
                        }
                };

                SSLContext sc = SSLContext.getInstance("SSL");
                sc.init(null, trustAllCerts, new java.security.SecureRandom());
                HttpsURLConnection.setDefaultSSLSocketFactory(sc.getSocketFactory());

                // Create all-trusting host name verifier
                HostnameVerifier allHostsValid = new HostnameVerifier() {
                    public boolean verify(String hostname, SSLSession session) {
                        return true;
                    }
                };
                // Install the all-trusting host verifier
                HttpsURLConnection.setDefaultHostnameVerifier(allHostsValid);
            }

            urlConnection = (HttpURLConnection) new URL(url).openConnection();

            InputStream in = new BufferedInputStream(urlConnection.getInputStream());

            Scanner s = new java.util.Scanner(in).useDelimiter("\\A");
            String plainText = s.hasNext() ? s.next() : "";

            return  plainText;
        } finally {
            if (urlConnection != null) {
                urlConnection.disconnect();
            }
        }
    }
}
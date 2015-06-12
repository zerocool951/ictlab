package ictlab.contextRules;

import android.os.AsyncTask;

import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Scanner;

/**
 * Created by GWigWam on 10-6-2015.
 * Gets plain text from an url
 */
public class HttpHelper {
    /** Returns plain text String gotten from input url. If url is not reachable/readable throws IOException */
    public static String getHttpRequestText(String url) throws IOException {
        HttpURLConnection urlConnection = null;
        try {
            urlConnection = (HttpURLConnection) new URL(url).openConnection();

            InputStream in = new BufferedInputStream(urlConnection.getInputStream());

            Scanner s = new java.util.Scanner(in).useDelimiter("\\A");
            String plainText =  s.hasNext() ? s.next() : "";

            return  plainText;
        } finally {
            if (urlConnection != null) {
                urlConnection.disconnect();
            }
        }
    }
}
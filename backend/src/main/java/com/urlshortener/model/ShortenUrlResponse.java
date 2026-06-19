package com.urlshortener.model;

public class ShortenUrlResponse {
    private String shortUrl;
    private String alias;
    private String fullUrl;

    public ShortenUrlResponse() {
    }

    public ShortenUrlResponse(String shortUrl, String alias, String fullUrl) {
        this.shortUrl = shortUrl;
        this.alias = alias;
        this.fullUrl = fullUrl;
    }

    public String getShortUrl() {
        return shortUrl;
    }

    public void setShortUrl(String shortUrl) {
        this.shortUrl = shortUrl;
    }

    public String getAlias() {
        return alias;
    }

    public void setAlias(String alias) {
        this.alias = alias;
    }

    public String getFullUrl() {
        return fullUrl;
    }

    public void setFullUrl(String fullUrl) {
        this.fullUrl = fullUrl;
    }
}

package com.urlshortener.model;

public class UrlListItem {
    private String alias;
    private String fullUrl;
    private String shortUrl;

    public UrlListItem() {
    }

    public UrlListItem(String alias, String fullUrl, String shortUrl) {
        this.alias = alias;
        this.fullUrl = fullUrl;
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

    public String getShortUrl() {
        return shortUrl;
    }

    public void setShortUrl(String shortUrl) {
        this.shortUrl = shortUrl;
    }
}

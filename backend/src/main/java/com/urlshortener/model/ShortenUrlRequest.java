package com.urlshortener.model;

import jakarta.validation.constraints.NotBlank;

public class ShortenUrlRequest {

    @NotBlank(message = "fullUrl is required.")
    private String fullUrl;

    private String customAlias;

    public String getFullUrl() {
        return fullUrl;
    }

    public void setFullUrl(String fullUrl) {
        this.fullUrl = fullUrl;
    }

    public String getCustomAlias() {
        return customAlias;
    }

    public void setCustomAlias(String customAlias) {
        this.customAlias = customAlias;
    }
}

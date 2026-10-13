package com.rmeunier.meowtopia.backend.cat.exception;

import java.util.UUID;

public class CatNotFoundException extends RuntimeException {
    public CatNotFoundException() {
        super("Could not find cats");
    }

    public CatNotFoundException(UUID catId) {
        super("Could not find cat " + catId);
    }

    public CatNotFoundException(String name) {
        super("Could not find cat " + name);
    }
}

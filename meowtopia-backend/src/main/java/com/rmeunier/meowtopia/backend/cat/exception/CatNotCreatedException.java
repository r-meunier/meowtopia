package com.rmeunier.meowtopia.backend.cat.exception;

public class CatNotCreatedException extends RuntimeException {
    public CatNotCreatedException() {
        super("Cat could not be created");
    }
    public CatNotCreatedException(String errorMessage) {
        super(errorMessage);
    }
}

package com.rmeunier.meowtopia.backend.cat.exception;

import java.util.UUID;

public class BreedNotFoundException extends RuntimeException {
    public BreedNotFoundException() {
        super("Breeds not found");
    }

    public BreedNotFoundException(UUID breedId) {
        super("Breed with id " + breedId + " not found");
    }

    public BreedNotFoundException(String name) {
        super("Breed with name " + name + " not found");
    }
}

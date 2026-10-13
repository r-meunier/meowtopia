package com.rmeunier.meowtopia.backend.user.exception;

import java.util.UUID;

public class UserProfileNotFoundException extends RuntimeException {
    public UserProfileNotFoundException() {
        super("User profiles not found");
    }

    public UserProfileNotFoundException(UUID userProfileId) {
        super("User profile not found with id: " + userProfileId.toString());
    }
}

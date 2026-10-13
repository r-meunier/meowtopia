package com.rmeunier.meowtopia.backend.user.exception;

import java.util.UUID;

public class UserAccountNotFoundException extends RuntimeException {

    public UserAccountNotFoundException() {
        super("Could not find user accounts");
    }

    public UserAccountNotFoundException(UUID userAccountId) {
        super("Could not find user account " + userAccountId);
    }

    public UserAccountNotFoundException(String username) {
        super("Could not find user account " + username);
    }
}

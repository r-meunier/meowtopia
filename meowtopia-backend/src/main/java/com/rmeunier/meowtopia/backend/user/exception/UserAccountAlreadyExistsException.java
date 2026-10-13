package com.rmeunier.meowtopia.backend.user.exception;

public class UserAccountAlreadyExistsException extends RuntimeException {
    public UserAccountAlreadyExistsException(String username) {
        super("User account with username " + username + " already exists");
    }
}

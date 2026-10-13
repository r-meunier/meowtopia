package com.rmeunier.meowtopia.backend.user.exception;

import java.util.UUID;

public class UserInventoryItemNotFoundException extends RuntimeException {
    public UserInventoryItemNotFoundException() {
        super("User inventory items not found");
    }

    public UserInventoryItemNotFoundException(UUID id) {
        super("Could not find user inventory item " + id);
    }
}

package com.rmeunier.meowtopia.backend.shop.exception;

import java.util.UUID;

public class ShopItemNotFoundException extends RuntimeException {
    public ShopItemNotFoundException() {
        super("Shop item not found");
    }

    public ShopItemNotFoundException(UUID itemId) {
        super("Shop item not found with id " + itemId);
    }
}

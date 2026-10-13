package com.rmeunier.meowtopia.backend.shop.exception;

import java.util.UUID;

public class TransactionNotFoundException extends RuntimeException {
    public TransactionNotFoundException() {
        super("Transactions not found");
    }

    public TransactionNotFoundException(UUID transactionId) {
        super("Could not find transaction " + transactionId);
    }
}

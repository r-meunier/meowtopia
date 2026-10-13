package com.rmeunier.meowtopia.backend.other;

import lombok.*;

@Builder
@Getter @Setter
@NoArgsConstructor
public class GenericResponse {
    private String message;
    private String error;
    private long timestamp;

    public GenericResponse(String message, String error, long timestamp) {
        this.message = message;
        this.error = error;
        this.timestamp = timestamp;
    }

    public GenericResponse(String message, long timestamp) {
        this(message, null, timestamp);
    }
}

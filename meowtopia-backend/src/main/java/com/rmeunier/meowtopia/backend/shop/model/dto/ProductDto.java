package com.rmeunier.meowtopia.backend.shop.model.dto;

import lombok.*;

import java.util.UUID;

@Data
@Getter @Setter
@NoArgsConstructor @AllArgsConstructor
public class ProductDto {
    private UUID productId;
    private String name;
    private Double price;
}

package com.rmeunier.meowtopia.backend.cat.model.dto;

import com.rmeunier.meowtopia.backend.cat.model.CatFurType;
import jakarta.validation.constraints.NotNull;
import lombok.*;

import java.util.UUID;

@Data
@Getter @Setter
@NoArgsConstructor @AllArgsConstructor
@Builder
public class BreedDto {

    private UUID breedId;

    @NotNull(message = "Name is required")
    private String name;

    private String description;

    @NotNull(message = "Life span is required")
    private int lifeSpan;

    @NotNull(message = "Fur type is required")
    private CatFurType furType;
}
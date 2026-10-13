package com.rmeunier.meowtopia.backend.cat.model.dto;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.rmeunier.meowtopia.backend.cat.model.Gender;
import jakarta.validation.constraints.NotEmpty;
import jakarta.validation.constraints.NotNull;
import lombok.*;

import java.util.Date;
import java.util.UUID;

@Data
@Getter @Setter
@NoArgsConstructor @AllArgsConstructor
@Builder
public class CatDto {
    private UUID catId;

    @NotNull(message = "Name is required")
    @NotEmpty(message = "Name is required")
    private String name;

    private String breedName;

    @NotNull(message = "Fur color is required")
    @NotEmpty(message = "Fur color cannot be empty")
    private String furColor;

    @NotNull(message = "Eye color is required")
    @NotEmpty(message = "Eye color cannot be empty")
    private String eyeColor;

    @NotNull(message = "Gender is required")
    private Gender gender;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy HH:mm:ss")
    private Date createdAt;

    private int weight;
    private boolean isFixed;

    private int intelligence;
    private int curiosity;
    private int independence;
    private int playfulness;
    private int affectionate;
    private int laziness;

    private int hunger;
    private int thirst;
    private int happiness;
    private int health;
    private int energy;
    private int cleanliness;
}

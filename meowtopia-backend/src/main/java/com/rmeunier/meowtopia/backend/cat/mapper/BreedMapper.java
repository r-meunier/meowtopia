package com.rmeunier.meowtopia.backend.cat.mapper;

import com.rmeunier.meowtopia.backend.cat.model.Breed;
import com.rmeunier.meowtopia.backend.cat.model.dto.BreedDto;

public class BreedMapper {

    public static BreedDto mapToDto(Breed breed) {
        return BreedDto.builder()
                .breedId(breed.getBreedId())
                .name(breed.getName())
                .description(breed.getDescription())
                .lifeSpan(breed.getLifeSpan())
                .furType(breed.getFurType())
                .build();
    }

    public static Breed mapToEntity(BreedDto breedDto) {
        return Breed.builder()
                .name(breedDto.getName())
                .description(breedDto.getDescription())
                .lifeSpan(breedDto.getLifeSpan())
                .furType(breedDto.getFurType())
                .build();
    }
}

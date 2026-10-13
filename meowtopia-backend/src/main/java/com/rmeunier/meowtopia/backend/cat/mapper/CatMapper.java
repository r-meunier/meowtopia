package com.rmeunier.meowtopia.backend.cat.mapper;

import com.rmeunier.meowtopia.backend.cat.model.Cat;
import com.rmeunier.meowtopia.backend.cat.model.dto.CatDto;


public class CatMapper {
    public static CatDto mapToDto(Cat cat) {
        return CatDto.builder()
                .catId(cat.getCatId())
                .name(cat.getName())
                .breedName(cat.getBreed().getName())
                .furColor(cat.getFurColor())
                .eyeColor(cat.getEyeColor())
                .gender(cat.getGender())
                .weight(cat.getWeight())
                .isFixed(cat.isFixed())
                .intelligence(cat.getIntelligence())
                .curiosity(cat.getCuriosity())
                .independence(cat.getIndependence())
                .playfulness(cat.getPlayfulness())
                .affectionate(cat.getAffectionate())
                .laziness(cat.getLaziness())
                .hunger(cat.getHunger())
                .thirst(cat.getThirst())
                .energy(cat.getEnergy())
                .health(cat.getHealth())
                .happiness(cat.getHappiness())
                .createdAt(cat.getCreatedAt())
                .build();
    }

    public static Cat mapToEntity(CatDto catDto) {
        return Cat.builder()
                .name(catDto.getName())
                .furColor(catDto.getFurColor())
                .eyeColor(catDto.getEyeColor())
                .weight(catDto.getWeight())
                .isFixed(catDto.isFixed())
                .gender(catDto.getGender())
                .playfulness(catDto.getPlayfulness())
                .affectionate(catDto.getAffectionate())
                .curiosity(catDto.getCuriosity())
                .independence(catDto.getIndependence())
                .intelligence(catDto.getIntelligence())
                .laziness(catDto.getLaziness())
                .build();
    }
}

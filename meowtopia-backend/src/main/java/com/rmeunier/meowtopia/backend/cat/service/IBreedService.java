package com.rmeunier.meowtopia.backend.cat.service;

import com.rmeunier.meowtopia.backend.cat.model.dto.BreedDto;
import org.springframework.data.domain.Page;

import java.util.List;
import java.util.UUID;

public interface IBreedService {
    List<BreedDto> getAllBreeds();

    Page<BreedDto> getAllBreeds(Integer page, Integer size, String orderBy, String direction);

    BreedDto getBreedById(UUID breedId);

    BreedDto searchBreedByName(String name);

    BreedDto createBreed(BreedDto breed);

    BreedDto updateBreed(UUID breedId, BreedDto breed);

    boolean deleteBreed(UUID breedId);
}

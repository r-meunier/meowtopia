package com.rmeunier.meowtopia.backend.cat.service.impl;

import com.rmeunier.meowtopia.backend.cat.mapper.BreedMapper;
import com.rmeunier.meowtopia.backend.cat.model.dto.BreedDto;
import com.rmeunier.meowtopia.backend.cat.repository.IBreedRepository;
import com.rmeunier.meowtopia.backend.cat.exception.BreedNotFoundException;
import com.rmeunier.meowtopia.backend.cat.model.Breed;
import com.rmeunier.meowtopia.backend.cat.service.IBreedService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class BreedServiceImpl implements IBreedService {
    private final IBreedRepository breedRepository;

    @Autowired
    public BreedServiceImpl(IBreedRepository breedRepository) {
        this.breedRepository = breedRepository;
    }

    @Override
    public List<BreedDto> getAllBreeds() {
        List<BreedDto> allBreeds = breedRepository.findAll()
                .stream().map(BreedMapper::mapToDto)
                .toList();

        if (allBreeds.isEmpty()) {
            throw new BreedNotFoundException();
        }

        return allBreeds;
    }

    @Override
    public Page<BreedDto> getAllBreeds(Integer page, Integer size, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return breedRepository.findAll(pageRequest)
                .map(BreedMapper::mapToDto);
    }

    @Override
    public BreedDto getBreedById(UUID breedId) {
        return breedRepository.findById(breedId)
                .map(BreedMapper::mapToDto)
                .orElseThrow(() -> new BreedNotFoundException(breedId));
    }

    @Override
    public BreedDto searchBreedByName(String name) {
        // TODO implement search
        return null;
    }

    @Override
    public BreedDto createBreed(BreedDto breedDto) {
        Breed breed = BreedMapper.mapToEntity(breedDto);
        Breed save = breedRepository.save(breed);
        return BreedMapper.mapToDto(save);
    }

    @Override
    public BreedDto updateBreed(UUID breedId, BreedDto breedDto) {
        Breed breed = BreedMapper.mapToEntity(breedDto);
        Breed existingBreed = breedRepository.findById(breedId)
                .orElseThrow(() -> new BreedNotFoundException(breedId));

        // TODO validation
        existingBreed.setName(breed.getName());
        existingBreed.setDescription(breed.getDescription());
        existingBreed.setLifeSpan(breed.getLifeSpan());
        existingBreed.setFurType(breed.getFurType());

        Breed save = breedRepository.save(existingBreed);
        return BreedMapper.mapToDto(save);
    }

    @Override
    public boolean deleteBreed(UUID breedId) {
        Breed breed = breedRepository.findById(breedId)
                .orElseThrow(() -> new BreedNotFoundException(breedId));

        if (breed != null) {
            breedRepository.delete(breed);
            return true;
        }
        return false;
    }
}

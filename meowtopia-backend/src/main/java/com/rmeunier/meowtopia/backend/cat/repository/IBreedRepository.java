package com.rmeunier.meowtopia.backend.cat.repository;

import com.rmeunier.meowtopia.backend.cat.model.Breed;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;
import java.util.UUID;

public interface IBreedRepository extends JpaRepository<Breed, UUID> {

    Optional<Breed> findBreedByName(String name);
}

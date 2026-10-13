package com.rmeunier.meowtopia.backend.shop.repo.shopitems;

import com.rmeunier.meowtopia.backend.shop.model.shopitems.Drink;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface IDrinkRepository extends JpaRepository<Drink, UUID> {
}

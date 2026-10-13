package com.rmeunier.meowtopia.backend.shop.repo.shopitems;

import com.rmeunier.meowtopia.backend.shop.model.shopitems.Food;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface IFoodRepository extends JpaRepository<Food, UUID> {
}

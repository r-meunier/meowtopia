package com.rmeunier.meowtopia.backend.shop.repo;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface IShopItemRepository extends JpaRepository<ShopItem, UUID> {
}
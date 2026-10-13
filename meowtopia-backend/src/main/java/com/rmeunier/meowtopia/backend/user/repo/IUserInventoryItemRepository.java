package com.rmeunier.meowtopia.backend.user.repo;

import com.rmeunier.meowtopia.backend.user.model.UserInventoryItem;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Repository
public interface IUserInventoryItemRepository extends JpaRepository<UserInventoryItem, UUID> {
    List<UserInventoryItem> findByUserAccountUserAccountId(UUID userAccountId);
    Optional<UserInventoryItem> findByUserAccountUserAccountIdAndShopItemShopItemId(UUID userAccountId, UUID productId);
}

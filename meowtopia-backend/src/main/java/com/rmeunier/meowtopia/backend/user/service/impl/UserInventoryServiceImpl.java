package com.rmeunier.meowtopia.backend.user.service.impl;

import com.rmeunier.meowtopia.backend.shop.exception.ShopItemNotFoundException;
import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import com.rmeunier.meowtopia.backend.shop.repo.IShopItemRepository;
import com.rmeunier.meowtopia.backend.user.exception.UserAccountNotFoundException;
import com.rmeunier.meowtopia.backend.user.exception.UserInventoryItemNotFoundException;
import com.rmeunier.meowtopia.backend.user.model.UserAccount;
import com.rmeunier.meowtopia.backend.user.model.UserInventoryItem;
import com.rmeunier.meowtopia.backend.user.repo.IUserAccountRepository;
import com.rmeunier.meowtopia.backend.user.repo.IUserInventoryItemRepository;
import com.rmeunier.meowtopia.backend.user.service.IUserInventoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.UUID;

@Service
public class UserInventoryServiceImpl implements IUserInventoryService {
    private IUserAccountRepository userAccountRepository;
    private IUserInventoryItemRepository userInventoryItemRepository;
    private IShopItemRepository shopItemRepository;

    @Autowired
    public UserInventoryServiceImpl(IUserAccountRepository userAccountRepository,
                                    IUserInventoryItemRepository userInventoryItemRepository,
                                    IShopItemRepository shopItemRepository) {
        this.userAccountRepository = userAccountRepository;
        this.userInventoryItemRepository = userInventoryItemRepository;
        this.shopItemRepository = shopItemRepository;
    }

    @Override
    @Transactional
    public UserInventoryItem addProductToInventory(UUID userAccountId, UUID productId, Integer quantity) {
        UserAccount userAccount = userAccountRepository.findById(userAccountId)
                .orElseThrow(() -> new UserAccountNotFoundException(userAccountId));

        ShopItem product = shopItemRepository.findById(productId)
                .orElseThrow(() -> new ShopItemNotFoundException(productId));

        UserInventoryItem userInventoryItem = userInventoryItemRepository
                .findByUserAccountUserAccountIdAndShopItemShopItemId(userAccountId, productId)
                .orElse(new UserInventoryItem(userAccount, product, 0));

        userInventoryItem.setQuantity(userInventoryItem.getQuantity() + quantity);
        return userInventoryItemRepository.save(userInventoryItem);
    }

    @Override
    public List<UserInventoryItem> getInventoryItemsByUser(UUID userAccountId) {
        return userInventoryItemRepository.findByUserAccountUserAccountId(userAccountId);
    }

    @Override
    public UserInventoryItem getInventoryItemById(UUID inventoryItemId) {
        return userInventoryItemRepository.findById(inventoryItemId)
                .orElseThrow(() -> new UserInventoryItemNotFoundException(inventoryItemId));
    }

    @Override
    public UserInventoryItem saveInventoryItem(UserInventoryItem inventoryItem) {
        return userInventoryItemRepository.save(inventoryItem);
    }

    @Override
    public boolean deleteInventoryItem(UUID inventoryItemId) {
        if (userInventoryItemRepository.existsById(inventoryItemId)) {
            userInventoryItemRepository.deleteById(inventoryItemId);
            return true;
        }
        return false;
    }
}

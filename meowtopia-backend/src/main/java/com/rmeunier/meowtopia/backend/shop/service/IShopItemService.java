package com.rmeunier.meowtopia.backend.shop.service;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import org.springframework.data.domain.Page;

import java.util.List;
import java.util.UUID;

public interface IShopItemService<T extends ShopItem> {

    List<T> getAllShopItems();

    Page<T> getAllShopItems(Integer size, Integer page, String sort, String direction);

    T getShopItemById(UUID itemId);

    T createShopItem(T shopItem);

    void updateStock(UUID productId, int i);

    T updateShopItem(UUID shopItemId, T shopItem);

    boolean deleteShopItem(UUID itemId);
}

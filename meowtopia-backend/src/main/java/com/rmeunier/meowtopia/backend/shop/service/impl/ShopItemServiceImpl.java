package com.rmeunier.meowtopia.backend.shop.service.impl;

import com.rmeunier.meowtopia.backend.cat.utils.DateConverterUtil;
import com.rmeunier.meowtopia.backend.shop.exception.ShopItemNotFoundException;
import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import com.rmeunier.meowtopia.backend.shop.repo.IShopItemRepository;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;
import java.util.UUID;

@Service
@Primary
public class ShopItemServiceImpl implements IShopItemService {

    private final IShopItemRepository shopItemRepository;

    @Autowired
    public ShopItemServiceImpl(IShopItemRepository shopItemRepository) {
        this.shopItemRepository = shopItemRepository;
    }

    @Override
    public ShopItem getShopItemById(UUID itemId) {
        return shopItemRepository.findById(itemId)
                .orElseThrow(() -> new ShopItemNotFoundException(itemId));
    }

    @Override
    public List getAllShopItems() {
        return shopItemRepository.findAll();
    }

    public Page<ShopItem> getAllShopItems(Integer size, Integer page, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return shopItemRepository.findAll(pageRequest);
    }

    @Override
    public ShopItem createShopItem(ShopItem shopItem) {
        return shopItemRepository.save(shopItem);
    }

    @Override
    public void updateStock(UUID productId, int quantity) {
        ShopItem shopItem = shopItemRepository.findById(productId)
                .orElseThrow(() -> new ShopItemNotFoundException(productId));
        shopItem.setStock(shopItem.getStock() + quantity);
        shopItemRepository.save(shopItem);
    }

    @Override
    public ShopItem updateShopItem(UUID shopItemId, ShopItem shopItem) {
        if (shopItemRepository.existsById(shopItemId)) {
            LocalDate currentDate = LocalDate.now();
            shopItem.setUpdatedAt(DateConverterUtil.localDateToDate(currentDate));
            return shopItemRepository.save(shopItem);
        }
        return null;
    }

    @Override
    public boolean deleteShopItem(UUID itemId) {
        if (shopItemRepository.existsById(itemId)) {
            shopItemRepository.deleteById(itemId);
            return true;
        }
        return false;
    }
}

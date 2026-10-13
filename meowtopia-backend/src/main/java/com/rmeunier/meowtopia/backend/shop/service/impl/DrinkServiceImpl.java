package com.rmeunier.meowtopia.backend.shop.service.impl;

import com.rmeunier.meowtopia.backend.shop.exception.ProductNotFoundException;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.Drink;
import com.rmeunier.meowtopia.backend.shop.repo.shopitems.IDrinkRepository;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class DrinkServiceImpl implements IShopItemService<Drink> {
    private final IDrinkRepository drinkRepository;

    @Autowired
    public DrinkServiceImpl(IDrinkRepository drinkRepository) {
        this.drinkRepository = drinkRepository;
    }

    @Override
    public List<Drink> getAllShopItems() {
        return drinkRepository.findAll();
    }

    @Override
    public Page<Drink> getAllShopItems(Integer size, Integer page, String sort, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), sort);
        return drinkRepository.findAll(pageRequest);
    }

    @Override
    public Drink getShopItemById(UUID itemId) {
        return drinkRepository.findById(itemId).orElseThrow(() -> new ProductNotFoundException(itemId));
    }

    @Override
    public Drink createShopItem(Drink shopItem) {
        return drinkRepository.save(shopItem);
    }

    @Override
    public void updateStock(UUID productId, int i) {
        Drink drink = drinkRepository.findById(productId).orElseThrow(() -> new ProductNotFoundException(productId));
        drink.setStock(drink.getStock() + i);
        drinkRepository.save(drink);
    }

    @Override
    public Drink updateShopItem(UUID shopItemId, Drink shopItem) {
        Drink drink = drinkRepository.findById(shopItemId).orElseThrow(() -> new ProductNotFoundException(shopItemId));
        drink.setName(shopItem.getName());
        drink.setPrice(shopItem.getPrice());
        drink.setStock(shopItem.getStock());
        drink.setFlavor(shopItem.getFlavor());
        return drinkRepository.save(drink);
    }

    @Override
    public boolean deleteShopItem(UUID itemId) {
        if (drinkRepository.existsById(itemId)) {
            drinkRepository.deleteById(itemId);
            return true;
        }
        return false;
    }
}

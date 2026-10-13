package com.rmeunier.meowtopia.backend.shop.service.impl;

import com.rmeunier.meowtopia.backend.shop.exception.ProductNotFoundException;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.Food;
import com.rmeunier.meowtopia.backend.shop.repo.shopitems.IFoodRepository;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class FoodServiceImpl implements IShopItemService<Food> {
    private final IFoodRepository foodRepository;

    @Autowired
    public FoodServiceImpl(IFoodRepository foodRepository)  {
        this.foodRepository = foodRepository;
    }

    @Override
    public List<Food> getAllShopItems() {
        return foodRepository.findAll();
    }

    @Override
    public Page<Food> getAllShopItems(Integer size, Integer page, String sort, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), sort);
        return foodRepository.findAll(pageRequest);
    }

    @Override
    public Food getShopItemById(UUID itemId) {
        return foodRepository.findById(itemId).orElseThrow(() -> new ProductNotFoundException(itemId));
    }

    @Override
    public Food createShopItem(Food shopItem) {
        return foodRepository.save(shopItem);
    }

    @Override
    public void updateStock(UUID productId, int i) {
        Food food = foodRepository.findById(productId).orElseThrow(() -> new ProductNotFoundException(productId));
        food.setStock(food.getStock() + i);
        foodRepository.save(food);
    }

    @Override
    public Food updateShopItem(UUID shopItemId, Food shopItem) {
        Food food = foodRepository.findById(shopItemId).orElseThrow(() -> new ProductNotFoundException(shopItemId));
        food.setName(shopItem.getName());
        food.setPrice(shopItem.getPrice());
        food.setStock(shopItem.getStock());
        food.setEffectEnergy(shopItem.getEffectEnergy());
        food.setEffectHunger(shopItem.getEffectHunger());
        return foodRepository.save(food);
    }

    @Override
    public boolean deleteShopItem(UUID itemId) {
        if (foodRepository.existsById(itemId)) {
            foodRepository.deleteById(itemId);
            return true;
        }
        return false;
    }
}

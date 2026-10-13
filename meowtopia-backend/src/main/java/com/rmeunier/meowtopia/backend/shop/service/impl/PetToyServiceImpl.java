package com.rmeunier.meowtopia.backend.shop.service.impl;

import com.rmeunier.meowtopia.backend.shop.exception.ProductNotFoundException;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.PetToy;
import com.rmeunier.meowtopia.backend.shop.repo.shopitems.IPetToyRepository;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class PetToyServiceImpl implements IShopItemService<PetToy> {

    private final IPetToyRepository petToyRepository;

    @Autowired
    public PetToyServiceImpl(IPetToyRepository petToyRepository)  {
        this.petToyRepository = petToyRepository;
    }

    @Override
    public List<PetToy> getAllShopItems() {
        return petToyRepository.findAll();
    }

    @Override
    public Page<PetToy> getAllShopItems(Integer size, Integer page, String sort, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), sort);
        return petToyRepository.findAll(pageRequest);
    }

    @Override
    public PetToy getShopItemById(UUID itemId) {
        return petToyRepository.findById(itemId).orElseThrow(() -> new ProductNotFoundException(itemId));
    }

    @Override
    public PetToy createShopItem(PetToy shopItem) {
        return petToyRepository.save(shopItem);
    }

    @Override
    public void updateStock(UUID productId, int i) {
        PetToy petToy = petToyRepository.findById(productId).orElseThrow(() -> new ProductNotFoundException(productId));
        petToy.setStock(petToy.getStock() + i);
        petToyRepository.save(petToy);
    }

    @Override
    public PetToy updateShopItem(UUID shopItemId, PetToy shopItem) {
        PetToy petToy = petToyRepository.findById(shopItemId).orElseThrow(() -> new ProductNotFoundException(shopItemId));
        petToy.setName(shopItem.getName());
        petToy.setPrice(shopItem.getPrice());
        petToy.setStock(shopItem.getStock());
        petToy.setDescription(shopItem.getDescription());
        return petToyRepository.save(petToy);
    }

    @Override
    public boolean deleteShopItem(UUID itemId) {
        if (petToyRepository.existsById(itemId)) {
            petToyRepository.deleteById(itemId);
            return true;
        }
        return false;
    }
}

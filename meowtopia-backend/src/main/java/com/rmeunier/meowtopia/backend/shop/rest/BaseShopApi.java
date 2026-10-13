package com.rmeunier.meowtopia.backend.shop.rest;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import com.rmeunier.meowtopia.backend.user.service.IUserInventoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

public abstract class BaseShopApi<T extends ShopItem> {

    protected IShopItemService<T> shopItemService;

    protected IUserInventoryService userInventoryService;

    @Autowired
    public BaseShopApi(IShopItemService<T> shopItemService, IUserInventoryService userInventoryService) {
        this.shopItemService = shopItemService;
        this.userInventoryService = userInventoryService;
    }

    @PostMapping("/buy/{itemId}")
    public ResponseEntity<String> buyProduct(@PathVariable UUID itemId,
                                             @RequestParam("quantity") int quantity,
                                             @RequestParam("userId") UUID userId) {
        // Fetch the item from the appropriate service
        ShopItem shopItem = shopItemService.getShopItemById(itemId);
        if (shopItem == null) {
            return ResponseEntity.notFound().build();
        }

        // Check if there's enough stock
        if (shopItem.getStock() < quantity) {
            return ResponseEntity.badRequest().body("Insufficient stock");
        }

        // Update stock in the shop
        shopItemService.updateStock(itemId, -quantity);

        // Add item to the user's inventory
        userInventoryService.addProductToInventory(userId, itemId, quantity);

        return ResponseEntity.ok("Product purchased successfully");
    }

    @GetMapping
    public ResponseEntity<List<T>> getAllItems() {
        return ResponseEntity.ok(shopItemService.getAllShopItems());
    }

    @GetMapping("/{id}")
    public ResponseEntity<T> getItemById(@PathVariable UUID id) {
        return ResponseEntity.ok(shopItemService.getShopItemById(id));
    }

    @PostMapping
    public ResponseEntity<T> addItem(@RequestBody T item) {
        return ResponseEntity.ok(shopItemService.createShopItem(item));
    }

    @PutMapping("/{id}")
    public ResponseEntity<T> updateItem(@PathVariable UUID id, @RequestBody T item) {
        return ResponseEntity.ok(shopItemService.updateShopItem(id, item));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteItem(@PathVariable UUID id) {
        shopItemService.deleteShopItem(id);
        return ResponseEntity.noContent().build();
    }
}

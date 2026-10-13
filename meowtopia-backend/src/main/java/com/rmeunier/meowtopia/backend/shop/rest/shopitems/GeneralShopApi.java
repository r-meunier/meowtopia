package com.rmeunier.meowtopia.backend.shop.rest.shopitems;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import com.rmeunier.meowtopia.backend.shop.rest.BaseShopApi;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import com.rmeunier.meowtopia.backend.user.service.IUserInventoryService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;


@RestController
@RequestMapping("/api/shop")
public class GeneralShopApi extends BaseShopApi<ShopItem> {
    public GeneralShopApi(IShopItemService<ShopItem> shopItemService, IUserInventoryService userInventoryService) {
        super(shopItemService, userInventoryService);
    }

    @GetMapping
    public ResponseEntity<List<ShopItem>> getAllItems() {
        return ResponseEntity.ok(shopItemService.getAllShopItems());
    }

    @GetMapping("/{id}")
    public ResponseEntity<ShopItem> getItemById(@PathVariable UUID id) {
        return ResponseEntity.ok(shopItemService.getShopItemById(id));
    }

    @PutMapping("/{id}")
    public ResponseEntity<ShopItem> updateItem(@PathVariable UUID id, @RequestBody ShopItem item) {
        return ResponseEntity.ok(shopItemService.updateShopItem(id, item));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteItem(@PathVariable UUID id) {
        shopItemService.deleteShopItem(id);
        return ResponseEntity.noContent().build();
    }
}

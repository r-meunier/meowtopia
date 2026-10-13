package com.rmeunier.meowtopia.backend.shop.rest;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.Drink;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.Food;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.PetToy;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import com.rmeunier.meowtopia.backend.shop.service.impl.DrinkServiceImpl;
import com.rmeunier.meowtopia.backend.shop.service.impl.FoodServiceImpl;
import com.rmeunier.meowtopia.backend.shop.service.impl.PetToyServiceImpl;
import com.rmeunier.meowtopia.backend.user.service.IUserInventoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/shop")
public class ShopApi {
//
//    private final IShopItemService shopItemService;
//    private final IUserInventoryService userInventoryService;
//    private final FoodServiceImpl foodService;
//    private final PetToyServiceImpl petToyService;
//    private final DrinkServiceImpl drinkService;
//
//    @Autowired
//    public ShopApi(IShopItemService shopItemService,
//                   IUserInventoryService userInventoryService,
//                   FoodServiceImpl foodService,
//                   PetToyServiceImpl petToyService,
//                   DrinkServiceImpl drinkService) {
//        this.shopItemService = shopItemService;
//        this.userInventoryService = userInventoryService;
//        this.foodService = foodService;
//        this.petToyService = petToyService;
//        this.drinkService = drinkService;
//    }
//
//    @GetMapping("/products")
//    public ResponseEntity<List<ShopItem>> getAllProducts() {
//        List<ShopItem> products = new ArrayList<>();
//        return ResponseEntity.ok(products);
//    }
//
//
//    @PostMapping("/buy/{itemId}")
//    public ResponseEntity<String> buyProduct(@PathVariable UUID itemId, @RequestParam("quantity") int quantity, @RequestParam("userId") UUID userId) {
//        ShopItem shopItem = shopItemService.getShopItemById(itemId);
//        if (shopItem.getStock() < quantity) {
//            return ResponseEntity.badRequest().body("Insufficient stock");
//        }
//
//        userInventoryService.addProductToInventory(userId, itemId, quantity);
//        shopItemService.updateStock(itemId, -quantity);
//
//        return ResponseEntity.ok("Product purchased successfully");
//    }
//
//    // Food endpoints
//    @GetMapping("/foods")
//    public ResponseEntity<List<Food>> getAllFoods() {
//        return ResponseEntity.ok(foodService.getAllShopItems());
//    }
//
//    @GetMapping("/foods/{id}")
//    public ResponseEntity<Food> getFoodById(@PathVariable UUID id) {
//        return ResponseEntity.ok(foodService.getShopItemById(id));
//    }
//
//    @PostMapping("/foods")
//    public ResponseEntity<Food> addFood(@RequestBody Food food) {
//        return ResponseEntity.ok(foodService.createShopItem(food));
//    }
//
//    @PutMapping("/foods/{id}")
//    public ResponseEntity<Food> updateFood(@PathVariable UUID id, @RequestBody Food food) {
//        return ResponseEntity.ok(foodService.updateShopItem(id, food));
//    }
//
//    @DeleteMapping("/foods/{id}")
//    public ResponseEntity<Void> deleteFood(@PathVariable UUID id) {
//        foodService.deleteShopItem(id);
//        return ResponseEntity.noContent().build();
//    }
//    // Drink endpoints
//    @GetMapping("/drinks")
//    public ResponseEntity<List<Drink>> getAllDrinks() {
//        return ResponseEntity.ok(drinkService.getAllShopItems());
//    }
//
//    @GetMapping("/drinks/{id}")
//    public ResponseEntity<Drink> getDrinkById(@PathVariable UUID id) {
//        return ResponseEntity.ok(drinkService.getShopItemById(id));
//    }
//
//    @PostMapping("/drinks")
//    public ResponseEntity<Drink> addDrink(@RequestBody Drink drink) {
//        return ResponseEntity.ok(drinkService.createShopItem(drink));
//    }
//
//    @PutMapping("/drinks/{id}")
//    public ResponseEntity<Drink> updateDrink(@PathVariable UUID id, @RequestBody Drink drink) {
//        return ResponseEntity.ok(drinkService.updateShopItem(id, drink));
//    }
//
//    @DeleteMapping("/drinks/{id}")
//    public ResponseEntity<Void> deleteDrink(@PathVariable UUID id) {
//        drinkService.deleteShopItem(id);
//        return ResponseEntity.noContent().build();
//    }
//
//    // Pet Toy endpoints
//    @GetMapping("/pet-toys")
//    public ResponseEntity<List<PetToy>> getAllPetToys() {
//        return ResponseEntity.ok(petToyService.getAllShopItems());
//    }
//
//    @GetMapping("/pet-toys/{id}")
//    public ResponseEntity<PetToy> getPetToyById(@PathVariable UUID id) {
//        return ResponseEntity.ok(petToyService.getShopItemById(id));
//    }
//
//    @PostMapping("/pet-toys")
//    public ResponseEntity<PetToy> addPetToy(@RequestBody PetToy petToy) {
//        return ResponseEntity.ok(petToyService.createShopItem(petToy));
//    }
//
//    @PutMapping("/pet-toys/{id}")
//    public ResponseEntity<PetToy> updatePetToy(@PathVariable UUID id, @RequestBody PetToy petToy) {
//        return ResponseEntity.ok(petToyService.updateShopItem(id, petToy));
//    }
//
//    @DeleteMapping("/pet-toys/{id}")
//    public ResponseEntity<Void> deletePetToy(@PathVariable UUID id) {
//        petToyService.deleteShopItem(id);
//        return ResponseEntity.noContent().build();
//    }
}

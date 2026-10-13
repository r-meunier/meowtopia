package com.rmeunier.meowtopia.backend.shop.rest.shopitems;

import com.rmeunier.meowtopia.backend.shop.model.shopitems.Drink;
import com.rmeunier.meowtopia.backend.shop.rest.BaseShopApi;
import com.rmeunier.meowtopia.backend.shop.service.IShopItemService;
import com.rmeunier.meowtopia.backend.user.service.IUserInventoryService;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/shop/drinks")
public class DrinkShopApi extends BaseShopApi<Drink> {
    public DrinkShopApi(IShopItemService<Drink> shopItemService, IUserInventoryService userInventoryService) {
        super(shopItemService, userInventoryService);
    }
}

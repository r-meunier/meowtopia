package com.rmeunier.meowtopia.backend.shop.model.shopitems;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import jakarta.persistence.*;
import lombok.*;

@Getter @Setter
@NoArgsConstructor
@Entity
//@Table(name = "pet_toy")
public class PetToy extends ShopItem {

    public PetToy(String name, Double price, int stock) {
        super(name, price, stock);
    }
}
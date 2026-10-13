package com.rmeunier.meowtopia.backend.shop.model.shopitems;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import jakarta.persistence.*;
import lombok.*;

@Getter @Setter
@NoArgsConstructor
@Entity
@Table(name = "pet_toy")
public class PetToy extends ShopItem {

    @Column
    private int effectHappiness;

    @Column(name = "description", columnDefinition = "TEXT")
    private String description;

    public PetToy(String name, Double price, int stock, int effectHappiness, String description) {
        super(name, price, stock);
        this.effectHappiness = effectHappiness;
        this.description = description;
    }
}
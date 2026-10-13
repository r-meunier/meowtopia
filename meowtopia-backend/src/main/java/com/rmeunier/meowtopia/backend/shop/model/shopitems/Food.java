package com.rmeunier.meowtopia.backend.shop.model.shopitems;

import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter @Setter
@NoArgsConstructor
@Entity
@Table(name = "food")
public class Food extends ShopItem {
    @Column
    int effectHunger;

    @Column
    int effectHappiness;

    @Column
    int effectHealth;

    @Column
    int effectEnergy;

    public Food(String name, Double price, int stock) {
        super(name, price, stock);
    }

    public Food(String name,
                Double price,
                int stock,
                int effectHunger,
                int effectHappiness,
                int effectHealth,
                int effectEnergy) {
        super(name, price, stock);
        this.effectHunger = effectHunger;
        this.effectHappiness = effectHappiness;
        this.effectHealth = effectHealth;
        this.effectEnergy = effectEnergy;
    }
}
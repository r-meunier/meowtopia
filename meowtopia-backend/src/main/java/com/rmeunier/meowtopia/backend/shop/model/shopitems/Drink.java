package com.rmeunier.meowtopia.backend.shop.model.shopitems;


import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter @Setter
@NoArgsConstructor
@Entity
@Table(name = "drink")
public class Drink extends ShopItem {
    private String flavor;

    public Drink(String name, Double price, int stock, String flavor) {
        super(name, price, stock);
        this.flavor = flavor;
    }
}

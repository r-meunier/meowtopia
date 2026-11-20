package com.rmeunier.meowtopia.backend.shop.model;

import com.fasterxml.jackson.annotation.JsonFormat;
import jakarta.persistence.*;
import lombok.*;

import java.util.Date;
import java.util.UUID;

@Getter @Setter
@NoArgsConstructor
@Entity
@Inheritance(strategy = InheritanceType.JOINED)
@Table(name = "shop_items")
public abstract class ShopItem {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    @Column(name = "id")
    private UUID shopItemId;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "price", nullable = false)
    private Double price;

    @Column(name = "description", columnDefinition = "TEXT")
    private String description;

    @Column(name = "stock")
    private int stock;

    @Column(name = "image_url")
    private String imageUrl;

    @Column(name = "effect_hunger")
    private int effectHunger;

    @Column(name = "effect_thirst")
    private int effectThirst;

    @Column(name = "effect_happiness")
    private int effectHappiness;

    @Column(name = "effect_energy")
    private int effectEnergy;

    @Column(name = "effect_health")
    private int effectHealth;

    @Column(name = "effect_cleanliness")
    private int effectCleanliness;

    // Dates and timestamps

    @Column(name = "created_at")
    @Temporal(TemporalType.TIMESTAMP)
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy HH:mm:ss")
    private Date createdAt;

    @Column(name = "updated_at")
    @Temporal(TemporalType.TIMESTAMP)
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy HH:mm:ss")
    private Date updatedAt;

    public ShopItem(String name, Double price, int stock) {
        this.name = name;
        this.price = price;
        this.stock = stock;
    }
}

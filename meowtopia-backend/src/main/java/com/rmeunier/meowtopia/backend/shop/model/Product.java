//package com.rmeunier.meowtopia.backend.shop.model;
//
//import com.rmeunier.meowtopia.backend.user.model.UserInventoryItem;
//import jakarta.persistence.*;
//import lombok.Getter;
//import lombok.NoArgsConstructor;
//import lombok.Setter;
//
//import java.util.Set;
//import java.util.UUID;
//
//@Getter @Setter
//@NoArgsConstructor
//@Entity
//@Table(name = "products")
//@Inheritance(strategy = InheritanceType.JOINED)
//public abstract class Product {
//    @Id
//    @GeneratedValue(strategy = GenerationType.UUID)
//    @Column(name = "id")
//    private UUID productId;
//
//    @Column(name = "name", nullable = false)
//    private String name;
//
//    @Column(name = "price", nullable = false)
//    private Double price;
//
//    @OneToMany(mappedBy = "product", fetch = FetchType.LAZY)
//    private Set<UserInventoryItem> usersInventory;
//
//    // Constructors
//    public Product(String name, Double price) {
//        this.name = name;
//        this.price = price;
//    }
//}

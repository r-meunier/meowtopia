package com.rmeunier.meowtopia.backend.cat.model;

import com.fasterxml.jackson.annotation.JsonManagedReference;
import jakarta.persistence.*;
import lombok.*;

import java.util.Set;
import java.util.UUID;

@Getter @Setter
@NoArgsConstructor @AllArgsConstructor
@Builder
@ToString
@Entity
@Table(name = "breeds")
public class Breed {
    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    @Column(name = "breed_id")
    private UUID breedId;

    @NonNull
    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "description")
    private String description;

    @Column(name = "life_span", nullable = false)
    private int lifeSpan;

    @Enumerated(EnumType.STRING)
    @Column(name = "fur_type")
    private CatFurType furType;

    @OneToMany(mappedBy = "breed", fetch = FetchType.LAZY, cascade = CascadeType.ALL)
    @JsonManagedReference("breed-cats")
    private Set<Cat> cats;

    public Breed(String name, String description, int lifeSpan, CatFurType furType) {
        this.name = name;
        this.description = description;
        this.lifeSpan = lifeSpan;
        this.furType = furType;
    }
}

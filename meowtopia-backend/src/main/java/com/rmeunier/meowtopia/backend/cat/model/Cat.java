package com.rmeunier.meowtopia.backend.cat.model;

import com.fasterxml.jackson.annotation.JsonBackReference;
import com.fasterxml.jackson.annotation.JsonFormat;
import com.rmeunier.meowtopia.backend.cat.utils.DateConverterUtil;
import com.rmeunier.meowtopia.backend.shop.model.ShopItem;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.Food;
import com.rmeunier.meowtopia.backend.user.model.UserAccount;
import jakarta.persistence.*;
import jakarta.validation.constraints.NotEmpty;
import jakarta.validation.constraints.NotNull;
import lombok.*;

import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.Date;
import java.util.Random;
import java.util.UUID;

@Getter @Setter
@AllArgsConstructor
@Builder
@Entity
@Table(name = "cats")
public class Cat {
    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
//    @NonNull
//    @NotNull
    @Column(name = "cat_id")
    private UUID catId;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_account_id")
    @JsonBackReference("user-cats")
    private UserAccount userAccount;

    // Basic information

    @NonNull
    @NotNull(message = "Name is required")
    @NotEmpty(message = "Name is required")
    @Column(name = "name", nullable = false)
    private String name;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "breed_id", nullable = false)
    @JsonBackReference("breed-cats")
    private Breed breed;

    // store a hexadecimal color code
    @NotNull(message = "Fur color is required")
    @Column(name = "fur_color")
    private String furColor;

    // store a hexadecimal color code
    @NotNull(message = "Eye color is required")
    @Column(name = "eye_color")
    private String eyeColor;

    @Column(name = "weight")
    private int weight;

    @Transient
    private long ageInDays;

    @Transient
    private CatLifeStage lifeStage;

    @Enumerated(EnumType.STRING)
    @NotNull(message = "Gender is required")
    @Column(name = "gender", nullable = false)
    private Gender gender;

    @Column(name = "is_fixed")
    private boolean isFixed;

    // Personality traits

    @Column(name = "playfulness")
    private int playfulness;

    @Column(name = "affectionate")
    private int affectionate;

    @Column(name = "curiosity")
    private int curiosity;

    @Column(name = "independence")
    private int independence;

    @Column(name = "laziness")
    private int laziness;

    @Column(name = "intelligence")
    private int intelligence;

    // Health and well-being, pre-set basic stats

    @Column(name = "health")
    private int health;

    @Column(name = "hunger")
    private int hunger;

    @Column(name = "thirst")
    private int thirst;

    @Column(name = "energy")
    private int energy;

    @Column(name = "happiness")
    private int happiness;

    @Column(name = "cleanliness")
    private int cleanliness;

    // Dates and timestamps

    @Column(name = "created_at")
    @Temporal(TemporalType.TIMESTAMP)
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy HH:mm:ss")
    private Date createdAt;

    @Column(name = "updated_at")
    @Temporal(TemporalType.TIMESTAMP)
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy HH:mm:ss")
    private Date updatedAt;

    // Constructors

    public Cat() {
        setBasicStats();
    }

    // Methods to pre-set stats

    public int calculateAge() {
        LocalDate currentDate = LocalDate.now();
        if (createdAt != null) {
            return (int) ChronoUnit.DAYS.between(DateConverterUtil.dateToLocalDateTime(createdAt), currentDate);
        } else {
            return 0;
        }
    }

    public void setBasicStats() {
        this.ageInDays = calculateAge();

        this.health = 80;
        this.hunger = 50;
        this.thirst = 50;
        this.energy = 60;
        this.happiness = 70;
        this.cleanliness = 50;
        this.weight = 50;

        setPersonalityStats();
    }

    private void setPersonalityStats() {
        Random random = new Random();
        this.playfulness = random.nextInt(100);
        this.affectionate = random.nextInt(100);
        this.curiosity = random.nextInt(100);
        this.independence = random.nextInt(100);
        this.laziness = random.nextInt(100);
        this.intelligence = random.nextInt(100);
    }

    // Methods for updates

    public void updateCatAgeAndLifeStage() {
        // TODO implement batch processing

        // Calculate age based on createdAt date and current date
        LocalDate createdAtDate = DateConverterUtil.dateToLocalDate(createdAt);
        LocalDate currentDate = LocalDate.now();
        long daysPassed = ChronoUnit.DAYS.between(createdAtDate, currentDate);

        // Update age in years
        this.ageInDays = (int) (daysPassed / 365);

        // Update life stage based on age
        if (ageInDays < 1) {
            this.lifeStage = CatLifeStage.BABY;
        } else if (ageInDays < 3) {
            this.lifeStage = CatLifeStage.KITTEN;
        } else if (ageInDays < 10) {
            this.lifeStage = CatLifeStage.ADULT;
        } else {
            this.lifeStage = CatLifeStage.SENIOR;
        }
    }

    public void updateStatsOverTime() {
        this.hunger = Math.max(0, this.hunger - 1);
        this.thirst = Math.max(0, this.thirst - 1);
        this.happiness = Math.max(0, this.happiness - 1);
        this.health = Math.max(0, this.health - 1);
        this.cleanliness = Math.max(0, this.cleanliness - 1);
    }

    public void applyItemEffects(ShopItem item) {
        this.hunger = Math.min(100, this.hunger + item.getEffectHunger());
        this.thirst = Math.min(100, this.thirst + item.getEffectThirst());
        this.happiness = Math.min(100, this.happiness + item.getEffectHappiness());
        this.health = Math.min(100, this.health + item.getEffectHealth());
        this.cleanliness = Math.min(100, this.cleanliness + item.getEffectCleanliness());
    }
}

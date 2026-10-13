package com.rmeunier.meowtopia.backend.cat.model;

public enum CatLifeStage {
    // equivalent to 2 weeks real life and to 1 day in game
    BABY(0, 28),
    // equivalent to 1 month real life and to 2 days in game
    KITTEN(29, 364),
    // equivalent to 1 year real life and to 7 days in game
    JUNIOR(365, 1094),
    // equivalent to 2 years real life and to 14 days in game
    ADULT(1095, 3284),
    // equivalent to 3 years real life and to 21 days in game
    MATURE(3285, 5474),
    // equivalent to 4 years real life and to 28 days in game
    SENIOR(5475, 7664),
    // equivalent to 5 years real life and to 35 days in game
    GERIATRIC(7665, Integer.MAX_VALUE);

    private final int minAge;
    private final int maxAge;

    CatLifeStage(int minAge, int maxAge) {
        this.minAge = minAge;
        this.maxAge = maxAge;
    }

    public static CatLifeStage getLifeStageByAge(int age) {
        for (CatLifeStage stage : CatLifeStage.values()) {
            if (age >= stage.minAge && age <= stage.maxAge) {
                return stage;
            }
        }
        throw new IllegalArgumentException("Invalid age for life stage");
    }

    public int getMinAge() {
        return minAge;
    }

    public int getMaxAge() {
        return maxAge;
    }

    public static CatLifeStage calculateLifeStage(long ageInDays) {
        for (CatLifeStage stage : CatLifeStage.values()) {
            if (ageInDays >= stage.getMinAge() && ageInDays <= stage.getMaxAge()) {
                return stage;
            }
        }
        return null;
    }
}
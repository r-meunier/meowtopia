package com.rmeunier.meowtopia.backend.user.model;

import jakarta.persistence.*;
import lombok.*;

import java.util.Date;
import java.util.UUID;

@Getter @Setter
@NoArgsConstructor @AllArgsConstructor
@Builder
@ToString
@Entity(name = "user_profiles")
@Table(name = "user_profiles")
public class UserProfile {
    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    @Column(name = "user_profile_id")
    private UUID userProfileId;

    @Column(name = "first_name")
    private String firstName;

    @Column(name = "last_name")
    private String lastName;

    @Column(name = "bio")
    private String bio;

    @Column(name = "avatar_url")
    private String avatarUrl;

    @Column(name = "created_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date createdAt;

    @Column(name = "updated_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date updatedAt;

    @OneToOne(fetch = FetchType.LAZY)
    private UserAccount userAccount;

    public UserProfile(String firstName, String lastName, String bio, String avatarUrl) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.bio = bio;
        this.avatarUrl = avatarUrl;
    }
}

package com.rmeunier.meowtopia.backend.user.model;

import com.fasterxml.jackson.annotation.JsonManagedReference;
import com.rmeunier.meowtopia.backend.cat.model.Cat;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.PetToy;
import com.rmeunier.meowtopia.backend.shop.model.Transaction;
import jakarta.persistence.*;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotNull;
import lombok.*;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import java.util.*;

@Getter @Setter
@NoArgsConstructor @AllArgsConstructor
@Builder
@ToString
@Entity
@Table(name = "user_accounts")
public class UserAccount implements UserDetails {
    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    @Column(name = "user_account_id")
    private UUID userAccountId;

    @Getter
    @NonNull
    @Column(name = "username", nullable = false, unique = true)
    @NotNull
    private String username;

    @NonNull
    @Column(name = "email", nullable = false)
    @NotNull(message = "Email is required")
    @Email(message = "Email should be valid")
    private String email;

    @NonNull
    @Column(name = "password_hash")
    @NotNull(message = "Password is required")
    private String passwordHash;

    @Enumerated(EnumType.STRING)
    @Column(name = "role")
    private UserRole role;

    @Column(name = "enabled")
    private boolean enabled;

    @Column(name = "created_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date createdAt;

    @Column(name = "updated_at")
    @Temporal(TemporalType.TIMESTAMP)
    private Date updatedAt;

    @OneToOne(fetch = FetchType.EAGER, mappedBy = "userAccount")
    private UserProfile userProfile;

    @OneToMany(fetch = FetchType.LAZY, mappedBy = "userAccount", cascade = CascadeType.ALL, orphanRemoval = true)
    @JsonManagedReference("user-cats")
    private Set<Cat> cats;

    @OneToMany(fetch = FetchType.LAZY, mappedBy = "userAccount", cascade = CascadeType.ALL, orphanRemoval = true)
    private Set<UserInventoryItem> inventoryItems = new HashSet<>();

    @OneToMany(mappedBy = "userAccount", cascade = CascadeType.ALL)
    private Set<Transaction> transactions;

    @ManyToMany(fetch = FetchType.EAGER)
    @JoinTable(name = "user_pet_toys",
            joinColumns = @JoinColumn(name = "user_id"),
            inverseJoinColumns = @JoinColumn(name = "pet_toy_id"))
    private Set<PetToy> petToys;

    public UserAccount(String username, String email, UserRole role) {
        this.username = username;
        this.email = email;
        this.role = role;
    }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        if (this.role == UserRole.ADMIN) {
            return List.of(new SimpleGrantedAuthority("ROLE_ADMIN"), new SimpleGrantedAuthority("ROLE_USER"));
        }
        return List.of(new SimpleGrantedAuthority("ROLE_USER"));
    }

    @Override
    public String getPassword() {
        return this.passwordHash;
    }

    @Override
    public boolean isEnabled() {
        return true;
    }
}

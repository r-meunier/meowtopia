package com.rmeunier.meowtopia.backend.user.repo;

import com.rmeunier.meowtopia.backend.user.model.UserProfile;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface IUserProfileRepository extends JpaRepository<UserProfile, UUID> {
}

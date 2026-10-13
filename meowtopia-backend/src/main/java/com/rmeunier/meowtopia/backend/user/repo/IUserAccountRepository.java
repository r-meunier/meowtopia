package com.rmeunier.meowtopia.backend.user.repo;

import com.rmeunier.meowtopia.backend.user.model.UserAccount;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;
import java.util.UUID;

public interface IUserAccountRepository extends JpaRepository<UserAccount, UUID> {
    Optional<UserAccount> findByUsername(String username);
    Page<UserAccount> findAll(Pageable pageable);
    boolean existsByUsername(String username);
}

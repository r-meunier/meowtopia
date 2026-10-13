package com.rmeunier.meowtopia.backend.shop.repo;

import com.rmeunier.meowtopia.backend.shop.model.Transaction;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface ITransactionRepository extends JpaRepository<Transaction, UUID> {
    Page<Transaction> findByUserAccountUserAccountId(UUID userAccountId, Pageable pageable);
}

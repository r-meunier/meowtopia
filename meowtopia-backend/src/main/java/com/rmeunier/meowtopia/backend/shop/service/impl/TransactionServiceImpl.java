package com.rmeunier.meowtopia.backend.shop.service.impl;

import com.rmeunier.meowtopia.backend.shop.exception.TransactionNotFoundException;
import com.rmeunier.meowtopia.backend.shop.model.Transaction;
import com.rmeunier.meowtopia.backend.shop.repo.ITransactionRepository;
import com.rmeunier.meowtopia.backend.shop.service.ITransactionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
public class TransactionServiceImpl implements ITransactionService {
    private final ITransactionRepository transactionRepository;

    @Autowired
    public TransactionServiceImpl(ITransactionRepository transactionRepository) {
        this.transactionRepository = transactionRepository;
    }

    @Override
    public Page<Transaction> getTransactionsByUser(UUID userAccountId, Integer size, Integer page, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return transactionRepository.findByUserAccountUserAccountId(userAccountId, pageRequest);
    }

    @Override
    public Transaction getTransactionById(UUID transactionId) {
        return transactionRepository.findById(transactionId)
                .orElseThrow(() -> new TransactionNotFoundException(transactionId));
    }

    @Override
    public Transaction createTransaction(Transaction transaction) {
        return transactionRepository.save(transaction);
    }

    @Override
    public boolean deleteTransaction(UUID transactionId) {
        if (transactionRepository.existsById(transactionId)) {
            transactionRepository.deleteById(transactionId);
            return true;
        }
        return false;
    }
}

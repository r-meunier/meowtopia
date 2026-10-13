package com.rmeunier.meowtopia.backend.user.service.impl;

import com.rmeunier.meowtopia.backend.cat.mapper.CatMapper;
import com.rmeunier.meowtopia.backend.cat.model.dto.CatDto;
import com.rmeunier.meowtopia.backend.cat.utils.DateConverterUtil;
import com.rmeunier.meowtopia.backend.user.exception.UserAccountNotFoundException;
import com.rmeunier.meowtopia.backend.user.mapper.UserAccountMapper;
import com.rmeunier.meowtopia.backend.user.model.UserAccount;
import com.rmeunier.meowtopia.backend.user.model.dto.UserAccountDto;
import com.rmeunier.meowtopia.backend.user.repo.IUserAccountRepository;
import com.rmeunier.meowtopia.backend.user.service.IUserAccountService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;
import java.util.UUID;

@Service
public class UserAccountServiceImpl implements IUserAccountService {

    private final IUserAccountRepository userAccountRepository;

    private final PasswordEncoder passwordEncoder;

    private final AuthenticationManager authenticationManager;

    @Autowired
    public UserAccountServiceImpl(IUserAccountRepository userAccountRepository,
                                  PasswordEncoder passwordEncoder,
                                  AuthenticationManager authenticationManager) {
        this.userAccountRepository = userAccountRepository;
        this.passwordEncoder = passwordEncoder;
        this.authenticationManager = authenticationManager;
    }

    @Override
    public List<UserAccountDto> getAllUserAccounts() {
        return userAccountRepository.findAll().stream()
                .map(UserAccountMapper::mapToDto)
                .toList();
    }

    public Page<UserAccountDto> getFilteredUserAccounts(Integer page, Integer size, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return userAccountRepository.findAll(pageRequest)
                .map(UserAccountMapper::mapToDto);
    }

    @Override
    public UserAccountDto getUserAccountById(UUID userAccountId) {
        UserAccount userAccount = userAccountRepository.findById(userAccountId)
                .orElseThrow(() -> new UserAccountNotFoundException(userAccountId));
        return UserAccountMapper.mapToDto(userAccount);
    }

    @Override
    public List<CatDto> getAllCatsByUserAccountId(UUID userAccountId) {
        UserAccount userAccount = userAccountRepository.findById(userAccountId)
                .orElseThrow(() -> new UserAccountNotFoundException(userAccountId));
        return userAccount.getCats().stream().map(CatMapper::mapToDto).toList();
    }

    @Override
    public UserAccountDto getUserAccountByUsername(String username) {
        UserAccount userAccount = userAccountRepository.findByUsername(username)
                .orElseThrow(() -> new UserAccountNotFoundException("Username not found: " + username));
        return UserAccountMapper.mapToDto(userAccount);
    }

    @Override
    public UserAccountDto createUserAccount(UserAccountDto userAccountDto) {
        UserAccount userAccount = UserAccountMapper.mapToEntity(userAccountDto);
        userAccount.setPasswordHash(passwordEncoder.encode(userAccount.getPassword()));
        UserAccount save = userAccountRepository.save(userAccount);
        return UserAccountMapper.mapToDto(save);
    }

    @Override
    public UserAccountDto updateUserAccount(UUID userAccountId, UserAccountDto userAccountDto) {
        UserAccount existingUserAccount = userAccountRepository.findById(userAccountId).orElse(null);
        if (existingUserAccount != null) {
            // Update existingUserAccount fields with userAccountDTO fields
            // Here we update username and email only for example, you can add more
            existingUserAccount.setUsername(userAccountDto.getUsername());
            existingUserAccount.setEmail(userAccountDto.getEmail());


            LocalDate currentDate = LocalDate.now();
            existingUserAccount.setUpdatedAt(DateConverterUtil.localDateToDate(currentDate));

            // Save updated user account
            existingUserAccount = userAccountRepository.save(existingUserAccount);

            // Map updated user account back to DTO
            return UserAccountMapper.mapToDto(existingUserAccount);
        } else {
            throw new UserAccountNotFoundException(userAccountId);
        }
    }

    @Override
    public boolean deleteUserAccount(UUID userAccountId) {
        UserAccount userAccount = userAccountRepository.findById(userAccountId)
                .orElseThrow(() -> new UserAccountNotFoundException(userAccountId));
        if (userAccount != null) {
            userAccountRepository.delete(userAccount);
            return true;
        }
        return false;
    }

    @Override
    public UserAccountDto login(UserAccountDto userAccountDto) {
        if (userAccountDto == null) {
            throw new IllegalArgumentException("User account DTO cannot be null");
        }
        authenticationManager.authenticate(new UsernamePasswordAuthenticationToken(
                userAccountDto.getUsername(), userAccountDto.getPassword())
        );
        return userAccountDto;
    }
}

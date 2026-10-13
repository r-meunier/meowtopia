package com.rmeunier.meowtopia.backend.user.mapper;

import com.rmeunier.meowtopia.backend.user.model.UserAccount;
import com.rmeunier.meowtopia.backend.user.model.UserRole;
import com.rmeunier.meowtopia.backend.user.model.dto.UserAccountDto;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;

public class UserAccountMapper {

    public static UserAccountDto mapToDto(UserAccount userAccount) {
        return UserAccountDto.builder()
                .userAccountId(userAccount.getUserAccountId())
                .email(userAccount.getEmail())
                .password(userAccount.getPasswordHash())
                .username(userAccount.getUsername())
                .role(userAccount.getRole().toString())
                .build();
    }

    public static UserAccount mapToEntity(UserAccountDto userAccountDto) {
        return UserAccount.builder()
                .email(userAccountDto.getEmail())
                .passwordHash(userAccountDto.getPassword())
                .username(userAccountDto.getUsername())
                .role(UserRole.valueOf(userAccountDto.getRole()))
                .build();
    }

    public static UserDetails mapToUserDetails(UserAccount userAccount) {
        return User.withUsername(userAccount.getUsername())
                .password(userAccount.getPasswordHash())
                .authorities(userAccount.getAuthorities())
                .build();
    }
}

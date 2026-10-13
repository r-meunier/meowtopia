package com.rmeunier.meowtopia.backend.user.model.dto;

import com.rmeunier.meowtopia.backend.user.validation.Password;
import com.rmeunier.meowtopia.backend.user.validation.PasswordMatches;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotEmpty;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import lombok.*;

import java.util.UUID;

@Data
@Getter @Setter
@NoArgsConstructor @AllArgsConstructor
@Builder
@PasswordMatches
public class UserAccountDto {

    private UUID userAccountId;

    @NotNull(message = "Username is required")
    @NotEmpty
    private String username;

    @NotNull(message = "Email is required")
    @NotEmpty
    @Email(message = "Email must be valid")
    private String email;

    @NotNull(message = "Password is required")
    @NotEmpty
    @Size(min = 8, message = "Password must be at least 8 characters long")
    @Password
    private String password;

    private String passwordConfirmation;

    private String role;
}

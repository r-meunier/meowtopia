package com.rmeunier.meowtopia.backend.user.validation;

import com.rmeunier.meowtopia.backend.user.model.dto.UserAccountDto;
import jakarta.validation.ConstraintValidator;
import jakarta.validation.ConstraintValidatorContext;

public class PasswordMatchesValidator
        implements ConstraintValidator<PasswordMatches, Object> {

    @Override
    public void initialize(PasswordMatches constraintAnnotation) {
    }
    @Override
    public boolean isValid(Object obj, ConstraintValidatorContext context){
        UserAccountDto user = (UserAccountDto) obj;
        return user.getPassword().equals(user.getPasswordConfirmation());
    }
}
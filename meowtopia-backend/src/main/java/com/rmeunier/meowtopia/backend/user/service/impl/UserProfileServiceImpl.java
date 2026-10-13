package com.rmeunier.meowtopia.backend.user.service.impl;

import com.rmeunier.meowtopia.backend.user.exception.UserProfileNotFoundException;
import com.rmeunier.meowtopia.backend.user.mapper.UserAccountMapper;
import com.rmeunier.meowtopia.backend.user.model.UserAccount;
import com.rmeunier.meowtopia.backend.user.model.UserProfile;
import com.rmeunier.meowtopia.backend.user.model.dto.UserAccountDto;
import com.rmeunier.meowtopia.backend.user.repo.IUserProfileRepository;
import com.rmeunier.meowtopia.backend.user.service.IUserAccountService;
import com.rmeunier.meowtopia.backend.user.service.IUserProfileService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
public class UserProfileServiceImpl implements IUserProfileService {

    private final IUserAccountService userAccountService;

    private final IUserProfileRepository userProfileRepository;

    @Autowired
    public UserProfileServiceImpl(IUserAccountService userAccountService,
                                  IUserProfileRepository userProfileRepository) {
        this.userAccountService = userAccountService;
        this.userProfileRepository = userProfileRepository;
    }


    @Override
    public UserProfile getUserProfileById(UUID userProfileId) {
        return userProfileRepository.findById(userProfileId)
                .orElseThrow(() -> new UserProfileNotFoundException(userProfileId));
    }

    @Override
    public UserProfile createUserProfileForUserAccount(UUID userAccountId, UserProfile userProfile) {
        UserAccountDto userAccountDtoById = userAccountService.getUserAccountById(userAccountId);
        UserAccount userAccountById = UserAccountMapper.mapToEntity(userAccountDtoById);

        userProfile.setUserAccount(userAccountById);

        // TODO validation on user profile

        return userProfileRepository.save(userProfile);
    }

    @Override
    public UserProfile updateUserProfile(UUID userAccountId, UserProfile userProfile) {
        UserAccountDto userAccountDtoById = userAccountService.getUserAccountById(userAccountId);
        UserAccount userAccountById = UserAccountMapper.mapToEntity(userAccountDtoById);

        userProfile.setUserAccount(userAccountById);

        // TODO validation on user profile

        return userProfileRepository.save(userProfile);
    }
}

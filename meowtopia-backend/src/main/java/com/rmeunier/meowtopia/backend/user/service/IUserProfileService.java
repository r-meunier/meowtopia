package com.rmeunier.meowtopia.backend.user.service;

import com.rmeunier.meowtopia.backend.user.model.UserProfile;

import java.util.UUID;

public interface IUserProfileService {

    UserProfile getUserProfileById(UUID userProfileId);

    UserProfile createUserProfileForUserAccount(UUID userAccountId, UserProfile userProfile);

    UserProfile updateUserProfile(UUID userProfileId, UserProfile userProfile);

}

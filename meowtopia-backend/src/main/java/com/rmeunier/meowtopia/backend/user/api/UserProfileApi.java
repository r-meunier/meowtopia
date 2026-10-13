package com.rmeunier.meowtopia.backend.user.api;

import com.rmeunier.meowtopia.backend.user.model.UserProfile;
import com.rmeunier.meowtopia.backend.user.service.IUserProfileService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.UUID;

@RestController
@RequestMapping("/api/user/profiles")
public class UserProfileApi {
    private final IUserProfileService userProfileService;

    @Autowired
    public UserProfileApi(IUserProfileService userProfileService) {
        this.userProfileService = userProfileService;
    }

    @GetMapping("/{userProfileId}")
    public ResponseEntity<UserProfile> getUserProfileById(@PathVariable("userProfileId") UUID userProfileId) {
        return ResponseEntity.ok(userProfileService.getUserProfileById(userProfileId));
    }
}

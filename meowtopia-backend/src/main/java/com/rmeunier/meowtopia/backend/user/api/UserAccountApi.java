package com.rmeunier.meowtopia.backend.user.api;

import com.rmeunier.meowtopia.backend.cat.model.dto.CatDto;
import com.rmeunier.meowtopia.backend.cat.service.ICatService;
import com.rmeunier.meowtopia.backend.other.GenericResponse;
import com.rmeunier.meowtopia.backend.user.model.UserInventoryItem;
import com.rmeunier.meowtopia.backend.user.model.dto.UserAccountDto;
import com.rmeunier.meowtopia.backend.user.service.IUserAccountService;
import com.rmeunier.meowtopia.backend.user.service.IUserInventoryService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.*;

import java.time.Instant;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/users")
@Validated
public class UserAccountApi {
    private final IUserAccountService userAccountService;

    private final IUserInventoryService userInventoryService;

    private final ICatService catService;

    @Autowired
    public UserAccountApi(IUserAccountService userAccountService,
                          IUserInventoryService userInventoryService,
                          ICatService catService) {
        this.userAccountService = userAccountService;
        this.userInventoryService = userInventoryService;
        this.catService = catService;
    }

    @GetMapping
    public ResponseEntity<Page<UserAccountDto>> getFilteredUsers(@RequestParam(value = "page", defaultValue = "0") Integer page,
                                                              @RequestParam(value = "size", defaultValue = "3") Integer size,
                                                              @RequestParam(value = "orderBy", defaultValue = "createdAt") String orderBy,
                                                              @RequestParam(value = "direction", defaultValue = "ASC") String direction) {
        return ResponseEntity.ok(userAccountService.getFilteredUserAccounts(page, size, orderBy, direction));
    }

    @GetMapping("/{userAccountId}")
    public ResponseEntity<UserAccountDto> getUserAccountById(@PathVariable("userAccountId") UUID userAccountId) {
        return ResponseEntity.ok(userAccountService.getUserAccountById(userAccountId));
    }

    @GetMapping("{userAccountId}/cats")
    public ResponseEntity<List<CatDto>> getAllCatsByUserAccountId(@PathVariable("userAccountId") UUID userAccountId) {
        return ResponseEntity.ok(userAccountService.getAllCatsByUserAccountId(userAccountId));
    }


    @GetMapping("/{userAccountId}/inventory")
    public ResponseEntity<List<UserInventoryItem>> getAllInventoryItems(UUID inventoryItemId) {
        return ResponseEntity.ok(userInventoryService.getInventoryItemsByUser(inventoryItemId));
    }

    @PostMapping
    public ResponseEntity<UserAccountDto> signUpAndCreateUserAccount(@RequestBody @Valid UserAccountDto userAccountDto) {
        return ResponseEntity.ok(userAccountService.createUserAccount(userAccountDto));
    }

    @PostMapping("/login")
    public ResponseEntity<UserAccountDto> login(@RequestBody @Valid UserAccountDto userAccountDto) {
        return ResponseEntity.ok(userAccountService.login(userAccountDto));
    }

    @PostMapping("{userAccountId}/cats")
    public ResponseEntity<CatDto> createCatForUserAccount(
            @PathVariable("userAccountId") String userAccountIdString,
            @RequestParam("breedId") String breedIdString,
            @Valid @RequestBody CatDto catDto) {
        UUID userAccountId = UUID.fromString(userAccountIdString);
        UUID breedId = UUID.fromString(breedIdString);
        return ResponseEntity.ok(catService.createCat(userAccountId, breedId, catDto));
    }

    @PutMapping("/{userAccountId}")
    public ResponseEntity<UserAccountDto> updateUserAccount(@PathVariable("userAccountId") UUID userAccountId, @Valid  UserAccountDto userAccountDto) {
        return ResponseEntity.ok(userAccountService.updateUserAccount(userAccountId, userAccountDto));
    }

    @DeleteMapping("/{userAccountId}")
    public ResponseEntity<UserAccountDto> deleteUserAccount(@PathVariable("userAccountId") UUID userAccountId) {
        userAccountService.deleteUserAccount(userAccountId);
        return ResponseEntity.noContent().build();
    }

    @ExceptionHandler(MethodArgumentNotValidException.class)
    @ResponseStatus(HttpStatus.BAD_REQUEST)
    public GenericResponse handlePasswordValidationException(MethodArgumentNotValidException e) {

        return GenericResponse.builder()
                .message(String.join(",", e.getMessage()))
                .timestamp(Instant.now().getEpochSecond())
                .build();

    }
}

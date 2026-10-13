package com.rmeunier.meowtopia.backend.cat.api;

import com.rmeunier.meowtopia.backend.cat.model.dto.CatDto;
import com.rmeunier.meowtopia.backend.cat.service.ICatService;
import com.rmeunier.meowtopia.backend.other.GenericResponse;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.*;

import java.time.Instant;
import java.util.UUID;

@RestController
@RequestMapping("/api/cats")
public class CatApi {
    private final ICatService catService;

    @Autowired
    public CatApi(ICatService catService) {
        this.catService = catService;
    }

    @GetMapping
    public ResponseEntity<Page<CatDto>> getAllCats(
            @RequestParam(value = "page", defaultValue = "0") Integer page,
            @RequestParam(value = "size", defaultValue = "5") Integer size,
            @RequestParam(value = "orderBy", defaultValue = "createdAt") String orderBy,
            @RequestParam(value = "direction", defaultValue = "DESC") String direction) {
        return ResponseEntity.ok(catService.getAllCats(page, size, orderBy, direction));
    }

    @GetMapping("/{catId}")
    public ResponseEntity<CatDto> getCatById(@PathVariable("catId") UUID catId) {
        CatDto catDto = catService.findById(catId);
        if (catDto != null) {
            return ResponseEntity.ok(catDto);
        }
        return ResponseEntity.notFound().build();
    }

    @PostMapping
    public ResponseEntity<CatDto> createCat(
            @RequestParam("breedId") UUID breedId,
            @Valid @RequestBody CatDto catDto) {
        CatDto createdCat = catService.createCat(breedId, catDto);
        return ResponseEntity.status(HttpStatus.CREATED).body(createdCat);
    }

    @PutMapping("/{catId}/feed")
    public ResponseEntity<CatDto> feedCat(@PathVariable("catId") UUID catId,
                                          @RequestParam("foodId") UUID foodId) {
        CatDto fedCat = catService.feedCat(catId, foodId);
        if (fedCat != null) {
            return ResponseEntity.ok(fedCat);
        }
        return ResponseEntity.notFound().build();
    }

    @PutMapping("/{catId}")
    public ResponseEntity<CatDto> updateCat(@PathVariable("catId") UUID catId, @Valid @RequestBody CatDto updateCat) {
        CatDto updatedCat = catService.updateCat(catId, updateCat);
        if (updatedCat != null) {
            return ResponseEntity.ok(updatedCat);
        }
        return ResponseEntity.notFound().build();
    }

    @DeleteMapping("/{catId}")
    public ResponseEntity<Void> deleteCat(@PathVariable("catId") UUID catId) {
        if (catService.deleteCat(catId)) {
            return ResponseEntity.noContent().build();
        }
        return ResponseEntity.notFound().build();
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

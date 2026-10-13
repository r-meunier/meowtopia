package com.rmeunier.meowtopia.backend.cat.service.impl;

import com.rmeunier.meowtopia.backend.cat.exception.BreedNotFoundException;
import com.rmeunier.meowtopia.backend.cat.exception.CatNotCreatedException;
import com.rmeunier.meowtopia.backend.cat.mapper.CatMapper;
import com.rmeunier.meowtopia.backend.cat.model.Breed;
import com.rmeunier.meowtopia.backend.cat.model.dto.CatDto;
import com.rmeunier.meowtopia.backend.cat.repository.IBreedRepository;
import com.rmeunier.meowtopia.backend.cat.repository.ICatRepository;
import com.rmeunier.meowtopia.backend.cat.exception.CatNotFoundException;
import com.rmeunier.meowtopia.backend.cat.model.Cat;
import com.rmeunier.meowtopia.backend.cat.service.ICatService;
import com.rmeunier.meowtopia.backend.cat.utils.DateConverterUtil;
import com.rmeunier.meowtopia.backend.shop.exception.ProductNotFoundException;
import com.rmeunier.meowtopia.backend.shop.model.shopitems.Food;
import com.rmeunier.meowtopia.backend.shop.repo.shopitems.IFoodRepository;
import com.rmeunier.meowtopia.backend.user.exception.UserAccountNotFoundException;
import com.rmeunier.meowtopia.backend.user.model.UserAccount;
import com.rmeunier.meowtopia.backend.user.repo.IUserAccountRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;
import java.util.UUID;

@Service
public class CatServiceImpl implements ICatService {
    private final ICatRepository catRepository;

    private final IBreedRepository breedRepository;

    private final IUserAccountRepository userAccountRepository;

    private final IFoodRepository foodRepository;

    @Autowired
    public CatServiceImpl(ICatRepository catRepository,
                          IBreedRepository breedRepository,
                          IUserAccountRepository userAccountRepository,
                          IFoodRepository foodRepository) {
        this.catRepository = catRepository;
        this.breedRepository = breedRepository;
        this.userAccountRepository = userAccountRepository;
        this.foodRepository = foodRepository;
    }

    @Override
    public CatDto findById(UUID catId) {
        Cat cat = catRepository.findById(catId)
                .orElseThrow(() -> new CatNotFoundException(catId));
        if (cat != null) {
            return CatMapper.mapToDto(cat);
        }
        return null;
    }

    @Override
    public List<CatDto> getAllCats() {
        return catRepository.findAll()
                .stream().map(CatMapper::mapToDto)
                .toList();
    }

    @Override
    public Page<CatDto> getAllCats(Integer page, Integer size, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return catRepository.findAll(pageRequest)
                .map(CatMapper::mapToDto);
    }

    @Override
    public List<CatDto> getAllCatsByUserAccountId(UUID userAccountId) {
        return catRepository.findByUserAccountUserAccountId(userAccountId)
                .stream().map(CatMapper::mapToDto)
                .toList();
    }

    @Override
    public Page<CatDto> getAllCatsFiltered(Integer page, Integer size, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return catRepository.findAll(pageRequest)
                .map(CatMapper::mapToDto);
    }

    @Override
    public Page<CatDto> getAllCatsByUserAccountIdFiltered(UUID userAccountId, Integer page, Integer size, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return catRepository.findByUserAccountUserAccountId(userAccountId, pageRequest)
                .map(CatMapper::mapToDto);
    }

    @Transactional
    @Override
    public CatDto createCat(UUID breedId, CatDto catDto) {
        Cat cat = CatMapper.mapToEntity(catDto);

        Breed breedById = breedRepository.findById(breedId)
                .orElseThrow(() -> new BreedNotFoundException(breedId));

        cat.setBreed(breedById);

        if (breedById == null) {
            throw new BreedNotFoundException(breedId);
        }
        cat.setBreed(breedById);
        cat = setNewCatAgeAndLifeStage(cat);

        Cat save = catRepository.save(cat);
        return CatMapper.mapToDto(save);
    }

    @Transactional
    public CatDto createCat(UUID userAccountId, UUID breedId, CatDto catDto) {
        if (breedId == null || userAccountId == null) {
            throw new CatNotCreatedException();
        }

        UserAccount userAccount = userAccountRepository.findById(userAccountId)
                .orElseThrow(() -> new UserAccountNotFoundException(userAccountId));

        Breed breed = breedRepository.findById(breedId)
                .orElseThrow(() -> new BreedNotFoundException(breedId));

        Cat cat = CatMapper.mapToEntity(catDto);
        cat.setUserAccount(userAccount);
        cat.setBreed(breed);
        cat.setBasicStats();

        cat = setNewCatAgeAndLifeStage(cat);
        Cat save = catRepository.save(cat);

        return CatMapper.mapToDto(save);
    }

    @Transactional
    @Override
    public CatDto feedCat(UUID catId, UUID foodId) {
        Cat cat = catRepository.findById(catId)
                .orElseThrow(() -> new CatNotFoundException(catId));

        Food food = foodRepository.findById(foodId)
                .orElseThrow(() -> new ProductNotFoundException(foodId));

        if (food != null) {
            cat.feed(food);
            cat = catRepository.save(cat);
        }

        return CatMapper.mapToDto(cat);
    }

        @Transactional
    @Override
    public CatDto updateCat(UUID catId, CatDto updatedCatDto) {
        Cat updatedCat = CatMapper.mapToEntity(updatedCatDto);
        // TODO fix breed
        Cat existingCat = catRepository.findById(catId)
                .orElseThrow(() -> new CatNotFoundException(catId));

        if (existingCat == null) {
            throw new CatNotFoundException(catId);
        }

        existingCat.setName(updatedCat.getName());
        existingCat.setBreed(updatedCat.getBreed());
        existingCat.setFurColor(updatedCat.getFurColor());
        existingCat.setEyeColor(updatedCat.getEyeColor());

        LocalDate currentDate = LocalDate.now();
        existingCat.setUpdatedAt(DateConverterUtil.localDateToDate(currentDate));

        existingCat.setGender(updatedCat.getGender());
        existingCat.setWeight(updatedCat.getWeight());
        existingCat.setFixed(updatedCat.isFixed());

        existingCat.setIntelligence(updatedCat.getIntelligence());
        existingCat.setCuriosity(updatedCat.getCuriosity());
        existingCat.setIndependence(updatedCat.getIndependence());
        existingCat.setPlayfulness(updatedCat.getPlayfulness());
        existingCat.setAffectionate(updatedCat.getAffectionate());

        existingCat.setHappiness(updatedCat.getHappiness());
        existingCat.setHunger(updatedCat.getHunger());
        existingCat.setThirst(updatedCat.getThirst());
        existingCat.setHealth(updatedCat.getHealth());
        existingCat.setEnergy(updatedCat.getEnergy());
        existingCat.setCleanliness(updatedCat.getCleanliness());

        Cat save = catRepository.save(existingCat);

        return CatMapper.mapToDto(save);
    }

    @Override
    public boolean deleteCat(UUID catId) {
        Cat cat = catRepository.findById(catId)
                .orElseThrow(() -> new CatNotFoundException(catId));
        if (cat != null) {
            catRepository.delete(cat);
            return true;
        }
        return false;
    }

    @Override
    public List<CatDto> findByBreedId(UUID breedId) {
        List<CatDto> cats = catRepository.findByBreedBreedId(breedId)
                .stream().map(CatMapper::mapToDto)
                .toList();
        if (cats.isEmpty()) {
            throw new CatNotFoundException();
        }
        return cats;
    }

    @Override
    public Page<CatDto> findByBreedId(UUID breedId, Integer page, Integer size, String orderBy, String direction) {
        PageRequest pageRequest = PageRequest.of(page, size, Sort.Direction.valueOf(direction), orderBy);
        return catRepository.findByBreedBreedId(breedId, pageRequest)
                .map(CatMapper::mapToDto);
    }

    @Override
    @Transactional
    @Async
    public void updateAllCatAgesAndLifeStages() {
        List<Cat> cats = catRepository.findAll();
        cats.parallelStream().forEach(cat -> {
            cat.updateCatAgeAndLifeStage();
            catRepository.save(cat);
        });
    }

    private Cat setNewCatAgeAndLifeStage(Cat cat) {
        LocalDate currentDate = LocalDate.now();
        if (cat.getCreatedAt() == null) {
            cat.setCreatedAt(DateConverterUtil.localDateToDate(currentDate));
//            cat.setLifeStage(cat.calculateLifeStage();
        } else {
            cat.setAgeInDays(cat.calculateAge());
        }
        return cat;
    }
}

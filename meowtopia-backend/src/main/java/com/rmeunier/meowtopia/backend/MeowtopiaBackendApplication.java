package com.rmeunier.meowtopia.backend;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.security.servlet.SecurityAutoConfiguration;

@SpringBootApplication(exclude = { SecurityAutoConfiguration.class }, scanBasePackages = "com.rmeunier.meowtopia.backend")
public class MeowtopiaBackendApplication {

    public static void main(String[] args) {
        SpringApplication.run(MeowtopiaBackendApplication.class, args);
    }

}

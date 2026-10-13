CREATE TABLE IF NOT EXISTS user_accounts (
    user_account_id UUID PRIMARY KEY,
    username VARCHAR(255) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(255) NOT NULL,
    enabled BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE user_profiles (
    user_profile_id UUID PRIMARY KEY,
    user_account_id UUID REFERENCES user_accounts(user_account_id),
    first_name VARCHAR(255),
    last_name VARCHAR(255),
    bio TEXT,
    avatar_url VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE breeds (
    breed_id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    life_span INTEGER NOT NULL,
    fur_type VARCHAR(50)
);

CREATE TABLE cats (
    cat_id UUID PRIMARY KEY,
    user_account_id UUID REFERENCES user_accounts(user_account_id),
    name VARCHAR(255) NOT NULL,
    breed_id UUID REFERENCES breeds(breed_id),
    color VARCHAR(255),
    birth_date DATE,
    gender VARCHAR(50),
    weight INTEGER,
    is_fixed BOOLEAN,
    intelligence INTEGER,
    sociability INTEGER,
    activity INTEGER,
    curiosity INTEGER,
    independence INTEGER,
    stubbornness INTEGER,
    playfulness INTEGER,
    affection INTEGER,
    happiness INTEGER,
    hunger INTEGER,
    thirst INTEGER,
    health INTEGER,
    energy INTEGER,
    cleanliness INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE shop_products (
    shop_product_id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10, 2),
    description TEXT,
    image VARCHAR(255),
    category VARCHAR(50),
    stock INT
);

CREATE TABLE pet_toys (
    pet_toy_id UUID PRIMARY KEY REFERENCES shop_products(shop_product_id),
    quantity INT
);



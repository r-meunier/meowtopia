CREATE TABLE user_accounts (
                               user_account_id  uuid NOT NULL PRIMARY KEY,
                               username        VARCHAR(255) NOT NULL UNIQUE ,
                               email            VARCHAR(255) NOT NULL,
                               password_hash    VARCHAR(255) NOT NULL ,
                               enabled          BOOLEAN,
                               role             VARCHAR(255)
                                   CONSTRAINT user_accounts_role_check CHECK ((role)::TEXT = ANY (
                                       (ARRAY [
                                           'ADMIN'::CHARACTER VARYING,
                                           'USER'::CHARACTER VARYING
                                           ])::TEXT[])
                                       ),
                               created_at       TIMESTAMP(6) WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                               updated_at       TIMESTAMP(6) WITH TIME ZONE
);

ALTER TABLE user_accounts
    OWNER TO rmeunier;

CREATE TABLE user_profiles (
                               user_profile_id              uuid NOT NULL PRIMARY KEY,
                               first_name                   VARCHAR(50),
                               last_name                    VARCHAR(50),
                               bio                          VARCHAR(255),
                               avatar_url                   VARCHAR(255),
                               created_at                   TIMESTAMP(6) WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                               updated_at                   TIMESTAMP(6) WITH TIME ZONE,
                               user_account_user_account_id uuid UNIQUE
                                   CONSTRAINT fkr19nlpa51kg1nn6nnldu5mhoj REFERENCES user_accounts
);

ALTER TABLE user_profiles
    OWNER TO rmeunier;

CREATE TABLE breeds (
                        breed_id    uuid NOT NULL  PRIMARY KEY,
                        name        VARCHAR(255) NOT NULL,
                        life_span   INTEGER NOT NULL,
                        description VARCHAR(255),
                        fur_type    VARCHAR(255)
                            CONSTRAINT breeds_fur_type_check
                                CHECK ((fur_type)::TEXT = ANY
                                       ((ARRAY [
                                           'SHORT'::CHARACTER VARYING,
                                           'MEDIUM'::CHARACTER VARYING,
                                           'LONG'::CHARACTER VARYING,
                                           'HAIRLESS'::CHARACTER VARYING,
                                           'CURLY'::CHARACTER VARYING,
                                           'WIRE'::CHARACTER VARYING,
                                           'FLUFFY'::CHARACTER VARYING,
                                           'SMOOTH'::CHARACTER VARYING,
                                           'SILKY'::CHARACTER VARYING,
                                           'COARSE'::CHARACTER VARYING
                                           ])::TEXT[])),
                        created_at TIMESTAMP(6) WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP(6) WITH TIME ZONE
);

ALTER TABLE breeds
    OWNER TO rmeunier;

CREATE TABLE cats
(
    cat_id          uuid NOT NULL PRIMARY KEY,
    name            VARCHAR(120) NOT NULL,
    eye_color       VARCHAR(100) NOT NULL,
    fur_color       VARCHAR(100) NOT NULL,
    gender          VARCHAR(50) NOT NULL
        CONSTRAINT cats_gender_check
            CHECK ((gender)::TEXT = ANY ((ARRAY [
                'MALE'::CHARACTER VARYING,
                'FEMALE'::CHARACTER VARYING
                ])::TEXT[])),
    breed_id        uuid         NOT NULL
        CONSTRAINT fkollwb0vu3wn5103ec0rs6a2i7 REFERENCES breeds,
    weight          INTEGER,
    is_fixed        BOOLEAN,
    hunger          INTEGER,
    thirst          INTEGER,
    happiness       INTEGER,
    health          INTEGER,
    energy          INTEGER,
    affectionate    INTEGER,
    cleanliness     INTEGER,
    curiosity       INTEGER,
    independence    INTEGER,
    intelligence    INTEGER,
    laziness        INTEGER,
    playfulness     INTEGER,
    created_at      TIMESTAMP(6) WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMP(6) WITH TIME ZONE,
    user_account_id uuid
        CONSTRAINT fkf1hrx53odfpnyn8q0pdjkh1qo REFERENCES user_accounts
);

ALTER TABLE cats
    OWNER TO rmeunier;

CREATE TABLE shop_items
(
    id                  uuid NOT NULL PRIMARY KEY,
    name                VARCHAR(200) NOT NULL,
    price               DOUBLE PRECISION NOT NULL,
    description         TEXT,
    stock               INTEGER,
    image_url           VARCHAR(255),
    type                VARCHAR(100)
        CONSTRAINT shop_item_type_check
            CHECK ((type)::TEXT = ANY ((ARRAY [
                'FOOD'::CHARACTER VARYING,
                'DRINK'::CHARACTER VARYING,
                'PET_TOY'::CHARACTER VARYING,
                'CAT_FURNITURE'::CHARACTER VARYING
                ])::TEXT[])),
    effect_hunger       INT,
    effect_thirst       INT,
    effect_happiness    INT,
    effect_health       INT,
    effect_energy       INT,
    effect_cleanliness  INT,
    created_at          TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at          TIMESTAMP WITH TIME ZONE
);

ALTER TABLE shop_items
    OWNER TO rmeunier;

CREATE TABLE food
(
    id               uuid NOT NULL PRIMARY KEY
        CONSTRAINT fk3hk73v63m29xlnjs7q33939qc REFERENCES shop_items
);

ALTER TABLE food
    OWNER TO rmeunier;

CREATE TABLE pet_toy
(
    id               uuid NOT NULL PRIMARY KEY
        CONSTRAINT fkil5h4ym1no0dps9bdimmoeu53 REFERENCES shop_items
);

ALTER TABLE pet_toy
    OWNER TO rmeunier;

CREATE TABLE drink
(
    id     uuid NOT NULL PRIMARY KEY
        CONSTRAINT fka324g1vnnohvc2wd8sv818g0i REFERENCES shop_items,
    flavor VARCHAR(100)
);

ALTER TABLE drink
    OWNER TO rmeunier;

CREATE TABLE user_inventory_items
(
    user_inventory_item_id uuid NOT NULL PRIMARY KEY,
    user_account_id        uuid NOT NULL
        CONSTRAINT fkgi7xmwy7u8vphkmqn9gpbdtix REFERENCES user_accounts,
    shop_item_id           uuid NOT NULL
        CONSTRAINT fkmp28aijr09mlagaq7e3q9ei58 REFERENCES shop_items,
    quantity               INTEGER,
    added_at               TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE user_inventory_items
    OWNER TO rmeunier;


CREATE TABLE IF NOT EXISTS public.items
(
    key character varying(50) COLLATE pg_catalog."default" NOT NULL UNIQUE,
    expiration_period int NOT NULL,
    expiration_date date,

    CONSTRAINT pkey_items  PRIMARY KEY (key)
)

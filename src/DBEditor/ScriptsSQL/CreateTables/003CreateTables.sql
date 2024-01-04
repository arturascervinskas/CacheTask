CREATE TABLE IF NOT EXISTS public.users
(
    id uuid NOT NULL DEFAULT gen_random_uuid() UNIQUE,
    name character varying(50) COLLATE pg_catalog."default" NOT NULL,
    password character varying(50) COLLATE pg_catalog."default" NOT NULL,

    CONSTRAINT pkey_users  PRIMARY KEY (id)
),

CREATE TABLE IF NOT EXISTS public.items
(
    key character varying(50) COLLATE pg_catalog."default" NOT NULL UNIQUE,
    expiration_period int NOT NULL,

    CONSTRAINT pkey_items  PRIMARY KEY (key)
)
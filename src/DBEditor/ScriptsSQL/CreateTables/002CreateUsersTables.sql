CREATE TABLE IF NOT EXISTS public.users
(
    id uuid NOT NULL DEFAULT gen_random_uuid() UNIQUE,
    name character varying(50) COLLATE pg_catalog."default" NOT NULL,
    password bytea NOT NULL,
    role int,

    CONSTRAINT pkey_users  PRIMARY KEY (id)
)
﻿-- Table: public.users

-- DROP TABLE IF EXISTS public.users;

CREATE TABLE IF NOT EXISTS public.users
(
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    username character varying(50),
    usermail character varying(100),
    alternativemail character varying(100),
    phonenumber character varying(20),
    createdby integer,
    createdon timestamp with time zone,
    modifiedby integer,
    modifiedon timestamp with time zone,
    isactive boolean,
    isdeleted boolean,
    CONSTRAINT users_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.users
    OWNER to postgres;
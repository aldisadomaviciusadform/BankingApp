CREATE TABLE IF NOT EXISTS public.accounts
(
    created timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_by character varying(50) COLLATE pg_catalog."default" NOT NULL  DEFAULT 'a'::character varying,
    is_deleted boolean DEFAULT false,
    modified timestamp without time zone,
    modified_by character varying(50) COLLATE pg_catalog."default",    

    id uuid NOT NULL DEFAULT gen_random_uuid(),
    type character varying(50) COLLATE pg_catalog."default" NOT NULL,
    balance money NOT NULL DEFAULT 0,
    user_id uuid NOT NULL,

    CONSTRAINT "pkey_accounts"  PRIMARY KEY (id),
    CONSTRAINT "fk_accounts_userId" FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
    
    CREATE UNIQUE INDEX index_user_id ON accounts(user_id);
)
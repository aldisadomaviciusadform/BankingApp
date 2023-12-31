﻿CREATE TABLE IF NOT EXISTS public.transactions
(
    created timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,        

    id uuid NOT NULL DEFAULT gen_random_uuid(),
    type character varying(50) COLLATE pg_catalog."default" NOT NULL,
    amount money NOT NULL DEFAULT 0,
    account_id uuid NOT NULL,

    CONSTRAINT "pkey_transactions" PRIMARY KEY (id),
    CONSTRAINT "fk_transactions_accountId" FOREIGN KEY (account_id)
        REFERENCES public.accounts (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID

    CREATE UNIQUE INDEX index_account_id ON transactions(account_id);
)
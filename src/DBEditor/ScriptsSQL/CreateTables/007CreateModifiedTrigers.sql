﻿CREATE OR REPLACE FUNCTION public.insert_updated_field()
RETURNS TRIGGER AS $$
BEGIN
	IF NEW.modified IS NULL THEN
   		NEW.modified := CURRENT_TIMESTAMP;
	END IF;
	
   	IF NEW.modified_by IS NULL THEN
   		NEW.modified_by := NEW.created_by;
	END IF;

   RETURN NEW;
END;
$$ LANGUAGE plpgsql;


---------CREATE-TRIGGERS-FOR-TABLES-------------

CREATE OR REPLACE TRIGGER updated_field
    BEFORE UPDATE
    ON public.users
    FOR EACH ROW
    EXECUTE FUNCTION public.insert_updated_field();

CREATE OR REPLACE TRIGGER updated_field
    BEFORE UPDATE
    ON public.accounts
    FOR EACH ROW
    EXECUTE FUNCTION public.insert_updated_field();

CREATE OR REPLACE TRIGGER updated_field
    BEFORE UPDATE
    ON public.transactions
    FOR EACH ROW
    EXECUTE FUNCTION public.insert_updated_field();

